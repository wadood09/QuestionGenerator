using AutoMapper;
using QuestionGenerator.Core.Application.AutoMapper.Converter;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Models.AssessmentModel;
using QuestionGenerator.Models.DocumentModel;
using QuestionGenerator.Models.OptionModel;
using QuestionGenerator.Models.TokenModel;
using QuestionGenerator.Models.QuestionModel;
using QuestionGenerator.Models.UserModel;
using QuestionGenerator.Core.Application.AutoMapper.Resolver;

namespace QuestionGenerator.Core.Application.AutoMapper.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Option, OptionResponse>();

            CreateMap<Question, QuestionResponse>();

            CreateMap<QuestionResult, AttemptQuestionResponse>()
                .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.Question.QuestionText))
                .ForMember(dest => dest.QuestionAnswer, opt => opt.MapFrom(src => src.Question.Answer))
                .ForMember(dest => dest.Elucidation, opt => opt.MapFrom(src => src.Question.Elucidation))
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Question.Options));

            CreateMap<Assessment, AssessmentResponse>()
                .ForMember(dest => dest.DocumentTitle, opt => opt.MapFrom(src => src.Document.Title))
                .ForMember(dest => dest.AssessmentType, opt => opt.ConvertUsing(new AssessmentTypeConverter(), src => src.AssessmentType));

            CreateMap<Assessment, AssessmentsResponse>()
                .ForMember(dest => dest.AssessmentType, opt => opt.ConvertUsing(new AssessmentTypeConverter(), src => src.AssessmentType))
                .ForMember(dest => dest.RecentGrade, opt => opt.MapFrom<AssessmentGradeResolver<Assessment, AssessmentsResponse>>());

            CreateMap<AssessmentSubmission, AssessmentAttemptsResponse>()
                .ForMember(dest => dest.TimeSubmitted, opt => opt.MapFrom(src => src.DateCreated))
                .ForMember(dest => dest.Grade, opt => opt.MapFrom<AssessmentGradeResolver<AssessmentSubmission, AssessmentAttemptsResponse>>());

            CreateMap<AssessmentSubmission, AssessmentAttemptResponse>()
                .ForMember(dest => dest.DocumentTitle, opt => opt.MapFrom(src => src.Document.Title))
                .ForMember(dest => dest.AssessmentType, opt => opt.ConvertUsing(new AssessmentTypeConverter(), src => src.Assessment.AssessmentType))
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Results))
                .ForMember(dest => dest.Grade, opt => opt.MapFrom<AssessmentGradeResolver<AssessmentSubmission, AssessmentAttemptResponse>>());

            CreateMap<Document, DocumentResponse>();

            CreateMap<Document, DocumentsResponse>();

            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));
        }
    }
}
