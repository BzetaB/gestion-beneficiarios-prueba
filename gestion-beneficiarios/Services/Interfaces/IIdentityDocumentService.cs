using gestion_beneficiarios.Models;
using gestion_beneficiarios.Models.Requests;

namespace gestion_beneficiarios.Services.Interfaces
{
    public interface IIdentityDocumentService
    {
        Task<IdentityDocument> CreateIndentityDocument(IdentityDocumentRequest identityDocumentRequest);
        Task<List<IdentityDocument>> GetAllAsync(bool? isActive);
        void ValidateDocumentNumber(string documentNumber, IdentityDocument identityDocument);
    }
}
