using AutoMapper;
using QuestionGenerator.Core.Application.AutoMapper.Converter;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Models.AssessmentModel;
using QuestionGenerator.Models.DocumentModel;
using QuestionGenerator.Models.OptionModel;
using QuestionGenerator.Models.QuestionModel;
using QuestionGenerator.Models.UserModel;

namespace QuestionGenerator.Core.Application.AutoMapper.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Option, OptionResponse>();

            CreateMap<Question, QuestionResponse>();

            CreateMap<Assessment, AssessmentResponse>()
                .ForMember(dest => dest.DocumentTitle, opt => opt.MapFrom(src => src.Document.Title))
                .ForMember(dest => dest.AssessmentType, opt => opt.ConvertUsing(new AssessmentTypeConverter(), src => src.AssessmentType));

            CreateMap<Assessment, AssessmentsResponse>()
                .ForMember(dest => dest.AssessmentType, opt => opt.ConvertUsing(new AssessmentTypeConverter(), src => src.AssessmentType))
                .ForMember(dest => dest.RecentGrade, opt => opt.Ignore());

            CreateMap<Document, DocumentResponse>();

            CreateMap<Document, DocumentsResponse>();

            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));
        }
    }
}
