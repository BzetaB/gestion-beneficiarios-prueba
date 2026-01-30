using gestion_beneficiarios.Context;
using gestion_beneficiarios.DTOs;
using gestion_beneficiarios.Models;
using gestion_beneficiarios.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace gestion_beneficiarios.Repositories
{
    public class BeneficiaryRepository : IBeneficiaryRepository
    {
        private readonly AppDbContext _context;

        public BeneficiaryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Beneficiary> CreateBeneficiaryAsync(Beneficiary beneficiary)
        {
            ArgumentNullException.ThrowIfNull(beneficiary);

            await _context.Beneficiaries.AddAsync(beneficiary);
            await _context.SaveChangesAsync();

            return beneficiary;
        }

        public async Task<Beneficiary?> GetByDocumentNumberAsync(string number)
        {
            return await _context.Set<Beneficiary>()
                             .FirstOrDefaultAsync(b => b.DocumentNumber == number);
        }

        public async Task<List<BeneficiaryDocumentDTO>> GetAllDocumentNumbersOfBeneficiariesAsync(bool? isActive, string? country)
        {
            var isActiveParam = new SqlParameter("@IsActive", 
                isActive.HasValue ? isActive.Value : (object)DBNull.Value);

            var isCountryParam = new SqlParameter("@Country", 
                !string.IsNullOrEmpty(country) ? country : (object)DBNull.Value);

            return await _context
                .Set<BeneficiaryDocumentDTO>()
                .FromSqlRaw("EXEC sp_Beneficiary_GetDocumentNumbers @IsActive, @Country", isActiveParam, isCountryParam)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Beneficiary> UpdateBeneficiaryAsync(Beneficiary beneficiary)
        {
            _context.Set<Beneficiary>().Update(beneficiary); 
            await _context.SaveChangesAsync();
            return beneficiary;
        }

        public async Task<bool> DeleteByDocumentNumberAsync(string documentNumber)
        {
            var beneficiary = await _context.Set<Beneficiary>()
                                        .FirstOrDefaultAsync(b => b.DocumentNumber == documentNumber);

            if (beneficiary == null)
                return false;

            _context.Set<Beneficiary>().Remove(beneficiary);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<BeneficiaryDocumentDTO>> SearchBeneficiariesAsync(string searchTerm, bool? isActive, string? country)
        {
            var result = await _context
                .Set<BeneficiaryDocumentDTO>()
                .FromSqlRaw("EXEC sp_Beneficiaries_Search @SearchTerm = {0}, @IsActive = {1}, @Country = {2}",
                    searchTerm ?? string.Empty,
                    isActive,
                    country)
                .AsNoTracking()
                .ToListAsync();

            return result;
        }
    }
}
