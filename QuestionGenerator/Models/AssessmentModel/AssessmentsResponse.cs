namespace QuestionGenerator.Models.AssessmentModel
{
    public class AssessmentsResponse
    {
        public int Id { get; set; }
        public string AssessmentType { get; set; }
        public DateTime DateCreated { get; set; }
        public int QuestionsCount { get; set; }
        public double? RecentGrade { get; set; }
    }
}
