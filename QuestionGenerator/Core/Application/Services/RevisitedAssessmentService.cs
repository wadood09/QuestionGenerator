using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Core.Domain.Enums;
using QuestionGenerator.Models;
using QuestionGenerator.Models.AssessmentSubmissionModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace QuestionGenerator.Core.Application.Services
{
    public class RevisitedAssessmentService : IAssessmentSubmissionService
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAssessmentSubmissionRepository _assessmentSubmissionRepository;
        private readonly IQuestionResultRepository _questionResultRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IOptionRepository _optionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public RevisitedAssessmentService(IAssessmentRepository assessmentRepository, IQuestionRepository questionRepository, IOptionRepository optionRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IAssessmentSubmissionRepository assessmentSubmissionRepository, IQuestionResultRepository questionResultRepository)
        {
            _assessmentRepository = assessmentRepository;
            _questionRepository = questionRepository;
            _optionRepository = optionRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _assessmentSubmissionRepository = assessmentSubmissionRepository;
            _questionResultRepository = questionResultRepository;
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
                DateCreated = DateTime.Now,
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
                    DateCreated = DateTime.Now,
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
    }
}
