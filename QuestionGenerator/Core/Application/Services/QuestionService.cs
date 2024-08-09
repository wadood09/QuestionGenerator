using AutoMapper;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Models;
using QuestionGenerator.Models.QuestionModel;

namespace QuestionGenerator.Core.Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public QuestionService(IQuestionRepository questionRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public Task<BaseResponse<QuestionResponse>> GetQuestion(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<ICollection<QuestionResponse>>> GetQuestionByAssessment(int assessmentId)
        {
            throw new NotImplementedException();
        }
    }
}
