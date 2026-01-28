using System.ComponentModel.DataAnnotations;

namespace gestion_beneficiarios.Models.Requests
{
    public class IdentityDocumentRequest
    {
        [Required]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(10, ErrorMessage = "Abbreviation cannot be longer than 10 characters.")]
        public string Abbreviation { get; set; } = null!;

        [Required]
        [StringLength(50, ErrorMessage = "Country cannot be longer than 50 characters.")]
        public string Country { get; set; } = null!;

        [Required]
        [Range(1, 50, ErrorMessage = "Length must be between 1 and 50.")]
        public int Length { get; set; }

        [Required]
        public bool IsNumeric { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
