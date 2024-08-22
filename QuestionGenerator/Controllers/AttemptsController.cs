using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestionGenerator.Core.Application.Exceptions;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Models.AssessmentSubmissionModel;

namespace QuestionGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttemptsController : ControllerBase
    {
        private readonly IAssessmentSubmissionService _assessmentSubmissionService;

        public AttemptsController(IAssessmentSubmissionService assessmentSubmissionService)
        {
            _assessmentSubmissionService = assessmentSubmissionService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAssessment([FromBody] AssessmentSubmissionRequest request)
        {
            try
            {
                var submission = await _assessmentSubmissionService.SubmitAssessment(request);
                if (!submission.Status)
                    return NotFound(new { message = submission.Message });

                return Ok(new { message = submission.Message });
            }
            catch (UnAuthenticatedUserException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssessmentAttempt([FromRoute] int id)
        {
            var attempt = await _assessmentSubmissionService.GetAssessmentAttempt(id);
            if(!attempt.Status)
                return NotFound(new {message = attempt.Message});

            return Ok(new { attempt = attempt.Value });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAssessmentAttempt([FromRoute] int id)
        {
            var attempt = await _assessmentSubmissionService.RemoveAssessmentAttempt(id);
            if (!attempt.Status)
                return NotFound(new { message = attempt.Message });

            return Ok(new { message = attempt.Message });
        }
    }
}
