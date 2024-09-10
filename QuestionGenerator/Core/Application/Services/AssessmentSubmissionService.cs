using AutoMapper;
using QuestionGenerator.Core.Application.Exceptions;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Core.Domain.Enums;
using QuestionGenerator.Models;
using QuestionGenerator.Models.AssessmentModel;
using QuestionGenerator.Models.AssessmentSubmissionModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace QuestionGenerator.Core.Application.Services
{
    public class AssessmentSubmissionService : IAssessmentSubmissionService
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAssessmentSubmissionRepository _assessmentSubmissionRepository;
        private readonly IQuestionResultRepository _questionResultRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IOptionRepository _optionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AssessmentSubmissionService(IAssessmentRepository assessmentRepository, IQuestionRepository questionRepository, IOptionRepository optionRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IAssessmentSubmissionRepository assessmentSubmissionRepository, IQuestionResultRepository questionResultRepository, IMapper mapper)
        {
            _assessmentRepository = assessmentRepository;
            _questionRepository = questionRepository;
            _optionRepository = optionRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _assessmentSubmissionRepository = assessmentSubmissionRepository;
            _questionResultRepository = questionResultRepository;
            _mapper = mapper;
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
            var user = await _userRepository.GetAsync(int.Parse(loginUserId ?? "0")) ?? throw new UnAuthenticatedUserException();

            var submission = new AssessmentSubmission
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
                Message = $"{submission.Id}",
                Status = true
            };
        }

        public async Task<BaseResponse<ICollection<AssessmentAttemptsResponse>>> GetAssessmentAttempts(int assessmentId)
        {
            var loggedInUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue("Role");
            int count = loggedInUserRole switch
            {
                "Basic User" => 5,
                "Standard User" => 15,
                "Premium User" => int.MaxValue,
                _ => throw new InvalidUserRoleException()
            };

            var attempts = await _assessmentSubmissionRepository.GetAllAsync(x => x.AssessmentId == assessmentId, count);
            var response = _mapper.Map<List<AssessmentAttemptsResponse>>(attempts);

            return new BaseResponse<ICollection<AssessmentAttemptsResponse>>
            {
                Message = "List of attempts",
                Status = true,
                Value = response
            };
        }

        public async Task<BaseResponse<AssessmentAttemptResponse>> GetAssessmentAttempt(int id)
        {
            var attempt = await _assessmentSubmissionRepository.GetAsync(id);
            if (attempt == null)
            {
                return new BaseResponse<AssessmentAttemptResponse>
                {
                    Message = "Attempt not found",
                    Status = false
                };
            }

            var response = _mapper.Map<AssessmentAttemptResponse>(attempt);
            return new BaseResponse<AssessmentAttemptResponse>
            {
                Message = "Attempt successfully found",
                Status = true,
                Value = response
            };
        }

        public async Task<BaseResponse> RemoveAssessmentAttempt(int id)
        {
            var attempt = await _assessmentSubmissionRepository.GetAsync(id);
            if (attempt == null)
            {
                return new BaseResponse
                {
                    Message = "Attempt not found",
                    Status = false
                };
            }

            _assessmentSubmissionRepository.Remove(attempt);
            await _unitOfWork.SaveAsync();
            return new BaseResponse
            {
                Message = "Attempt successfully removed",
                Status = true
            };
        }
    }
}
