using System.ComponentModel.DataAnnotations;

namespace gestion_beneficiarios.Models
{
    public class Beneficiary
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = null!;

        [Required]
        public int IdentityDocumentId { get; set; }

        [Required]
        [StringLength(20)]
        public string DocumentNumber { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        [RegularExpression("^[MF]$", ErrorMessage = "Gender must be M or F")]
        public char Gender { get; set; }

    }
}
