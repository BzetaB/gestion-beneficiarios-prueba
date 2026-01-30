namespace gestion_beneficiarios.DTOs
{
    public class UpdateBeneficiaryDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? IdentityDocumentAbbreviation { get; set; }
        public string? DocumentNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public char? Gender { get; set; }
    }
}
