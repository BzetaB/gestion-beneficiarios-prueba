namespace gestion_beneficiarios.DTOs
{
    public class BeneficiaryDocumentDTO
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string DocumentNumber { get; set; } = null!;

        public string IdentityDocument { get; set; } = null!;
        public string Country { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
