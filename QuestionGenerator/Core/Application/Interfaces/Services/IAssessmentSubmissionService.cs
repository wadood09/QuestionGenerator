using QuestionGenerator.Models;
using QuestionGenerator.Models.AssessmentModel;
using QuestionGenerator.Models.AssessmentSubmissionModel;

namespace QuestionGenerator.Core.Application.Interfaces.Services
{
    public interface IAssessmentSubmissionService
    {
        Task<BaseResponse> SubmitAssessment(AssessmentSubmissionRequest request);
        Task<BaseResponse<ICollection<AssessmentAttemptsResponse>>> GetAssessmentAttempts(int assessmentId);
        Task<BaseResponse<AssessmentAttemptResponse>> GetAssessmentAttempt(int id);
        Task<BaseResponse> RemoveAssessmentAttempt(int id);
    }
}
