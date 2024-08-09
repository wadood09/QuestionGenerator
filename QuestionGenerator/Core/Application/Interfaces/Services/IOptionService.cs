using QuestionGenerator.Models;
using QuestionGenerator.Models.OptionModel;

namespace QuestionGenerator.Core.Application.Interfaces.Services
{
    public interface IOptionService
    {
        Task<BaseResponse<OptionResponse>> GetOption(int id);
        Task<BaseResponse<ICollection<OptionResponse>>> GetOptionsByQuestions(int questionId);
    }
}
