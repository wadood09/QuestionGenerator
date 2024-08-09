using QuestionGenerator.Models;
using QuestionGenerator.Models.QuestionModel;

namespace QuestionGenerator.Core.Application.Interfaces.Services
{
    public interface IQuestionService
    {
        Task<BaseResponse<QuestionResponse>> GetQuestion(int id);
        Task<BaseResponse<ICollection<QuestionResponse>>> GetQuestionByAssessment(int assessmentId);
    }
}
