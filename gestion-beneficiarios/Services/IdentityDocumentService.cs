using Azure.Core;
using gestion_beneficiarios.DTOs;
using gestion_beneficiarios.Models;
using gestion_beneficiarios.Models.Requests;
using gestion_beneficiarios.Repositories;
using gestion_beneficiarios.Repositories.Interfaces;
using gestion_beneficiarios.Services.Interfaces;

namespace gestion_beneficiarios.Services
{
    public class IdentityDocumentService : IIdentityDocumentService
    {
        private readonly IIdentityDocumentRepository _repository;

        public IdentityDocumentService (IIdentityDocumentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IdentityDocument> CreateIndentityDocument(IdentityDocumentRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var existing = await _repository.GetByAbbreviationAsync(request.Abbreviation);

            if (existing != null)
                throw new InvalidOperationException(
                    $"An identity document with abbreviation '{request.Abbreviation}' already exists.");

            var identityDocument = new IdentityDocument
            {
                Name = request.Name,
                Abbreviation = request.Abbreviation.ToUpper(),
                Country = request.Country,
                Length = request.Length,
                IsNumeric = request.IsNumeric,
                IsActive = request.IsActive
            };

            return await _repository.CreateIdentityDocumentAsync(identityDocument);
        }

        public async Task<List<IdentityDocument>> GetAllAsync(bool? isActive)
        {
            if (isActive != null && isActive != true && isActive != false)
                throw new ArgumentException("Invalid filter value");

            return await _repository.GetAllAsync(isActive);
        }

        public async Task<List<CountryAbbreviationDTO>> GetAllCountriesAsync()
        {
            return await _repository.GetAllCountriesAsync();
        }

        public void ValidateDocumentNumber(string documentNumber, IdentityDocument identityDocument)
        {
            if (identityDocument is null)
                throw new ArgumentNullException(nameof(identityDocument));

            // Validar longitud
            if (documentNumber.Length != identityDocument.Length)
                throw new ArgumentException(
                    $"The document number must be {identityDocument.Length} characters long.");

            // Validar formato (solo números)
            if (identityDocument.IsNumeric && !documentNumber.All(char.IsDigit))
                throw new ArgumentException(
                    "The document number must contain only digits.");
        }
    }
}
