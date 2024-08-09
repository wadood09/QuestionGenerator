using QuestionGenerator.Models;
using QuestionGenerator.Models.AssessmentSubmissionModel;

namespace QuestionGenerator.Core.Application.Interfaces.Services
{
    public interface IAssessmentSubmissionService
    {
        Task<BaseResponse> SubmitAssessment(AssessmentSubmissionRequest request);
    }
}
