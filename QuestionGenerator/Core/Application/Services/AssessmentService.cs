using Microsoft.Extensions.Options;
using OpenAI_API;
using QuestionGenerator.Core.Application.Config;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Core.Domain.Enums;
using QuestionGenerator.Models;
using QuestionGenerator.Models.AssessmentModel;

namespace QuestionGenerator.Core.Application.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly OpenAiConfig _openAiConfig;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssessmentService(IOptions<OpenAiConfig> openAiConfig, IAssessmentRepository assessmentRepository, IUnitOfWork unitOfWork, IDocumentRepository documentRepository)
        {
            _openAiConfig = openAiConfig.Value;
            _assessmentRepository = assessmentRepository;
            _unitOfWork = unitOfWork;
            _documentRepository = documentRepository;
        }

        public async Task<BaseResponse> CreateAssessment(int documentId, AssessmentRequest request)
        {
            var document = await _documentRepository.GetAsync(documentId);
            if(document == null)
            {
                return new BaseResponse
                {
                    Message = "Document does not exists",
                    Status = false
                };
            }

            var openApi = new OpenAIAPI(_openAiConfig.ApiKey);
            string prompt;
            if(request.AssessmentType == AssessmentType.MultipleChoice)
            {
                prompt = $"Generate {request.QuestionCount} MultipleChoice questions with four options each and an answer with elucidations";
            }
        }

        public Task<BaseResponse<AssessmentResponse>> GetAssessment(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<ICollection<AssessmentResponse>>> GetAssessmentByDocument(int documentId)
        {
            throw new NotImplementedException();
        }
    }
}
