using gestion_beneficiarios.DTOs;
using gestion_beneficiarios.Models;

namespace gestion_beneficiarios.Repositories.Interfaces
{
    public interface IBeneficiaryRepository
    {
        Task<bool> GetByDocumentNumberAsync(string number);
        Task<List<BeneficiaryDocumentDTO>> GetAllDocumentNumbersOfBeneficiariesAsync(bool? isActive);
        Task<Beneficiary> CreateBeneficiaryAsync(Beneficiary beneficiary);
    }
}
