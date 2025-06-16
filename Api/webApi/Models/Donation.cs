using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using webApi.Models.Base;

namespace webApi.Models
{
    public class Donation : ModelBase 
    {
        [Required]
        public int UserId { get; set; }
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

        public virtual ICollection<PixTransaction> PixTransactions { get; set; }

        public Donation()
        {
            DonationDate = DateTime.UtcNow;
            Status = "Pending";
            PixTransactions = new HashSet<PixTransaction>();
        }
    }
}