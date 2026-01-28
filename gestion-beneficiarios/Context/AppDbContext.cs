using gestion_beneficiarios.DTOs;
using gestion_beneficiarios.Models;
using Microsoft.EntityFrameworkCore;
namespace gestion_beneficiarios.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<IdentityDocument> IdentityDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityDocument>()
                .ToTable("IdentityDocument");

            modelBuilder.Entity<Beneficiary>()
                .ToTable("Beneficiary");

            modelBuilder.Entity<BeneficiaryDocumentDTO>().HasNoKey();
        }
    }
}
