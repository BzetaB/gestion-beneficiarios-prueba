using gestion_beneficiarios.Context;
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

        [HttpGet("documents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDocumentNumbers([FromQuery] bool? isActive)
        {
            try
            {
                var result = await _beneficiaryService
                    .GetAllDocumentNumbersOfBeneficiariesAsync(isActive);
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
                var beneficiary = await _beneficiaryService.CreateBeneficiaryAsync(request);
                return StatusCode(201, beneficiary); ;
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
    }
}
