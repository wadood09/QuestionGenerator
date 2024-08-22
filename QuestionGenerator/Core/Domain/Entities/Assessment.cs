using QuestionGenerator.Core.Domain.Enums;

namespace QuestionGenerator.Core.Domain.Entities
{
    public class Assessment : Auditables
    {
        public List<Question> Questions { get; set; } = [];
        public AssessmentType AssessmentType { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
        public int DocumentId { get; set; }
        public int UserId { get; set; }

        #region Relationships
        public Document Document { get; set; }
        public User User { get; set; }
        public List<AssessmentSubmission> AssessmentSubmissions { get; set; } = [];

        #endregion
    }
}
