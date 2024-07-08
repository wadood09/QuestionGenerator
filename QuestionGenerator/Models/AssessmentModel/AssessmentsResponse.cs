using QuestionGenerator.Core.Domain.Enums;

namespace QuestionGenerator.Models.AssessmentModel
{
    public class AssessmentsResponse
    {
        public int Id { get; set; }
        public AssessmentType AssessmentType { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
