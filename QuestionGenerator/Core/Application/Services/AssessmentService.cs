using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using OpenAI_API;
using OpenAI_API.Completions;
using Org.BouncyCastle.Asn1.Ocsp;
using QuestionGenerator.Core.Application.Config;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Core.Domain.Enums;
using QuestionGenerator.Models;
using QuestionGenerator.Models.AssessmentModel;
using System.Text.Json;

namespace QuestionGenerator.Core.Application.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly OpenAiConfig _openAiConfig;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IOptionRepository _optionRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssessmentService(IOptions<OpenAiConfig> openAiConfig, IAssessmentRepository assessmentRepository, IUnitOfWork unitOfWork, IDocumentRepository documentRepository, IOptionRepository optionRepository, IQuestionRepository questionRepository)
        {
            _openAiConfig = openAiConfig.Value;
            _assessmentRepository = assessmentRepository;
            _unitOfWork = unitOfWork;
            _documentRepository = documentRepository;
            _optionRepository = optionRepository;
            _questionRepository = questionRepository;
        }

        public async Task<BaseResponse> CreateAssessment(int documentId, AssessmentRequest request)
        {
            var document = await _documentRepository.GetAsync(documentId);
            if (document == null)
            {
                return new BaseResponse
                {
                    Message = "Document does not exists",
                    Status = false
                };
            }

            var openApi = new OpenAIAPI(_openAiConfig.ApiKey);
            var documentContent = File.ReadAllLines($"C:\\Users\\WADOOD\\OneDrive\\Desktop\\QuestionGenerator\\QuestionGenerator\\Files\\{document.DocumentUrl}");
            var prompt = GetPrompt(request.AssessmentType, request.QuestionCount, documentContent);
            var completionRequest = new CompletionRequest
            {
                Prompt = prompt,
                MaxTokens = 1000
            };

            var result = await openApi.Completions.CreateCompletionAsync(completionRequest);
            var assessment = new Assessment
            {
                AssessmentType = request.AssessmentType,
                CreatedBy = "0",
                DateCreated = DateTime.Now,
                DocumentId = documentId
            };

            var questions = new List<Question>();
            var options = new List<Domain.Entities.Option>();
            if (request.AssessmentType == AssessmentType.MultipleChoice) 
            {
                var parsedResult = ParsePromptResultToClass<MultipleChoice>(result.Completions[0].Text);
                foreach (var item in parsedResult)
                {
                    var question = new Question
                    {
                        Answer = item.Answer,
                        QuestionText = item.Question,
                        AssessmentId = assessment.Id,
                        Assessment = assessment,
                        CreatedBy = "0",
                        DateCreated = DateTime.Now,
                        Elucidation = item.Explanation
                    };
                    questions.Add(question);

                    var questionOptions = item.Options.Select(x => new Domain.Entities.Option
                    {
                        QuestionId = question.Id,
                        Question = question,
                        CreatedBy = "0",
                        DateCreated = DateTime.Now,
                        OptionText = x
                    });
                    options.AddRange(questionOptions);
                }
            }
            else if(request.AssessmentType == AssessmentType.TrueFalse)
            {
                var parsedResult = ParsePromptResultToClass<TrueFalse>(result.Completions[0].Text);
                foreach (var item in parsedResult)
                {
                    var question = new Question
                    {
                        AssessmentId = assessment.Id,
                        Assessment = assessment,
                        DateCreated = DateTime.Now,
                        CreatedBy = "0",
                        Answer = item.Answer,
                        QuestionText = item.Statement,
                        Elucidation = item.Explanation
                    };
                    questions.Add(question);
                }
            }
            else if(request.AssessmentType == AssessmentType.FillInTheBlanks)
            {
                var parsedResult = ParsePromptResultToClass<FillInTheBlanks>(result.Completions[0].Text);
                foreach (var item in parsedResult)
                {
                    var question = new Question
                    {
                        Assessment = assessment,
                        AssessmentId = assessment.Id,
                        DateCreated = DateTime.Now,
                        CreatedBy = "0",
                        QuestionText = item.Sentence,
                        Answer = item.Answer
                    };
                    questions.Add(question);
                }
            }
            else
            {
                var parsedResult = ParsePromptResultToClass<Flashcard>(result.Completions[0].Text);
                foreach (var item in parsedResult)
                {
                    
                }
            }

            foreach (var item in parsedResult)
            {
                var question = new Question
                {
                    AssessmentId = assessment.Id,
                    DateCreated = DateTime.Now,
                    QuestionText = item.FF
                }
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

        private List<T> ParsePromptResultToClass<T>(string result)
        {
            var promptResult = JsonSerializer.Deserialize<List<T>>(result);
            return promptResult;
        }

        private string GetPrompt(AssessmentType type, int count, string[] documentContent)
        {
            if (type == AssessmentType.MultipleChoice)
            {
                return $"Generate {count} multiple-choice questions in JSON format based on the following document:" +
                    $"\n\n{documentContent}" +
                    $"\n\n Each question should have a question text, four options, the correct answer, and an elucidation. " +
                    $"The JSON structure should be as follows:" +
                    @"{
                            'question': 'What is the capital of Japan?',
                            'options': ['A. Seoul', 'B. Beijing', 'C. Tokyo', 'D. Bangkok'],
                            'answer': 'C. Tokyo',
                            'explanation': 'Tokyo is the capital city of Japan.'
                        }";
            }
            else if (type == AssessmentType.TrueFalse)
            {
                return $"Generate {count} true or false questions in JSON format based on the following document:" +
                    $"\n\n{documentContent} " +
                    $"Each question should have a statement, a boolean value indicating the correct answer, and an explanation. " +
                    $"The JSON structure should be as follows:" +
                    @"{
                           'statement': 'Tokyo is the capital of Japan.',
                            'answer': true,
                            'explanation': 'Tokyo is indeed the capital city of Japan.'
                       }";
            }
            else if (type == AssessmentType.FillInTheBlanks)
            {
                return $"Generate {count} fill-in-the-gaps questions in JSON format based on the following document:" +
                    $"\n\n{documentContent}" +
                    $"\n\n Each question should have a sentence with a blank and the correct answer. " +
                    $"The JSON structure should be as follows:" +
                    @"{
                            'sentence': 'Tokyo is the capital of ____.',
                            'answer': 'Japan'
                        }";
            }
            else
            {
                return $"Generate {count} flashcards in JSON format based on the following document:" +
                    $"\n\n{documentContent}" +
                    $"\n\n Each flashcard should have a question, an answer, and an elucidation. " +
                    $"The JSON structure should be as follows:" +
                    @"{
                            'question': 'What is the capital of Japan?',
                            'answer': 'Tokyo',
                            'explanation': 'Tokyo is the capital city of Japan.'
                        }";
            }
        }

        private class Flashcard
        {
            public string Question { get; set; }
            public string Answer { get; set; }
            public string Explanation { get; set; }
        }

        private class FillInTheBlanks
        {
            public string Sentence { get; set; }
            public string Answer { get; set; }
        }

        private class TrueFalse
        {
            public string Statement { get; set; }
            public string Answer { get; set; }
            public string Explanation { get; set; }
        }

        private class MultipleChoice
        {
            public string Question { get; set; }
            public List<string> Options { get; set; }
            public string Answer { get; set; }
            public string Explanation { get; set; }
        }
    }
}
