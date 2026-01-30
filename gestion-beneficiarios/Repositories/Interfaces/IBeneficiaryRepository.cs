using gestion_beneficiarios.DTOs;
using gestion_beneficiarios.Models;

namespace gestion_beneficiarios.Repositories.Interfaces
{
    public interface IBeneficiaryRepository
    {
        Task<Beneficiary?> GetByDocumentNumberAsync(string number);
        Task<List<BeneficiaryDocumentDTO>> GetAllDocumentNumbersOfBeneficiariesAsync(bool? isActive, string? country);
        Task<Beneficiary> CreateBeneficiaryAsync(Beneficiary beneficiary);

        Task<Beneficiary> UpdateBeneficiaryAsync(Beneficiary beneficiary);

        Task<bool> DeleteByDocumentNumberAsync(string documentNumber);

        Task<List<BeneficiaryDocumentDTO>> SearchBeneficiariesAsync(string searchTerm, bool? isActive, string? country);
    }
}
