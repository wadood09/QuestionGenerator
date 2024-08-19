using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestionGenerator.Core.Application.Exceptions;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Models.AssessmentModel;

namespace QuestionGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentController : ControllerBase
    {
        private readonly IAssessmentService _assessmentService;

        public AssessmentController(IAssessmentService assessmentService)
        {
            _assessmentService = assessmentService;
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAssessment([FromRoute] int id)
        {
            var assessment = await _assessmentService.DeleteAssessment(id);
            if (!assessment.Status)
                return NotFound(new {message = assessment.Message});

            return Ok(new { message = assessment.Message });
        }
    }
}
