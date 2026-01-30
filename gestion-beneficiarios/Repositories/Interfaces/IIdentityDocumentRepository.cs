using gestion_beneficiarios.DTOs;
using gestion_beneficiarios.Models;

namespace gestion_beneficiarios.Repositories.Interfaces
{
    public interface IIdentityDocumentRepository
    {
        Task<IdentityDocument> CreateIdentityDocumentAsync(IdentityDocument identityDocument);
        Task<List<IdentityDocument>> GetAllAsync(bool? isActive);
        Task<IdentityDocument?> GetByAbbreviationAsync(string abbreviation);

        Task<List<CountryAbbreviationDTO>> GetAllCountriesAsync();
    }
}
