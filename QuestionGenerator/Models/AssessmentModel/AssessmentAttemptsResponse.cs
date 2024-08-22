namespace QuestionGenerator.Models.AssessmentModel
{
    public class AssessmentAttemptsResponse
    {
        public int Id { get; set; }
        public DateTime TimeSubmitted { get; set; }
        public double? Grade { get; set; }
    }
}
