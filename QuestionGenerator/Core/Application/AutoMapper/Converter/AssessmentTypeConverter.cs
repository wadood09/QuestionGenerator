using AutoMapper;
using QuestionGenerator.Core.Domain.Enums;

namespace QuestionGenerator.Core.Application.AutoMapper.Converter
{
    public class AssessmentTypeConverter : IValueConverter<AssessmentType, string>
    {
        public string Convert(AssessmentType sourceMember, ResolutionContext context)
        {
            if (sourceMember == AssessmentType.MultipleChoice)
            {
                return "Multiple Choice";
            }
            else if (sourceMember == AssessmentType.TrueFalse)
            {
                return "True or False";
            }
            else if (sourceMember == AssessmentType.FillInTheBlanks)
            {
                return "Fill in the Blanks";
            }
            else
            {
                return "Flash Cards";
            }
        }
    }
}
