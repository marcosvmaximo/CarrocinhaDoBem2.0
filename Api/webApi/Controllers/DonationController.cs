using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApi.Context;
using webApi.Models;
using webApi.Services; // Usando o novo serviço

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

        // POST: api/donations
        [HttpPost]
        public async Task<ActionResult<Donation>> CreateDonation([FromBody] Donation donationRequest)
        {
            if (donationRequest.DonationValue <= 0)
            {
                return BadRequest("O valor da doação deve ser maior que zero.");
            }

            var newDonation = new Donation
            {
                DonationValue = donationRequest.DonationValue,
                Description = donationRequest.Description,
                InstitutionId = donationRequest.InstitutionId,
                UserId = donationRequest.UserId,
                DonationDate = DateTime.UtcNow,
                Status = "Pending"
            };

            _context.Donations.Add(newDonation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDonation), new { id = newDonation.Id }, newDonation);
        }
        
        // NOVO ENDPOINT para iniciar o pagamento
        [HttpPost("{id}/checkout")]
        public async Task<IActionResult> CreateCheckoutSession(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound("Doação não encontrada.");
            }

            // Monta o pedido para o nosso FakePSP
            var chargeRequest = new CreateChargeRequest
            {
                DonationId = donation.Id,
                Amount = donation.DonationValue,
                Description = $"Doação para {donation.Description}",
                // URLs para onde o PSP deve redirecionar o usuário após o pagamento
                SuccessUrl = "http://localhost:4200/donation/success", // Rota no seu Angular
                CancelUrl = "http://localhost:4200/donation/cancel"    // Rota no seu Angular
            };

            var pspResponse = await _paymentService.CreateChargeAsync(chargeRequest);

            if (!string.IsNullOrEmpty(pspResponse.ErrorMessage))
            {
                return StatusCode(500, new { message = "Erro ao iniciar sessão de pagamento.", details = pspResponse.ErrorMessage });
            }

            // Atualiza o status da doação para indicar que o pagamento foi iniciado
            donation.Status = "ProcessingPayment";
            donation.PaymentMethod = "PSP_FAKE";
            // Você pode querer salvar o pspResponse.ChargeId na sua entidade de transação aqui
            await _context.SaveChangesAsync();

            // Retorna o objeto com a URL de checkout para o frontend
            return Ok(new { checkoutUrl = pspResponse.CheckoutUrl });
        }

        // O endpoint de webhook ficará em um controller separado (`PaymentsController`).

        #region Endpoints de Leitura e Exclusão (sem alterações)
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
             if (donation == null) return NotFound("Doação nao encontrada");
             _context.Donations.Remove(donation);
             await _context.SaveChangesAsync();
             return NoContent();
        }
        #endregion
    }
}
