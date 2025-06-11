using webApi.Models;

public class CreatePixChargeRequest
{
    public decimal Amount { get; set; }
    public int DonationId { get; set; } // << CORRIGIDO de Guid para int
    // Outras informações que o seu provedor PIX possa precisar
}

// DTO para a resposta da criação da cobrança PIX
public class CreatePixChargeResponse
{
    public string TransactionId { get; set; } // txid
    public string QrCode { get; set; }
    public string CopiaECola { get; set; }
    public System.DateTime ExpirationDate { get; set; }
    public string Status { get; set; }
    public string? ErrorMessage { get; set; }
}

public interface IPixService
{
    Task<CreatePixChargeResponse> CreatePixChargeAsync(CreatePixChargeRequest request);
    Task<PixTransaction> GetPixTransactionStatusAsync(string transactionId);
    Task ProcessPixWebhookAsync(object webhookPayload);
}