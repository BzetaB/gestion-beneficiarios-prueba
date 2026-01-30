using gestion_beneficiarios.DTOs;
using gestion_beneficiarios.Models;
using gestion_beneficiarios.Models.Requests;

namespace gestion_beneficiarios.Services.Interfaces
{
    public interface IBeneficiaryService
    {
        Task<Beneficiary> CreateBeneficiaryAsync(BeneficiaryRequest beneficiaryRequest);
        Task<List<BeneficiaryDocumentDTO>> GetAllDocumentNumbersOfBeneficiariesAsync(bool? isActive, string? country);

        Task<Beneficiary?> UpdateBeneficiaryAsync(string documentNumber, UpdateBeneficiaryDTO updateBeneficiaryDTO);

        Task<List<BeneficiaryDocumentDTO>> SearchBeneficiariesAsync(string searchTerm, bool? isActive, string? country);
        Task DeleteBeneficiaryAsync(string documentNumber);
    }
}
