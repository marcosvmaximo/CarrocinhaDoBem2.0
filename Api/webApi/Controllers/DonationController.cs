using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApi.Models;
using webApi.Context; 
using webApi.Services; 

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IPixService _pixService; // Serviço para lógica de negócio do PIX

        public DonationController(DataContext context, IPixService pixService)
        {
            _context = context;
            _pixService = pixService;
        }

        // GET: api/Donation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Donation>>> GetDonations([FromQuery] int? userId)
        {
            var query = _context.Donations.AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(d => d.UserId == userId.Value);
            }

            var donations = await query
                                .Include(d => d.User)
                                .Include(d => d.Institution)
                                .ToListAsync();

            if (!donations.Any())
            {
                return NotFound("Nenhuma doação encontrada.");
            }

            return Ok(donations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Donation>> GetDonation(int id)
        {
            var donation = await _context.Donations
                                .Include(d => d.User)
                                .Include(d => d.Institution)
                                .FirstOrDefaultAsync(d => d.Id == id);

            if (donation == null)
            {
                return NotFound("Doação não encontrada");
            }

            return donation;
        }

        // POST: api/Donation (Para criar o registro da doação ANTES de gerar o PIX)
        [HttpPost]
        public async Task<ActionResult<Donation>> PostDonation([FromBody] Donation donationRequest)
        {
            if (donationRequest.DonationValue <= 0)
            {
                ModelState.AddModelError("DonationValue", "O valor da doação deve ser maior que zero.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var newDonation = new Donation
            {
                DonationValue = donationRequest.DonationValue,
                Description = donationRequest.Description,
                InstitutionId = donationRequest.InstitutionId,
                UserId = donationRequest.UserId, // Pode ser nulo para doadores anônimos
                DonationDate = DateTime.UtcNow,
                Status = "Pending" // Status inicial antes de tentar o pagamento
            };

            _context.Donations.Add(newDonation);
            await _context.SaveChangesAsync();

            // Retorna a doação criada, incluindo o ID gerado, para que o frontend possa usá-lo
            return CreatedAtAction(nameof(GetDonation), new { id = newDonation.Id }, newDonation);
        }

        // POST: api/Donation/{id}/pix (Para gerar a cobrança PIX para uma doação existente)
        [HttpPost("{id}/pix")]
        public async Task<IActionResult> CreatePixForDonation(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound(new { message = "Doação não encontrada." });
            }

            if (donation.Status != "Pending")
            {
                return BadRequest(new { message = "Esta doação não está pendente e não pode gerar uma nova cobrança PIX." });
            }

            var chargeRequest = new CreatePixChargeRequest
            {
                DonationId = donation.Id,
                Amount = donation.DonationValue
            };

            var pixResponse = await _pixService.CreatePixChargeAsync(chargeRequest);

            if (!string.IsNullOrEmpty(pixResponse.ErrorMessage))
            {
                return StatusCode(500, new { message = "Erro ao gerar cobrança PIX.", details = pixResponse.ErrorMessage });
            }
            
            // Atualiza o status da doação para indicar que a cobrança PIX foi gerada
            donation.Status = "AwaitingPayment";
            donation.PaymentMethod = "PIX";
            _context.Entry(donation).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Cobrança PIX gerada com sucesso. Aguardando pagamento.",
                donationId = donation.Id,
                qrCode = pixResponse.QrCode,
                copiaECola = pixResponse.CopiaECola,
                expirationDate = pixResponse.ExpirationDate
            });
        }

        // DELETE: api/Donation/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonation(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound("Doação nao encontrada");
            }

            _context.Donations.Remove(donation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DonationExists(int id)
        {
            return _context.Donations.Any(e => e.Id == id);
        }
    }
}