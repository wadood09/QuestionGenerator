using QuestionGenerator.Models;
using QuestionGenerator.Models.AssessmentModel;

namespace QuestionGenerator.Core.Application.Interfaces.Services
{
    public interface IAssessmentService
    {
        Task<BaseResponse> CreateAssessment(int documentId, AssessmentRequest request);
        Task<BaseResponse<AssessmentResponse>> GetAssessment(int id);
        Task<BaseResponse<ICollection<AssessmentResponse>>> GetAssessmentByDocument(int documentId);
    }
}
