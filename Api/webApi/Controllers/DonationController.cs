using System;
using System.Linq;
using System.Security.Claims; // Importação necessária para ler o token
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Importação necessária para o [Authorize]
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApi.Context;
using webApi.Models;
using webApi.Models.Requests;
using webApi.Services;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IPaymentService _paymentService;

        public DonationController(DataContext context, IPaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }

        [Authorize] // Protege o endpoint. Apenas usuários logados podem acessá-lo.
        [HttpPost]
        public async Task<ActionResult<Donation>> CreateDonation([FromBody] DonationRequestDto donationDto)
        {
            // Extrai o ID do usuário do token (claim "NameIdentifier")
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("Token de usuário inválido ou não encontrado.");
            }

            if (!int.TryParse(userIdString, out var userId))
            {
                return BadRequest("O ID do usuário no token não é um número válido.");
            }

            // <<< CORREÇÃO AQUI >>>
            // Mapeia o DTO para a entidade, usando o ID do usuário obtido do token,
            // em vez de tentar pegar de donationDto.UserId.
            var newDonation = new Donation
            {
                DonationValue = donationDto.DonationValue,
                Description = donationDto.Description,
                InstitutionId = donationDto.InstitutionId,
                UserId = userId, // ID obtido do token
                DonationDate = DateTime.UtcNow,
                Status = "Pending"
            };

            _context.Donations.Add(newDonation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDonation), new { id = newDonation.Id }, newDonation);
        }

        [Authorize] // Também é uma boa prática proteger este endpoint
        [HttpPost("{id}/checkout")]
        public async Task<IActionResult> CreateCheckoutSession(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound("Doação não encontrada.");
            }

            // Validação de segurança: garante que o usuário logado é o "dono" da doação
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

        #region Endpoints de Leitura e Exclusão
        [HttpGet]
        public async Task<ActionResult<System.Collections.Generic.IEnumerable<Donation>>> GetDonations([FromQuery] int? userId)
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
        public async Task<IActionResult> DeleteDonation(int id)
        {
             var donation = await _context.Donations.FindAsync(id);
             if (donation == null) return NotFound("Doação não encontrada");
             _context.Donations.Remove(donation);
             await _context.SaveChangesAsync();
             return NoContent();
        }
        #endregion
    }
}
