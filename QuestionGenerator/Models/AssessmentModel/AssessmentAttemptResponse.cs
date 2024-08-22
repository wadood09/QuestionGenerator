using QuestionGenerator.Models.QuestionModel;

namespace QuestionGenerator.Models.AssessmentModel
{
    public class AssessmentAttemptResponse
    {
        public int Id { get; set; }
        public string AssessmentType { get; set; }
        public double? Grade { get; set; }
        public ICollection<AttemptQuestionResponse> Questions { get; set; } = [];
        public string DocumentTitle { get; set; } = default!;
    }
}
