using QuestionGenerator.Models;
using QuestionGenerator.Models.AssessmentModel;

namespace QuestionGenerator.Core.Application.Interfaces.Services
{
    public interface IAssessmentService
    {
        Task<BaseResponse<AssessmentResponse>> GetAssessment(int id);
        Task<BaseResponse<ICollection<AssessmentsResponse>>> GetAssessmentsByDocument(int documentId);
        Task<BaseResponse<ICollection<AssessmentsResponse>>> GetAssessmentsByUser(int userId);
        Task<BaseResponse<AssessmentResponse>> TakeAssessment(int documentId, AssessmentRequest request);
    }
}
