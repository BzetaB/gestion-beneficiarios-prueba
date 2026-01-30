using gestion_beneficiarios.Context;
using gestion_beneficiarios.DTOs;
using gestion_beneficiarios.Models;
using gestion_beneficiarios.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace gestion_beneficiarios.Repositories
{
    public class IdentityDocumentRepository : IIdentityDocumentRepository
    {
        private readonly AppDbContext _context;

        public IdentityDocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IdentityDocument> CreateIdentityDocumentAsync(IdentityDocument identityDocument)
        {
            ArgumentNullException.ThrowIfNull(identityDocument);

            await _context.IdentityDocuments.AddAsync(identityDocument);
            await _context.SaveChangesAsync();

            return identityDocument;
        }

        public async Task<List<IdentityDocument>> GetAllAsync(bool? isActive)
        {
            var isActiveParam = new SqlParameter("@IsActive",
                isActive.HasValue ? isActive.Value : (object)DBNull.Value);

            return await _context
                .Set<IdentityDocument>()
                .FromSqlRaw("EXEC sp_IdentityDocument_GetAll @IsActive", isActiveParam)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<CountryAbbreviationDTO>> GetAllCountriesAsync()
        {
            return await _context.IdentityDocuments
                .AsNoTracking()
                .Where(d => d.Country != null && d.Abbreviation != null)
                .Select(d => new CountryAbbreviationDTO
                {
                    Country = d.Country!,
                    Abbreviation = d.Abbreviation!
                })
                .Distinct()
                .OrderBy(x => x.Country)
                .ToListAsync();
        }

        public async Task<IdentityDocument?> GetByAbbreviationAsync(string abbreviation)
        {
            return await _context.IdentityDocuments
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Abbreviation.Trim() == abbreviation.Trim() && i.IsActive);
        }
    }
}
