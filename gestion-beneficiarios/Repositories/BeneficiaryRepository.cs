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

        public async Task<bool> GetByDocumentNumberAsync(string number)
        {
            return await _context
                .Beneficiaries
                .AnyAsync(b => b.DocumentNumber == number);
        }

        public async Task<List<BeneficiaryDocumentDTO>> GetAllDocumentNumbersOfBeneficiariesAsync(bool? isActive)
        {
            var isActiveParam = new SqlParameter("@IsActive", 
                isActive.HasValue ? isActive.Value : (object)DBNull.Value);

            return await _context
                .Set<BeneficiaryDocumentDTO>()
                .FromSqlRaw("EXEC sp_Beneficiary_GetDocumentNumbers @IsActive", isActiveParam)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
