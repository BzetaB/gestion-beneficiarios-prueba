using gestion_beneficiarios.Context;
using gestion_beneficiarios.DTOs;
using gestion_beneficiarios.Models;
using gestion_beneficiarios.Models.Requests;
using gestion_beneficiarios.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestion_beneficiarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiariesController : ControllerBase
    {
        private readonly IBeneficiaryService _beneficiaryService;

        public BeneficiariesController(IBeneficiaryService beneficiaryService)
        {
            _beneficiaryService = beneficiaryService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<BeneficiaryDocumentDTO>>> SearchBeneficiaries(
        [FromQuery] string searchTerm,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? country = null)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest(new { message = "El término de búsqueda no puede estar vacío." });
            }

            var beneficiaries = await _beneficiaryService.SearchBeneficiariesAsync(searchTerm, isActive, country);
            return Ok(beneficiaries);
        }

        [HttpPatch("{documentNumber}")]
        public async Task<IActionResult> UpdateBeneficiaryPartial(string documentNumber, [FromBody] UpdateBeneficiaryDTO dto)
        {
            try
            {
                var updated = await _beneficiaryService.UpdateBeneficiaryAsync(documentNumber, dto);
                return Ok(new { message = "Beneficiary updated succesfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error while updating beneficiary.");
            }
        }

        [HttpGet("documents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDocumentNumbers([FromQuery] bool? isActive, [FromQuery] string? country)
        {
            try
            {
                var result = await _beneficiaryService
                    .GetAllDocumentNumbersOfBeneficiariesAsync(isActive, country);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BeneficiaryRequest request)
        {
            try
            {
                await _beneficiaryService.CreateBeneficiaryAsync(request);
                return StatusCode(201, new {message = "Beneficiary created succesfully" }); ;
            }
            catch (ArgumentException ex)
            {
                // Formato inválido, longitud, fecha de nacimiento futura, etc.
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Ya existe beneficiario con ese documento
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Errores inesperados
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpDelete("{documentNumber}")]
        public async Task<IActionResult> DeleteBeneficiary(string documentNumber)
        {
            try
            {
                await _beneficiaryService.DeleteBeneficiaryAsync(documentNumber);
                return Ok($"Beneficiary with DocumentNumber '{documentNumber}' deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error while deleting beneficiary.");
            }
        }
    }
}
