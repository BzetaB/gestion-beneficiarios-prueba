using System.ComponentModel.DataAnnotations;

namespace gestion_beneficiarios.Models.Requests
{
    public class BeneficiaryRequest
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string IdentityDocumentAbbreviation { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string DocumentNumber { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        [RegularExpression("^[MFmf]$", ErrorMessage = "Gender must be M or F")]
        public char Gender { get; set; }
    }
}
