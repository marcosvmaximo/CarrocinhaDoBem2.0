using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace FakePSP.Api.Controllers
{
    

    // O que recebemos da API da ONG
    public class CreateChargeRequestDto
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int DonationId { get; set; }
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
    }

    // O que guardamos em "memória"
    public class Charge
    {
        public string ChargeId { get; set; } = Guid.NewGuid().ToString("N");
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int DonationId { get; set; }
        public string Status { get; set; } = "PENDING";
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
        public string WebhookUrl { get; set; } = "https://localhost:7001/api/payments/webhook"; // URL do webhook da sua API principal
    }
    
    // Payload que enviamos de volta para a ONG via Webhook
    public class PaymentWebhookPayload
    {
        public string EventType { get; set; } // "payment.succeeded"
        public int DonationId { get; set; }
        public string ChargeId { get; set; }
        public decimal AmountPaid { get; set; }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class ChargesController : ControllerBase
    {
        // Usamos um dicionário estático para simular um banco de dados em memória.
        // Isso mantém os dados vivos enquanto a API estiver rodando.
        private static readonly ConcurrentDictionary<string, Charge> _charges = new();
        private readonly IHttpClientFactory _httpClientFactory;

        public ChargesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // POST: api/charges
        // Endpoint que a API da ONG vai chamar para criar uma cobrança.
        [HttpPost]
        public IActionResult CreateCharge([FromBody] CreateChargeRequestDto request)
        {
            var newCharge = new Charge
            {
                Amount = request.Amount,
                Description = request.Description,
                DonationId = request.DonationId,
                SuccessUrl = request.SuccessUrl,
                CancelUrl = request.CancelUrl
            };

            _charges[newCharge.ChargeId] = newCharge;

            // Gera a URL de checkout que o usuário final vai visitar
            var checkoutUrl = Url.Action("ShowCheckoutPage", "Charges", new { chargeId = newCharge.ChargeId }, Request.Scheme);
            
            Console.WriteLine($"[FakePSP] Cobrança criada: {newCharge.ChargeId} para a Doação {newCharge.DonationId}. URL de Checkout: {checkoutUrl}");

            return Ok(new { chargeId = newCharge.ChargeId, checkoutUrl });
        }

        // GET: api/charges/checkout/{chargeId}
        // Endpoint que serve a página HTML de checkout.
        [HttpGet("checkout/{chargeId}")]
        public IActionResult ShowCheckoutPage(string chargeId)
        {
            if (!_charges.TryGetValue(chargeId, out var charge) || charge.Status != "PENDING")
            {
                return NotFound("Página de pagamento inválida ou já processada.");
            }

            // Geramos um HTML simples para a página de checkout
            var html = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <title>Checkout FakePSP</title>
                    <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css"" rel=""stylesheet"">
                    <style>
                        body {{ display: flex; align-items: center; justify-content: center; height: 100vh; background-color: #f8f9fa; }}
                        .card {{ padding: 2rem; box-shadow: 0 4px 8px rgba(0,0,0,0.1); }}
                    </style>
                </head>
                <body>
                    <div class=""card text-center"">
                        <h1 class=""mb-3"">Pagamento para Carrocinha do Bem</h1>
                        <p class=""lead"">Você está doando <strong>R$ {charge.Amount:F2}</strong>.</p>
                        <p>Descrição: {charge.Description}</p>
                        <div class=""d-grid gap-2 mt-4"">
                            <form method=""post"" action=""{Url.Action("ConfirmPayment", "Charges", new { chargeId = charge.ChargeId }, Request.Scheme)}"">
                                <button type=""submit"" class=""btn btn-success btn-lg"">Confirmar Pagamento</button>
                            </form>
                            <a href=""{charge.CancelUrl}"" class=""btn btn-danger btn-lg"">Cancelar</a>
                        </div>
                    </div>
                </body>
                </html>";

            return Content(html, "text/html", Encoding.UTF8);
        }

        // POST: api/charges/confirm/{chargeId}
        // Endpoint que o botão "Confirmar Pagamento" chama.
        [HttpPost("confirm/{chargeId}")]
        public async Task<IActionResult> ConfirmPayment(string chargeId)
        {
            if (!_charges.TryGetValue(chargeId, out var charge) || charge.Status != "PENDING")
            {
                return Content("Pagamento inválido ou já processado.");
            }
            
            // 1. Simula o pagamento bem-sucedido
            charge.Status = "PAID";
            Console.WriteLine($"[FakePSP] Pagamento confirmado para a cobrança: {chargeId}");

            // 2. Prepara o webhook para notificar a API da ONG
            var webhookPayload = new PaymentWebhookPayload
            {
                EventType = "payment.succeeded",
                DonationId = charge.DonationId,
                ChargeId = charge.ChargeId,
                AmountPaid = charge.Amount
            };

            // 3. Envia o webhook para a API principal
            try
            {
                var client = _httpClientFactory.CreateClient();
                var jsonPayload = JsonSerializer.Serialize(webhookPayload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                Console.WriteLine($"[FakePSP] Enviando webhook para {charge.WebhookUrl} com payload: {jsonPayload}");
                await client.PostAsync(charge.WebhookUrl, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FakePSP] ERRO ao enviar webhook: {ex.Message}");
                // Em um sistema real, haveria um sistema de retentativas (retry).
            }
            
            // 4. Redireciona o navegador do usuário para a URL de sucesso
            return Redirect(charge.SuccessUrl);
        }
    }
}
