using gestion_beneficiarios.DTOs;
using gestion_beneficiarios.Models;
using gestion_beneficiarios.Models.Requests;

namespace gestion_beneficiarios.Services.Interfaces
{
    public interface IBeneficiaryService
    {
        Task<Beneficiary> CreateBeneficiaryAsync(BeneficiaryRequest beneficiaryRequest);
        Task<List<BeneficiaryDocumentDTO>> GetAllDocumentNumbersOfBeneficiariesAsync(bool? isActive);

    }
}
