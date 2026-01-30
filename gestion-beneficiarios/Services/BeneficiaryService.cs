using Azure.Core;
using gestion_beneficiarios.DTOs;
using gestion_beneficiarios.Models;
using gestion_beneficiarios.Models.Requests;
using gestion_beneficiarios.Repositories;
using gestion_beneficiarios.Repositories.Interfaces;
using gestion_beneficiarios.Services.Interfaces;
using Humanizer;
using NuGet.Protocol.Core.Types;

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
            if(exist != null)
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

        public async Task DeleteBeneficiaryAsync(string documentNumber)
        {
            if (string.IsNullOrWhiteSpace(documentNumber))
                throw new ArgumentException("Document number is required.", nameof(documentNumber));

            bool deleted = await _beneficiaryRepository.DeleteByDocumentNumberAsync(documentNumber);
            if (!deleted)
                throw new KeyNotFoundException($"Beneficiary with DocumentNumber '{documentNumber}' not found.");
        }

        public async Task<List<BeneficiaryDocumentDTO>> GetAllDocumentNumbersOfBeneficiariesAsync(bool? isActive, string? country)
        {
            if (isActive != null && isActive != true && isActive != false)
                throw new ArgumentException("Invalid filter value");

            if (!string.IsNullOrWhiteSpace(country))
            {
                var validCountries = await _identityDocumentService.GetAllCountriesAsync();

                var isValidCountry = validCountries
                    .Any(c => c.Country.Equals(country, StringComparison.OrdinalIgnoreCase));

                if (!isValidCountry)
                    throw new ArgumentException("Invalid country filter");
            }

            var result = await _beneficiaryRepository.GetAllDocumentNumbersOfBeneficiariesAsync(isActive, country);

            return [.. result.OrderBy(x => x.LastName)];
        }

        public async Task<List<BeneficiaryDocumentDTO>> SearchBeneficiariesAsync(string searchTerm, bool? isActive, string? country)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllDocumentNumbersOfBeneficiariesAsync(isActive, country);
            }

            return await _beneficiaryRepository.SearchBeneficiariesAsync(searchTerm, isActive, country);
        }

        public async Task<Beneficiary?> UpdateBeneficiaryAsync(string documentNumber, UpdateBeneficiaryDTO dto)
        {
            if (string.IsNullOrWhiteSpace(documentNumber))
                throw new ArgumentException("Document number is required.", nameof(documentNumber));

            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var existing = await _beneficiaryRepository.GetByDocumentNumberAsync(documentNumber);
            if (existing == null)
                throw new KeyNotFoundException($"Beneficiary with DocumentNumber '{documentNumber}' not found.");

            // Actualizamos solo campos que no sean nulos
            if (!string.IsNullOrWhiteSpace(dto.FirstName))
                existing.FirstName = dto.FirstName;

            if (!string.IsNullOrWhiteSpace(dto.LastName))
                existing.LastName = dto.LastName;

            if (!string.IsNullOrWhiteSpace(dto.DocumentNumber))
                existing.DocumentNumber = dto.DocumentNumber;

            if (dto.BirthDate.HasValue)
                existing.BirthDate = dto.BirthDate.Value;

            if (dto.Gender.HasValue)
            {
                if (dto.Gender != 'M' && dto.Gender != 'F')
                    throw new ArgumentException("Gender must be 'M' or 'F'.");
                existing.Gender = dto.Gender.Value;
            }

            // Validación y asignación por abreviatura de documento
            if (!string.IsNullOrWhiteSpace(dto.IdentityDocumentAbbreviation))
            {
                var identityDoc = await _identityDocumentRepository.GetByAbbreviationAsync(dto.IdentityDocumentAbbreviation);
                if (identityDoc == null)
                    throw new ArgumentException($"IdentityDocument with abbreviation '{dto.IdentityDocumentAbbreviation}' does not exist or is inactive.");

                existing.IdentityDocumentId = identityDoc.Id;
            }

            return await _beneficiaryRepository.UpdateBeneficiaryAsync(existing);
        }
    }
}
