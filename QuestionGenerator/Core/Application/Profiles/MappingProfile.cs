using AutoMapper;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Models.AssessmentModel;
using QuestionGenerator.Models.OptionModel;
using QuestionGenerator.Models.QuestionModel;
using QuestionGenerator.Models.UserModel;

namespace QuestionGenerator.Core.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Option, OptionResponse>();

            CreateMap<Question, QuestionResponse>();

            CreateMap<Assessment, AssessmentResponse>()
                .ForMember(dest => dest.DocumentTitle, opt => opt.MapFrom(src => src.Document.Title));

            CreateMap<Assessment, AssessmentsResponse>();

            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));
        }
    }
}
