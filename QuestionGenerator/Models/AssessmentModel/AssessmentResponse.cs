using QuestionGenerator.Core.Domain.Enums;
using QuestionGenerator.Models.QuestionModel;

namespace QuestionGenerator.Models.AssessmentModel
{
    public class AssessmentResponse
    {
        public int Id { get; set; }
        public ICollection<QuestionResponse> Questions { get; set; } = [];
        public AssessmentType AssessmentType { get; set; }
        public string DocumentTitle { get; set; } = default!;
    }
}
