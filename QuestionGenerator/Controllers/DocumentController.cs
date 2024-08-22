using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestionGenerator.Core.Application.Exceptions;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Models.DocumentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace QuestionGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocument(DocumentRequest request)
        {
            try
            {
                await _documentService.CreateDocument(request);
                return Ok(new { message = "Document uploaded successfully" });
            }
            catch (UnAuthenticatedUserException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (DocumentAlreadyExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (InvalidDocumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnsupportedDocumentTypeException ex)
            {
                return StatusCode(415, new { message = ex.Message });
            }
            catch (FileTooLargeException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (FileTypeRestrictedException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while processing the document." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocument([FromRoute] int id)
        {
            var document = await _documentService.GetDocument(id);
            if (!document.Status)
                return NotFound(new { message = document.Message });

            return Ok(new { document = document.Value });
        }

        [HttpGet]
        public async Task<IActionResult> GetDocument([FromQuery] string? title)
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (userId == null)
                return Unauthorized();

            if (title == null)
            {
                var documents = await _documentService.GetDocumentsByUser(int.Parse(userId));
                return Ok(new { documents = documents.Value });
            }

            var document = await _documentService.GetDocument(int.Parse(userId), title);
            if (!document.Status)
                return NotFound(new { message = document.Message });

            return Ok(new { documents = document.Value });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveDocument([FromRoute] int id)
        {
            var document = await _documentService.DeleteDocument(id);
            if (!document.Status)
                return NotFound(new { message = document.Message });

            return Ok(new { message = document.Message });
        }
    }
}
