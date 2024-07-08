using QuestionGenerator.Core.Domain.Enums;

namespace QuestionGenerator.Models.AssessmentModel
{
    public class AssessmentRequest
    {
        public int QuestionCount { get; set; }
        public AssessmentType AssessmentType { get; set; }
    }
}
