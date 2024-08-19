﻿using AutoMapper;
using Microsoft.Extensions.Options;
using OpenAI_API;
using OpenAI_API.Completions;
using QuestionGenerator.Core.Application.Config;
using QuestionGenerator.Core.Application.Exceptions;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Core.Domain.Enums;
using QuestionGenerator.Infrastructure.Repositories;
using QuestionGenerator.Models;
using QuestionGenerator.Models.AssessmentModel;
using QuestionGenerator.Models.AssessmentSubmissionModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace QuestionGenerator.Core.Application.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly OpenAiConfig _openAiConfig;
        private readonly StorageConfig _storageConfig;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IQuestionResultRepository _questionResultRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IOptionRepository _optionRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAssessmentSubmissionRepository _assessmentSubmissionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AssessmentService(IOptions<OpenAiConfig> openAiConfig, IOptions<StorageConfig> storageConfig, IAssessmentRepository assessmentRepository, IUnitOfWork unitOfWork, IDocumentRepository documentRepository, IOptionRepository optionRepository, IQuestionRepository questionRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IMapper mapper, IAssessmentSubmissionRepository assessmentSubmissionRepository, IQuestionResultRepository questionResultRepository)
        {
            _openAiConfig = openAiConfig.Value;
            _storageConfig = storageConfig.Value;
            _assessmentRepository = assessmentRepository;
            _unitOfWork = unitOfWork;
            _documentRepository = documentRepository;
            _optionRepository = optionRepository;
            _questionRepository = questionRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _mapper = mapper;
            _assessmentSubmissionRepository = assessmentSubmissionRepository;
            _questionResultRepository = questionResultRepository;
        }

        public async Task<BaseResponse<AssessmentResponse>> TakeAssessment(int documentId, AssessmentRequest request)
        {
            var document = await _documentRepository.GetAsync(documentId);
            if (document == null)
            {
                return new BaseResponse<AssessmentResponse>
                {
                    Message = "Document does not exists",
                    Status = false
                };
            }

            var loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var user = await _userRepository.GetAsync(int.Parse(loginUserId ?? "0")) ?? throw new UnAuthenticatedUserException();

            var openApi = new OpenAIAPI(_openAiConfig.ApiKey);
            var documentContent = File.ReadAllLines($"{_storageConfig.Path}\\Documents\\{document.DocumentUrl}");
            var prompt = GetPrompt(request.AssessmentType, request.QuestionCount, documentContent, request.DifficultyLevel, request.Prefences, request.AdvancedPrefences);
            var completionRequest = new CompletionRequest
            {
                Prompt = prompt,
                MaxTokens = 1000
            };

            var result = await openApi.Completions.CreateCompletionAsync(completionRequest);
            var assessment = new Assessment
            {
                AssessmentType = request.AssessmentType,
                CreatedBy = loginUserId,
                DateCreated = DateTime.UtcNow,
                DocumentId = documentId,
                UserId = user.Id,
            };

            var questionsAndOptions = GetQuestionsAndOptions(request.AssessmentType, result, assessment, loginUserId);

            await _assessmentRepository.AddAsync(assessment);
            await _questionRepository.AddRangeAsync(questionsAndOptions.Item1);
            if (questionsAndOptions.Item2.Count > 0) await _optionRepository.AddRangeAsync(questionsAndOptions.Item2);
            await _unitOfWork.SaveAsync();

            assessment = await _assessmentRepository.GetAsync(assessment.Id);
            var assessmentResponse = _mapper.Map<AssessmentResponse>(assessment);

            return new BaseResponse<AssessmentResponse>
            {
                Message = "Assessment created succesfully",
                Status = true,
                Value = assessmentResponse
            };
        }

        public async Task<BaseResponse> SubmitAssessment(AssessmentSubmissionRequest request)
        {
            var assessment = await _assessmentRepository.GetAsync(request.AssessmentId);
            if (assessment == null)
            {
                return new BaseResponse
                {
                    Message = "Assessment not found",
                    Status = false
                };
            }

            var loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var user = await _userRepository.GetAsync(int.Parse(loginUserId));
            if (user == null)
            {
                return new BaseResponse
                {
                    Message = "User is not authenticated",
                    Status = false
                };
            }

            var submission = new AssesmentSubmission
            {
                AssessmentId = assessment.Id,
                CreatedBy = loginUserId,
                DateCreated = DateTime.UtcNow,
                DocumentId = assessment.DocumentId,
                UserId = user.Id
            };
            await _assessmentSubmissionRepository.AddAsync(submission);

            foreach (var item in request.QuestionAnswers)
            {
                var question = await _questionRepository.GetAsync(item.Key);
                if (question == null)
                {
                    return new BaseResponse
                    {
                        Message = "Question not found",
                        Status = false
                    };
                }

                var userAnswer = item.Value;
                if (assessment.AssessmentType == AssessmentType.MultipleChoice)
                {
                    var option = await _optionRepository.GetAsync(int.Parse(item.Value));
                    if (option == null)
                    {
                        return new BaseResponse
                        {
                            Message = "Option not found",
                            Status = false
                        };
                    }
                    userAnswer = option.OptionText;
                }


                await _questionResultRepository.AddAsync(new QuestionResult
                {
                    CreatedBy = loginUserId,
                    AssessmentSubmissionId = submission.Id,
                    AssesmentSubmission = submission,
                    DateCreated = DateTime.UtcNow,
                    QuestionId = question.Id,
                    UserAnswer = userAnswer
                });
            }
            await _unitOfWork.SaveAsync();
            return new BaseResponse
            {
                Message = "Assessment submission successfull",
                Status = true
            };
        }

        public async Task<BaseResponse<AssessmentResponse>> GetAssessment(int id)
        {
            var assessment = await _assessmentRepository.GetAsync(id);
            if (assessment == null)
            {
                return new BaseResponse<AssessmentResponse>
                {
                    Message = "Assessment not found",
                    Status = false
                };
            }

            var response = _mapper.Map<AssessmentResponse>(assessment);
            return new BaseResponse<AssessmentResponse>
            {
                Message = "Assessment successfully found",
                Status = true,
                Value = response
            };
        }

        public async Task<BaseResponse<ICollection<AssessmentsResponse>>> GetAssessmentsByDocument(int documentId)
        {
            var assessments = await _assessmentRepository.GetAllAsync(x => x.DocumentId == documentId);
            var response = _mapper.Map<List<AssessmentsResponse>>(assessments);
            await Task.WhenAll(response.Select(async x =>
            {
                var grade = await GetRecentGrade(x.Id);
                x.RecentGrade = grade == null ? string.Empty : grade.ToString();
            }));

            return new BaseResponse<ICollection<AssessmentsResponse>>
            {
                Message = "List of assessments",
                Status = true,
                Value = response
            };
        }

        public async Task<BaseResponse<ICollection<AssessmentsResponse>>> GetAssessmentsByUser(int userId)
        {
            var assessments = await _assessmentRepository.GetAllAsync(x => x.UserId == userId);
            var response = _mapper.Map<List<AssessmentsResponse>>(assessments);
            await Task.WhenAll(response.Select(async x =>
            {
                var grade = await GetRecentGrade(x.Id);
                x.RecentGrade = grade == null ? string.Empty : grade.ToString();
            }));

            return new BaseResponse<ICollection<AssessmentsResponse>>
            {
                Message = "List of assessments",
                Status = true,
                Value = response
            };
        }

        public async Task<BaseResponse> DeleteAssessment(int id)
        {
            var assessment = await _assessmentRepository.GetAsync(id);
            if (assessment == null)
            {
                return new BaseResponse
                {
                    Message = "Assessment not found",
                    Status = false
                };
            }

            _assessmentRepository.Remove(assessment);
            await _unitOfWork.SaveAsync();
            return new BaseResponse
            {
                Message = "Assessment removed successfully",
                Status = true
            };
        }

        private async Task<double?> GetRecentGrade(int assessmentId)
        {
            var assesmentSubmissions = await _assessmentSubmissionRepository.GetAllAsync(r => r.AssessmentId == assessmentId);
            var latestSubmission = assesmentSubmissions.Count != 0 ? assesmentSubmissions.OrderByDescending(r => r.DateCreated).First() : null;
            if (latestSubmission == null) 
                return null;
            var questionsPassed = 0;
            latestSubmission.Results.ForEach(x =>
            {
                if (x.Question.Answer == x.UserAnswer)
                    questionsPassed++;
            });
            var grade = questionsPassed / latestSubmission.Results.Count * 100;
            return grade;
        }

        private (List<Question>, List<Domain.Entities.Option>) GetQuestionsAndOptions(AssessmentType type, CompletionResult result, Assessment assessment, string loginUserId)
        {
            var questions = new List<Question>();
            var options = new List<Domain.Entities.Option>();
            if (type == AssessmentType.MultipleChoice)
            {
                var parsedResult = ParsePromptResultToJsonClass<MultipleChoice>(result.Completions[0].Text);
                foreach (var item in parsedResult)
                {
                    var question = new Question
                    {
                        Answer = item.Answer,
                        QuestionText = item.Question,
                        AssessmentId = assessment.Id,
                        Assessment = assessment,
                        CreatedBy = loginUserId,
                        DateCreated = DateTime.UtcNow,
                        Elucidation = item.Explanation
                    };
                    questions.Add(question);

                    var questionOptions = item.Options.Select(x => new Domain.Entities.Option
                    {
                        QuestionId = question.Id,
                        Question = question,
                        CreatedBy = loginUserId,
                        DateCreated = DateTime.UtcNow,
                        OptionText = x
                    });
                    options.AddRange(questionOptions);
                }
            }
            else if (type == AssessmentType.TrueFalse)
            {
                var parsedResult = ParsePromptResultToJsonClass<TrueFalse>(result.Completions[0].Text);
                foreach (var item in parsedResult)
                {
                    var question = new Question
                    {
                        AssessmentId = assessment.Id,
                        Assessment = assessment,
                        DateCreated = DateTime.UtcNow,
                        CreatedBy = loginUserId,
                        Answer = item.Answer,
                        QuestionText = item.Statement,
                        Elucidation = item.Explanation
                    };
                    questions.Add(question);
                }
            }
            else if (type == AssessmentType.FillInTheBlanks)
            {
                var parsedResult = ParsePromptResultToJsonClass<FillInTheBlanks>(result.Completions[0].Text);
                foreach (var item in parsedResult)
                {
                    var question = new Question
                    {
                        Assessment = assessment,
                        AssessmentId = assessment.Id,
                        DateCreated = DateTime.UtcNow,
                        CreatedBy = loginUserId,
                        QuestionText = item.Sentence,
                        Answer = item.Answer
                    };
                    questions.Add(question);
                }
            }
            else
            {
                var parsedResult = ParsePromptResultToJsonClass<Flashcard>(result.Completions[0].Text);
                foreach (var item in parsedResult)
                {
                    var question = new Question
                    {
                        AssessmentId = assessment.Id,
                        Assessment = assessment,
                        CreatedBy = loginUserId,
                        DateCreated = DateTime.UtcNow,
                        QuestionText = item.Question,
                        Answer = item.Answer,
                        Elucidation = item.Explanation
                    };
                    questions.Add(question);
                }
            }
            return (questions, options);
        }

        private List<T> ParsePromptResultToJsonClass<T>(string result)
        {
            var promptResult = JsonSerializer.Deserialize<List<T>>(result);
            return promptResult;
        }

        private string GetPrompt(AssessmentType type, int count, string[] documentContent, DifficultyLevel difficultyLevel, List<string> prefences, bool hasPrefence)
        {
            string prompt;
            var secondPart = $"Ensure the questions focus on the following areas of concentration:" +
                    $"\n\n{string.Join(", ", prefences ?? [])}. ";
            if (type == AssessmentType.MultipleChoice)
            {
                var firstPart = $"Generate {count} multiple-choice questions in JSON format based on the following document:" +
                    $"\n\n{documentContent}" +
                    $"\n\n Each question should have a question text, four options, the correct answer, and an elucidation. ";
                var lastPart = $"\n\n The difficulty level should be {difficultyLevel}" +
                    $"The JSON structure should be as follows:" +
                    @"{
                            'question': 'What is the capital of Japan?',
                            'options': ['A. Seoul', 'B. Beijing', 'C. Tokyo', 'D. Bangkok'],
                            'answer': 'C. Tokyo',
                            'explanation': 'Tokyo is the capital city of Japan.'
                        }";
                prompt = hasPrefence ? firstPart + secondPart + lastPart : firstPart + lastPart;
            }
            else if (type == AssessmentType.TrueFalse)
            {
                var firstPart = $"Generate {count} true or false questions in JSON format based on the following document:" +
                    $"\n\n{documentContent} " +
                    $"Each question should have a statement, a boolean value indicating the correct answer, and an explanation. ";
                var lastPart = $"\n\n The difficulty level should be {difficultyLevel}" +
                    $"The JSON structure should be as follows:" +
                    @"{
                           'statement': 'Tokyo is the capital of Japan.',
                            'answer': true,
                            'explanation': 'Tokyo is indeed the capital city of Japan.'
                       }";
                prompt = hasPrefence ? firstPart + secondPart + lastPart : firstPart + lastPart;
            }
            else if (type == AssessmentType.FillInTheBlanks)
            {
                var firstPart = $"Generate {count} fill-in-the-gaps questions in JSON format based on the following document:" +
                    $"\n\n{documentContent}" +
                    $"\n\n Each question should have a sentence with a blank and the correct answer. ";
                var lastPart = $"\n\n The difficulty level should be {difficultyLevel}" +
                    $"The JSON structure should be as follows:" +
                    @"{
                            'sentence': 'Tokyo is the capital of ____.',
                            'answer': 'Japan'
                        }";
                prompt = hasPrefence ? firstPart + secondPart + lastPart : firstPart + lastPart;
            }
            else
            {
                var firstPart = $"Generate {count} flashcards in JSON format based on the following document:" +
                    $"\n\n{documentContent}" +
                    $"\n\n Each flashcard should have a question, an answer, and an elucidation. ";
                var lastPart = $"\n\n The difficulty level should be {difficultyLevel}" +
                    $"The JSON structure should be as follows:" +
                    @"{
                            'question': 'What is the capital of Japan?',
                            'answer': 'Tokyo',
                            'explanation': 'Tokyo is the capital city of Japan.'
                        }";
                prompt = hasPrefence ? firstPart + secondPart + lastPart : firstPart + lastPart;
            }
            return prompt;
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
