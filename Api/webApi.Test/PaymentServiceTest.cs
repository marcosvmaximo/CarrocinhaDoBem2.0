

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using webApi.Services;
using Xunit;

namespace webApi.Test
{
    public class PaymentServiceTests
    {
        private class FakeHttpMessageHandler : HttpMessageHandler
        {
            private readonly Func<HttpRequestMessage, HttpResponseMessage> _sendFunc;

            public FakeHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> sendFunc)
            {
                _sendFunc = sendFunc;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_sendFunc(request));
            }
        }

        private IConfiguration GetConfiguration(string baseUrl)
        {
            var inMemorySettings = new System.Collections.Generic.Dictionary<string, string> {
                {"FakePspSettings:BaseUrl", baseUrl}
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Fact]
        public async Task CreateChargeAsync_DeveRetornarSucesso()
        {
            var expected = new CreateChargeResponse
            {
                ChargeId = "123",
                CheckoutUrl = "http://checkout.com"
            };

            var handler = new FakeHttpMessageHandler(request =>
            {
                var json = JsonSerializer.Serialize(expected);
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
            });

            var client = new HttpClient(handler);
            var config = GetConfiguration("http://fakepsp.com");

            var service = new PaymentService(client, config);
            var requestDto = new CreateChargeRequest
            {
                Amount = 10,
                Description = "Teste",
                DonationId = 1,
                SuccessUrl = "http://sucesso.com",
                CancelUrl = "http://cancelar.com"
            };

            var result = await service.CreateChargeAsync(requestDto);

            Assert.Equal(expected.ChargeId, result.ChargeId);
            Assert.Equal(expected.CheckoutUrl, result.CheckoutUrl);
            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public async Task CreateChargeAsync_DeveRetornarErroEmCasoDeFalhaNoPSP()
        {
            var handler = new FakeHttpMessageHandler(request =>
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Erro simulado")
                };
            });

            var client = new HttpClient(handler);
            var config = GetConfiguration("http://fakepsp.com");

            var service = new PaymentService(client, config);
            var requestDto = new CreateChargeRequest();

            var result = await service.CreateChargeAsync(requestDto);

            Assert.Null(result.ChargeId);
            Assert.Contains("Erro ao comunicar com o PSP", result.ErrorMessage);
        }

        [Fact]
        public async Task CreateChargeAsync_DeveRetornarErroInterno_EmCasoDeExcecao()
        {
            var handler = new FakeHttpMessageHandler(request =>
            {
                throw new Exception("Falha simulada");
            });

            var client = new HttpClient(handler);
            var config = GetConfiguration("http://fakepsp.com");

            var service = new PaymentService(client, config);
            var requestDto = new CreateChargeRequest();

            var result = await service.CreateChargeAsync(requestDto);

            Assert.Null(result.ChargeId);
            Assert.Contains("Erro interno", result.ErrorMessage);
        }

        [Fact]
        public void Construtor_DeveLancarExcecao_SeBaseUrlNaoConfigurada()
        {
            var config = GetConfiguration("");

            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                var client = new HttpClient();
                var service = new PaymentService(client, config);
            });

            Assert.Contains("FakePSP não está configurada", ex.Message);
        }
    }
}