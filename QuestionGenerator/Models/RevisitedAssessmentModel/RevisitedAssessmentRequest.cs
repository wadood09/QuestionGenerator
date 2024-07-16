namespace QuestionGenerator.Models.RevisitedAssessmentModel
{
    public class RevisitedAssessmentRequest
    {
        public int AssessmentId { get; set; }
        public Dictionary<int, int> QuestionAnswers { get; set; } = new Dictionary<int, int>();
    }
}
