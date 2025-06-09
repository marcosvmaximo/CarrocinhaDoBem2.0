using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApi.Models;
using webApi.Context; // Namespace correto para DataContext
// using webApi.Services; // Descomente quando IPixService e CreatePixChargeRequest estiverem definidos

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationController : ControllerBase
    {
        private readonly DataContext _context;
        // private readonly IPixService _pixService; // Descomente quando IPixService estiver pronto

        public DonationController(DataContext context /*, IPixService pixService */)
        {
            _context = context;
            // _pixService = pixService; // Descomente quando IPixService estiver pronto
        }

        // GET: api/Donation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Donation>>> GetDonations([FromQuery] Guid? userId) // UserId continua Guid?
        {
            var query = _context.Donations.AsQueryable();

            if (userId.HasValue)
            {
                // Assumindo que Donation.UserId é Guid?
                query = query.Where(d => d.UserId == userId.Value);
            }

            var donations = await query.ToListAsync();

            if (!donations.Any())
            {
                return NotFound("Nenhuma doação encontrada.");
            }

            return Ok(donations);
        }

        // GET: api/Donation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Donation>> GetDonation(int id) // Alterado de volta para int
        {
            // Assumindo que Donation.Id (ou ModelBase.Id) é int
            var donation = await _context.Donations.FindAsync(id);

            if (donation == null)
            {
                return NotFound("Doação não encontrada");
            }

            return donation;
        }

        // POST: api/Donation
        [HttpPost]
        public async Task<ActionResult<Donation>> PostDonation(Donation donation)
        {
            // Se o Id for int e gerado pelo banco (Identity), você geralmente não o define aqui.
            // Se não for identity e for 0 (valor padrão para int), pode ser um indicativo de novo registro.
            // A lógica de atribuição de Id pode variar dependendo da sua configuração de chave primária.
            // Se o Id é identity, o EF Core cuida disso.

            donation.DonationDate = DateTime.UtcNow; // Garante que a data é definida no momento da criação

            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();

            // CreatedAtAction espera que o 'id' no objeto de rota corresponda ao tipo do parâmetro 'id' de GetDonation
            return CreatedAtAction(nameof(GetDonation), new { id = donation.Id }, donation);
        }

        /*
        // Endpoint para criar uma cobrança PIX para uma doação
        // Descomente e ajuste quando IPixService e CreatePixChargeRequest estiverem prontos
        // Lembre-se de que chargeRequest.DonationId precisará ser do tipo correto (int, neste caso)
        [HttpPost("pix")]
        public async Task<IActionResult> CreatePixDonation([FromBody] CreatePixChargeRequest chargeRequest) // chargeRequest.DonationId deve ser int
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var donation = await _context.Donations.FindAsync(chargeRequest.DonationId); // DonationId aqui deve ser int
            if (donation == null)
            {
                return NotFound(new { message = "Doação não encontrada." });
            }
            if (donation.Amount != chargeRequest.Amount)
            {
                 return BadRequest(new { message = "O valor da requisição PIX não corresponde ao valor da doação." });
            }

            donation.Status = "PENDING_PIX_PAYMENT";
            donation.PaymentMethod = "PIX";
            _context.Entry(donation).State = EntityState.Modified;

            // var pixResponse = await _pixService.CreatePixChargeAsync(chargeRequest);

            // if (!string.IsNullOrEmpty(pixResponse.ErrorMessage))
            // {
            //     return StatusCode(500, new { message = "Erro ao gerar cobrança PIX.", details = pixResponse.ErrorMessage });
            // }
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                // Logar o erro
                return StatusCode(500, new { message = "Erro ao salvar dados da doação após gerar PIX.", details = ex.Message });
            }

            // return Ok(new
            // {
            //     message = "Cobrança PIX gerada com sucesso. Aguardando pagamento.",
            //     donationId = donation.Id, // Este será int
            //     pixTransactionId = pixResponse.TransactionId,
            //     qrCode = pixResponse.QrCode,
            //     copiaECola = pixResponse.CopiaECola,
            //     expirationDate = pixResponse.ExpirationDate
            // });

            return Ok(new { message = "Endpoint PIX a ser implementado com _pixService" }); // Placeholder
        }
        */

        // DELETE: api/Donation/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonation(int id) // Alterado de volta para int
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

        private bool DonationExists(int id) // Alterado de volta para int
        {
            // A comparação e => e.Id == id agora deve funcionar corretamente
            return _context.Donations.Any(e => e.Id == id);
        }
    }
}