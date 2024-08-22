namespace QuestionGenerator.Core.Domain.Entities
{
    public class QuestionResult : Auditables
    {
        public int QuestionId { get; set; }
        public int AssessmentSubmissionId { get; set; }
        public string UserAnswer { get; set; } = default!;

        #region Relationships
        public Question Question { get; set; }
        public AssessmentSubmission AssesmentSubmission { get; set; }

        #endregion
    }
}
