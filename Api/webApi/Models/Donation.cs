using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using webApi.Models.Base;

namespace webApi.Models // Namespace correto para modelos
{
    // O nome da classe deve ser a entidade que ela representa
    public class Donation : ModelBase 
    {
        // Propriedades da sua doação
        [Required]
        public Guid? UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public int InstitutionId { get; set; }
        public virtual Institution Institution { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DonationValue { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        public DateTime DonationDate { get; set; }

        [MaxLength(50)]
        public string? PaymentMethod { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        // Coleção de transações PIX associadas
        public virtual ICollection<PixTransaction> PixTransactions { get; set; }

        // O construtor é útil para inicializar valores padrão
        public Donation()
        {
            DonationDate = DateTime.UtcNow;
            Status = "Pending";
            PixTransactions = new HashSet<PixTransaction>();
        }
    }
}