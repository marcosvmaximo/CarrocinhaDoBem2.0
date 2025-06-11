using System.Threading.Tasks;

namespace webApi.Services
{
    // DTO para solicitar a criação de uma cobrança ao nosso PSP
    public class CreateChargeRequest
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int DonationId { get; set; }
        public string SuccessUrl { get; set; } // URL para onde o usuário será redirecionado em caso de sucesso
        public string CancelUrl { get; set; } // URL para onde o usuário será redirecionado em caso de cancelamento
    }

    // DTO para a resposta do PSP
    public class CreateChargeResponse
    {
        public string ChargeId { get; set; } // ID da cobrança gerado pelo PSP
        public string CheckoutUrl { get; set; } // URL para a página de pagamento
        public string? ErrorMessage { get; set; }
    }

    // A interface do nosso serviço de pagamento
    public interface IPaymentService
    {
        // O método agora retorna uma URL de checkout em vez de dados de PIX
        Task<CreateChargeResponse> CreateChargeAsync(CreateChargeRequest request);
    }
}