// Local: Api/webApi/Models/PixTransaction.cs
using webApi.Models.Base; // Supondo que ModelBase está aqui
using webApi.Models.Enum; // Se precisar de algum enum específico para status PIX
using System;
using System.ComponentModel.DataAnnotations;

namespace webApi.Models
{
    public class PixTransaction : ModelBase 
    {
        [Required]
        public int DonationId { get; set; } 
        public virtual Donation Donation { get; set; }

        [Required]
        public string TransactionId { get; set; } 

        public string? EndToEndId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        [Required]
        public string Status { get; set; }

        public string? QrCode { get; set; }

        public string? CopiaECola { get; set; }

        public string? PayerInfo { get; set; }

        public string? ErrorMessage { get; set; }
    }
}