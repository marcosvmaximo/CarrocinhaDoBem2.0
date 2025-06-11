using System;
using System.Net.Http;
using System.Net.Http.Json; // Requer o pacote System.Net.Http.Json
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using webApi.Models; // Para acessar a entidade Donation

namespace webApi.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PaymentService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            // Pega a URL base da nossa futura API FakePSP do appsettings.json
            var pspBaseUrl = _configuration["FakePspSettings:BaseUrl"];
            if (string.IsNullOrEmpty(pspBaseUrl))
            {
                throw new InvalidOperationException("A URL base do FakePSP não está configurada em appsettings.json");
            }
            _httpClient.BaseAddress = new Uri(pspBaseUrl);
        }

        public async Task<CreateChargeResponse> CreateChargeAsync(CreateChargeRequest request)
        {
            try
            {
                // Faz uma chamada POST para o endpoint /api/charges da nossa API FakePSP
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/charges", request);

                if (response.IsSuccessStatusCode)
                {
                    // Se a chamada foi bem-sucedida, lê a resposta e a retorna
                    var chargeResponse = await response.Content.ReadFromJsonAsync<CreateChargeResponse>();
                    return chargeResponse;
                }
                else
                {
                    // Se houve um erro, retorna a mensagem de erro
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new CreateChargeResponse { ErrorMessage = $"Erro ao comunicar com o PSP: {response.StatusCode} - {errorContent}" };
                }
            }
            catch (Exception ex)
            {
                // Captura exceções de rede ou outras
                return new CreateChargeResponse { ErrorMessage = $"Erro interno: {ex.Message}" };
            }
        }
    }
}
