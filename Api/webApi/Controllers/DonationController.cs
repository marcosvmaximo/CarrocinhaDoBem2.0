using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using webApi.Context;
using webApi.Models;
using webApi.Models.Requests;
using webApi.Services;

namespace webApi.Controllers
{
    // O DTO para o webhook pode ser definido aqui ou em um arquivo separado
    public class PaymentWebhookPayload
    {
        public string EventType { get; set; }
        public int DonationId { get; set; }
        public string ChargeId { get; set; }
        public decimal AmountPaid { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class DonationController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IPaymentService _paymentService;
        private readonly ILogger<DonationController> _logger;

        public DonationController(DataContext context, IPaymentService paymentService, ILogger<DonationController> logger)
        {
            _context = context;
            _paymentService = paymentService;
            _logger = logger;
        }

        // --- ENDPOINTS PARA UTILIZADORES LOGADOS ---

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Donation>> CreateDonation([FromBody] DonationRequestDto donationDto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Token de utilizador inválido.");
            }

            var newDonation = new Donation
            {
                DonationValue = donationDto.DonationValue,
                Description = donationDto.Description,
                InstitutionId = donationDto.InstitutionId,
                UserId = userId,
                DonationDate = DateTime.UtcNow,
                Status = "Pending"
            };

            _context.Donations.Add(newDonation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDonation), new { id = newDonation.Id }, newDonation);
        }

        [HttpPost("{id}/checkout")]
        [Authorize]
        public async Task<IActionResult> CreateCheckoutSession(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null) return NotFound("Doação não encontrada.");

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (donation.UserId.ToString() != userIdString)
            {
                return Forbid("Você não tem permissão para pagar esta doação.");
            }

            var chargeRequest = new CreateChargeRequest
            {
                DonationId = donation.Id,
                Amount = donation.DonationValue,
                Description = $"Doação: {donation.Description}",
                SuccessUrl = "http://localhost:4200/donation/success",
                CancelUrl = "http://localhost:4200/donation/cancel"
            };

            var pspResponse = await _paymentService.CreateChargeAsync(chargeRequest);

            if (!string.IsNullOrEmpty(pspResponse.ErrorMessage))
            {
                return StatusCode(500, new { message = "Erro ao iniciar sessão de pagamento.", details = pspResponse.ErrorMessage });
            }

            donation.Status = "ProcessingPayment";
            donation.PaymentMethod = "PSP_FAKE";
            await _context.SaveChangesAsync();

            return Ok(new { checkoutUrl = pspResponse.CheckoutUrl });
        }

        // --- ENDPOINT PARA O WEBHOOK DO PSP ---

        [HttpPost("webhook")]
        public async Task<IActionResult> HandleWebhook([FromBody] PaymentWebhookPayload payload)
        {
            _logger.LogInformation("--> Webhook recebido para a doação ID: {DonationId}", payload.DonationId);
            if (payload.EventType == "payment.succeeded")
            {
                var donation = await _context.Donations.FindAsync(payload.DonationId);
                if (donation != null)
                {
                    donation.Status = "Paid";
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("--> Doação ID {DonationId} atualizada para 'Paid'.", payload.DonationId);
                    return Ok(new { message = "Webhook processado com sucesso." });
                }
                _logger.LogError("--> Doação com ID {DonationId} não encontrada.", payload.DonationId);
                return NotFound();
            }
            return BadRequest();
        }

        // --- ENDPOINTS PARA ADMINS ---

        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Donation>>> GetAllDonations()
        {
             var donations = await _context.Donations.Include(d => d.User).Include(d => d.Institution).OrderByDescending(d => d.DonationDate).ToListAsync();
             if (!donations.Any()) return NotFound("Nenhuma doação encontrada.");
             return Ok(donations);
        }

        [HttpGet("admin/summary")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetDonationSummary()
        {
             var totalValuePaid = await _context.Donations.Where(d => d.Status == "Paid").SumAsync(d => d.DonationValue);
             var totalDonations = await _context.Donations.CountAsync();
             var donationsByStatus = await _context.Donations.GroupBy(d => d.Status).Select(g => new { Status = g.Key, Count = g.Count() }).ToListAsync();
             var summary = new { TotalDonations = totalDonations, TotalValuePaid = totalValuePaid, DonationsByStatus = donationsByStatus };
             return Ok(summary);
        }

        // --- ENDPOINTS PÚBLICOS OU DE LEITURA GERAL ---

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Donation>>> GetDonations([FromQuery] int? userId)
        {
             var query = _context.Donations.AsQueryable();
             if (userId.HasValue)
             {
                 query = query.Where(d => d.UserId == userId.Value);
             }
             var donations = await query.Include(d => d.User).Include(d => d.Institution).ToListAsync();
             if (!donations.Any()) return NotFound("Nenhuma doação encontrada.");
             return Ok(donations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Donation>> GetDonation(int id)
        {
             var donation = await _context.Donations.Include(d => d.User).Include(d => d.Institution).FirstOrDefaultAsync(d => d.Id == id);
             if (donation == null) return NotFound("Doação não encontrada");
             return donation;
        }

        [HttpDelete("{id}")]
        [Authorize] // Apenas utilizadores logados podem apagar as suas próprias doações (lógica a ser adicionada)
        public async Task<IActionResult> DeleteDonation(int id)
        {
             var donation = await _context.Donations.FindAsync(id);
             if (donation == null) return NotFound("Doação não encontrada");
             _context.Donations.Remove(donation);
             await _context.SaveChangesAsync();
             return NoContent();
        }
    }
}
