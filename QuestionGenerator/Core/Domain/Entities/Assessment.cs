using QuestionGenerator.Core.Domain.Enums;

namespace QuestionGenerator.Core.Domain.Entities
{
    public class Assessment : Auditables
    {
        public List<Question> Questions { get; set; } = [];
        public AssessmentType AssessmentType { get; set; }
        public int DocumentId { get; set; }

        #region Relationships
        public Document Document { get; set; }
        public List<RevisitedAssesment> RevistedAssesments { get; set; } = [];

        #endregion
    }
}
