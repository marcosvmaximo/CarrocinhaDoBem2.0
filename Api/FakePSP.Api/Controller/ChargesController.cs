using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Net.Http.Json; // Adicionado para PostAsJsonAsync/JsonContent
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Globalization; // Adicionado para o Sanitize

namespace FakePSP.Api.Controllers
{
    // --- Modelos de Dados ---
    public class CreateChargeRequestDto
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int DonationId { get; set; }
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
    }

    public class Charge
    {
        public string ChargeId { get; set; } = Guid.NewGuid().ToString("N");
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int DonationId { get; set; }
        public string Status { get; set; } = "PENDING";
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }

        // <<< CORREÇÃO AQUI >>>
        // A URL agora aponta para o endpoint correto no DonationController da sua API principal.
        public string WebhookUrl { get; set; } = "https://localhost:7001/api/donation/webhook";

        public string PixPayload { get; set; }
    }

    public class PaymentWebhookPayload
    {
        public string EventType { get; set; }
        public int DonationId { get; set; }
        public string ChargeId { get; set; }
        public decimal AmountPaid { get; set; }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class ChargesController : ControllerBase
    {
        private static readonly ConcurrentDictionary<string, Charge> _charges = new();
        private readonly IHttpClientFactory _httpClientFactory;

        private const string PixKey = "seu.email@dominio.com.br";
        private const string MerchantName = "ONG CARROCINHA DO BEM";
        private const string MerchantCity = "CURITIBA";

        public ChargesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

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

            newCharge.PixPayload = GeneratePixPayload(newCharge.Amount);

            _charges[newCharge.ChargeId] = newCharge;
            var checkoutUrl = Url.Action("ShowCheckoutPage", "Charges", new { chargeId = newCharge.ChargeId }, Request.Scheme);

            Console.WriteLine($"[FakePSP] Cobrança PIX criada: {newCharge.ChargeId}. URL de Checkout: {checkoutUrl}");

            return Ok(new { chargeId = newCharge.ChargeId, checkoutUrl });
        }

        [HttpGet("checkout/{chargeId}")]
        public IActionResult ShowCheckoutPage(string chargeId)
        {
            if (!_charges.TryGetValue(chargeId, out var charge) || charge.Status != "PENDING")
            {
                return NotFound("Página de pagamento inválida ou já processada.");
            }

            var html = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <title>Checkout PIX - FakePSP</title>
                    <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css"" rel=""stylesheet"">
                    <script src=""https://cdnjs.cloudflare.com/ajax/libs/qrcodejs/1.0.0/qrcode.min.js""></script>
                    <style>
                        body {{ display: flex; align-items: center; justify-content: center; height: 100vh; background-color: #f0f2f5; }}
                        .card {{ padding: 2.5rem; box-shadow: 0 6px 20px rgba(0,0,0,0.1); border: none; border-radius: 1rem; width: 100%; max-width: 450px; }}
                        #qrcode {{ padding: 10px; background: white; border-radius: 8px; margin-bottom: 1.5rem; }}
                        #qrcode img {{ margin: auto; }}
                        .input-group-text {{ cursor: pointer; }}
                    </style>
                </head>
                <body>
                    <div class=""card text-center"">
                        <h1 class=""mb-2"">Pague com PIX</h1>
                        <p class=""text-muted mb-4"">Para doar <strong>R$ {charge.Amount:F2}</strong> para <strong>{MerchantName}</strong></p>

                        <div id=""qrcode"" class=""d-flex justify-content-center""></div>

                        <p class=""fw-bold mt-2"">PIX Copia e Cola</p>
                        <div class=""input-group mb-4"">
                            <input type=""text"" id=""pixCode"" class=""form-control"" value=""{charge.PixPayload}"" readonly>
                            <span class=""input-group-text"" onclick=""copyCode()"">&#128203;</span>
                        </div>

                        <p class=""text-muted small"">Após pagar no app do seu banco, clique em 'Já Paguei!' abaixo.</p>

                        <div class=""d-grid gap-2 mt-2"">
                            <form method=""post"" action=""{Url.Action("ConfirmPayment", "Charges", new { chargeId = charge.ChargeId }, Request.Scheme)}"">
                                <button type=""submit"" class=""btn btn-success btn-lg w-100"">Já Paguei!</button>
                            </form>
                            <a href=""{charge.CancelUrl}"" class=""btn btn-link text-danger"">Cancelar Doação</a>
                        </div>
                    </div>
                    <script>
                        new QRCode(document.getElementById('qrcode'), {{
                            text: '{charge.PixPayload}',
                            width: 220,
                            height: 220,
                        }});
                        function copyCode() {{
                            const pixCodeInput = document.getElementById('pixCode');
                            pixCodeInput.select();
                            document.execCommand('copy');
                            alert('Código PIX copiado!');
                        }}
                    </script>
                </body>
                </html>";

            return Content(html, "text/html", Encoding.UTF8);
        }

        [HttpPost("confirm/{chargeId}")]
        public async Task<IActionResult> ConfirmPayment(string chargeId)
        {
            if (!_charges.TryGetValue(chargeId, out var charge) || charge.Status != "PENDING")
            {
                return Content("Pagamento inválido ou já processado.");
            }

            charge.Status = "PAID";
            Console.WriteLine($"[FakePSP] Confirmação manual recebida para a cobrança: {chargeId}");

            var webhookPayload = new PaymentWebhookPayload
            {
                EventType = "payment.succeeded",
                DonationId = charge.DonationId,
                ChargeId = charge.ChargeId,
                AmountPaid = charge.Amount
            };

            var client = _httpClientFactory.CreateClient();
            var content = JsonContent.Create(webhookPayload);

            try
            {
                Console.WriteLine($"[FakePSP] Enviando webhook para {charge.WebhookUrl}");
                await client.PostAsync(charge.WebhookUrl, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FakePSP] ERRO ao enviar webhook: {ex.Message}");
            }

            return Redirect(charge.SuccessUrl);
        }

        #region Métodos Auxiliares para Geração do Payload PIX
        private string GeneratePixPayload(decimal amount, string txId = "***")
        {
            var payload = new StringBuilder();
            payload.Append(FormatField("00", "01"));
            payload.Append(BuildMerchantAccountInfo());
            payload.Append(FormatField("52", "0000"));
            payload.Append(FormatField("53", "986"));
            payload.Append(FormatField("54", amount.ToString("F2", CultureInfo.InvariantCulture)));
            payload.Append(FormatField("58", "BR"));
            payload.Append(FormatField("59", SanitizeText(MerchantName, 25)));
            payload.Append(FormatField("60", SanitizeText(MerchantCity, 15)));
            payload.Append(FormatField("62", FormatField("05", txId)));

            payload.Append("6304");
            string crc16 = CalculateCrc16(payload.ToString());
            payload.Append(crc16);

            return payload.ToString();
        }

        private string BuildMerchantAccountInfo()
        {
            return FormatField("26", $"{FormatField("00", "br.gov.bcb.pix")}{FormatField("01", PixKey)}");
        }

        private string FormatField(string id, string value)
        {
            return $"{id}{value.Length:D2}{value}";
        }

        private string SanitizeText(string text, int maxLength)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            var sanitized = stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToUpper();

            return sanitized.Length > maxLength ? sanitized.Substring(0, maxLength) : sanitized;
        }

        private string CalculateCrc16(string data)
        {
            ushort crc = 0xFFFF;
            ushort polynomial = 0x1021;
            foreach (byte b in Encoding.ASCII.GetBytes(data))
            {
                crc ^= (ushort)(b << 8);
                for (int i = 0; i < 8; i++)
                {
                    crc = (crc & 0x8000) != 0 ? (ushort)((crc << 1) ^ polynomial) : (ushort)(crc << 1);
                }
            }
            return crc.ToString("X4");
        }
        #endregion
    }
}
