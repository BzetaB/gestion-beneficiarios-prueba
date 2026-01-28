using System.ComponentModel.DataAnnotations;

namespace gestion_beneficiarios.Models
{
    public class IdentityDocument
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string Abbreviation { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Country { get; set; } = null!;

        [Required]
        [Range(1, 50, ErrorMessage = "Length must be greater than 0")]
        public int Length { get; set; }

        [Required]
        public bool IsNumeric { get; set; }

        [Required]
        public bool IsActive { get; set; }

    }
}
