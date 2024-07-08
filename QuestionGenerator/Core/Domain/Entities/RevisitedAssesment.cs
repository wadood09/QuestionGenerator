namespace QuestionGenerator.Core.Domain.Entities
{
    public class RevisitedAssesment : Auditables
    {
        public int AssessmentId { get; set; }
        public double AssessmentScore { get; set; }
        public int DocumentId { get; set; }

        #region Relationships
        public Assessment Assessment { get; set; }
        public Document Document { get; set; }

        #endregion
    }
}
