using AutoMapper;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Models;
using QuestionGenerator.Models.OptionModel;

namespace QuestionGenerator.Core.Application.Services
{
    public class OptionService : IOptionService
    {
        private readonly IOptionRepository _optionRepository;
        private readonly IMapper _mapper;

        public OptionService(IOptionRepository optionRepository, IMapper mapper)
        {
            _optionRepository = optionRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<OptionResponse>> GetOption(int id)
        {
            var option = await _optionRepository.GetAsync(id);
            if(option == null)
            {
                return new BaseResponse<OptionResponse>
                {
                    Message = "Option not found",
                    Status = false
                };
            }

            var response = _mapper.Map<OptionResponse>(option);
            return new BaseResponse<OptionResponse>
            {
                Message = "Option found successfully",
                Status = true,
                Value = response
            };
        }

        public async Task<BaseResponse<ICollection<OptionResponse>>> GetOptionsByQuestions(int questionId)
        {
            var options = await _optionRepository.GetAllAsync(x => x.QuestionId == questionId);
            var response = _mapper.Map<List<OptionResponse>>(options);
            return new BaseResponse<ICollection<OptionResponse>>
            {
                Message = "List of options",
                Status = true,
                Value = response
            };
        }
    }
}
