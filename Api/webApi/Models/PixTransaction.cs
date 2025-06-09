// Local: Api/webApi/Models/PixTransaction.cs
using webApi.Models.Base; // Supondo que ModelBase está aqui
using webApi.Models.Enum; // Se precisar de algum enum específico para status PIX
using System;
using System.ComponentModel.DataAnnotations;

namespace webApi.Models
{
    public class PixTransaction : ModelBase // Ou herde de outra base se preferir
    {
        [Required]
        public Guid DonationId { get; set; } // Chave estrangeira para a Doação relacionada
        public virtual Donation Donation { get; set; } // Propriedade de navegação

        [Required]
        public string TransactionId { get; set; } // ID da transação gerado pelo provedor PIX (txid)

        public string? EndToEndId { get; set; } // E2E ID, preenchido após a confirmação

        [Required]
        public decimal Amount { get; set; } // Valor da transação

        [Required]
        public DateTime CreationDate { get; set; } // Data de criação da cobrança PIX

        public DateTime? ExpirationDate { get; set; } // Data de expiração da cobrança PIX

        public DateTime? PaymentDate { get; set; } // Data de pagamento

        [Required]
        public string Status { get; set; } // Status da transação (ex: PENDING, PAID, EXPIRED)
        // Considere criar um Enum para Status: EPixStatus

        public string? QrCode { get; set; } // O QR Code em formato texto (string Base64 da imagem ou o payload)

        public string? CopiaECola { get; set; } // O código "copia e cola" do PIX

        public string? PayerInfo { get; set; } // Informações do pagador (pode ser JSON ou campos específicos)

        public string? ErrorMessage { get; set; } // Para registrar erros da transação, se houver

        // Adicione outros campos que julgar necessários conforme a API PIX que for utilizar
    }
}