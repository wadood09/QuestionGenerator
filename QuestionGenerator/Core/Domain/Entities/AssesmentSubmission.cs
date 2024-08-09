namespace QuestionGenerator.Core.Domain.Entities
{
    public class AssesmentSubmission : Auditables
    {
        public int AssessmentId { get; set; }
        public List<QuestionResult> Results { get; set; } = [];
        public int DocumentId { get; set; }
        public int UserId { get; set; }

        #region Relationships
        public Assessment Assessment { get; set; }
        public Document Document { get; set; }
        public User User { get; set; }

        #endregion
    }
}
