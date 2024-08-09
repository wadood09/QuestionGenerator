namespace QuestionGenerator.Models.AssessmentSubmissionModel
{
    public class AssessmentSubmissionRequest
    {
        public int AssessmentId { get; set; }
        public Dictionary<int, string> QuestionAnswers { get; set; } = new Dictionary<int, string>();
    }
}
