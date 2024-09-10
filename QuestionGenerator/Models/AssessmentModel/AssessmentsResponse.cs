namespace QuestionGenerator.Models.AssessmentModel
{
    public class AssessmentsResponse
    {
        public int Id { get; set; }
        public string AssessmentType { get; set; }
        public string DateCreated { get; set; }
        public int QuestionsCount { get; set; }
        public double? RecentGrade { get; set; }
    }
}
