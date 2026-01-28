using Azure.Core;
using gestion_beneficiarios.DTOs;
using gestion_beneficiarios.Models;
using gestion_beneficiarios.Models.Requests;
using gestion_beneficiarios.Repositories;
using gestion_beneficiarios.Repositories.Interfaces;
using gestion_beneficiarios.Services.Interfaces;

namespace gestion_beneficiarios.Services
{
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IIdentityDocumentRepository _identityDocumentRepository;
        private readonly IIdentityDocumentService _identityDocumentService;

        public BeneficiaryService(IBeneficiaryRepository beneficiaryRepository, 
            IIdentityDocumentRepository identityDocumentRepository,
            IIdentityDocumentService identityDocumentService)
        {
            _beneficiaryRepository = beneficiaryRepository;
            _identityDocumentRepository = identityDocumentRepository;
            _identityDocumentService = identityDocumentService;
        }

        public async Task<Beneficiary> CreateBeneficiaryAsync(BeneficiaryRequest request)
        {
            var identityDocument = await _identityDocumentRepository
                .GetByAbbreviationAsync(request.IdentityDocumentAbbreviation);

            if (identityDocument is null || !identityDocument.IsActive)
                throw new ArgumentException("Invalid or inactive identity document");

            _identityDocumentService.ValidateDocumentNumber(request.DocumentNumber, identityDocument);

            var exist = await _beneficiaryRepository.GetByDocumentNumberAsync(request.DocumentNumber);
            if(exist)
                throw new InvalidOperationException("A beneficiary with this document number already exists.");

            if (request.BirthDate > DateTime.Today)
                throw new ArgumentException("The birth date cannot be later than today.");

            var beneficiary = new Beneficiary
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                IdentityDocumentId = identityDocument.Id,
                DocumentNumber = request.DocumentNumber,
                BirthDate = request.BirthDate,
                Gender = char.ToUpper(request.Gender)
            };

            return await _beneficiaryRepository.CreateBeneficiaryAsync(beneficiary);
        }

        public async Task<List<BeneficiaryDocumentDTO>> GetAllDocumentNumbersOfBeneficiariesAsync(bool? isActive)
        {
            if (isActive != null && isActive != true && isActive != false)
                throw new ArgumentException("Invalid filter value");

            var result = await _beneficiaryRepository.GetAllDocumentNumbersOfBeneficiariesAsync(isActive);

            return [.. result.OrderBy(x => x.LastName)];
        }
    }
}
