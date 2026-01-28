using gestion_beneficiarios.Models.Requests;
using gestion_beneficiarios.Services;
using gestion_beneficiarios.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace gestion_beneficiarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityDocumentsController : ControllerBase
    {
        private readonly IIdentityDocumentService _service;

        public IdentityDocumentsController(IIdentityDocumentService service)
        {
            _service = service;
        }

        [HttpGet("identity-documents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetIdentityDocuments([FromQuery] bool? isActive)
        {
            try
            {
                var result = await _service
                    .GetAllAsync(isActive);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("identity-documents")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateIdentityDocument([FromBody] IdentityDocumentRequest request)
        {
            if (request == null)
                return BadRequest(new { message = "Request body cannot be null." });

            try
            {
                var createdDocument = await _service.CreateIndentityDocument(request);

                // Devuelve 201 Created con el objeto recién creado
                return StatusCode(StatusCodes.Status201Created, createdDocument);
            }
            catch (ArgumentException ex)
            {
                // Validaciones de campos obligatorios, longitud, etc.
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Documento con la misma abreviación ya existe
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
