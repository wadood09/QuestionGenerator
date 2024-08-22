using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestionGenerator.Core.Application.Exceptions;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Models.AssessmentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace QuestionGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssessmentController : ControllerBase
    {
        private readonly IAssessmentService _assessmentService;
        private readonly IAssessmentSubmissionService _assessmentSubmissionService;

        public AssessmentController(IAssessmentService assessmentService, IAssessmentSubmissionService assessmentSubmissionService)
        {
            _assessmentService = assessmentService;
            _assessmentSubmissionService = assessmentSubmissionService;
        }

        [HttpPost("{documentId}")]
        public async Task<IActionResult> TakeAssessment([FromRoute] int documentId, [FromBody] AssessmentRequest request)
        {
            try
            {
                var assessment = await _assessmentService.TakeAssessment(documentId, request);
                if (!assessment.Status)
                    return NotFound(new { message = assessment.Message });

                return Ok(new { message = assessment.Message, assessment = assessment.Value });
            }
            catch (UnAuthenticatedUserException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssessment([FromRoute] int id)
        {
            var assessment = await _assessmentService.GetAssessment(id);
            if (!assessment.Status)
                return NotFound(new { message = assessment.Message });

            return Ok(new { assessment = assessment.Value });
        }

        [HttpGet("{id}/attempts")]
        public async Task<IActionResult> GetAssessmentAttempts([FromRoute] int id)
        {
            var attempts = await _assessmentSubmissionService.GetAssessmentAttempts(id);
            return Ok(new { attempts = attempts.Value });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssessments([FromQuery] string? documentTitle)
        {
            try
            {
                var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
                if (userId == null)
                    return Unauthorized();

                var assessments = documentTitle == null ?
                    await _assessmentService.GetAssessmentsByUser(int.Parse(userId)) :
                    await _assessmentService.GetAssessmentsByDocumentTitle(documentTitle);
                return Ok(new { assessments = assessments.Value });
            }
            catch (UnAuthenticatedUserException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAssessment([FromRoute] int id)
        {
            var assessment = await _assessmentService.DeleteAssessment(id);
            if (!assessment.Status)
                return NotFound(new { message = assessment.Message });

            return Ok(new { message = assessment.Message });
        }
    }
}
