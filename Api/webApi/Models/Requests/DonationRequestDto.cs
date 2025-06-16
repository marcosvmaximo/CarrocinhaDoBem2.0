using System.ComponentModel.DataAnnotations;

namespace webApi.Models.Requests
{
    // Este objeto representa os dados que o frontend envia para criar uma doação.
    public class DonationRequestDto
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor da doação deve ser maior que zero.")]
        public decimal DonationValue { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "A instituição é obrigatória.")]
        public int InstitutionId { get; set; }

    }
}
