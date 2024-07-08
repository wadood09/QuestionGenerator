namespace QuestionGenerator.Core.Domain.Entities
{
    public class RevisitedAssesment : Auditables
    {
        public int AssessmentId { get; set; }
        public double AssessmentScore { get; set; }
        public int DocumentId { get; set; }
        public int UserId { get; set; }

        #region Relationships
        public Assessment Assessment { get; set; }
        public Document Document { get; set; }
        public User User { get; set; }

        #endregion
    }
}
