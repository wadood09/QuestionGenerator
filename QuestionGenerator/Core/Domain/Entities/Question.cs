namespace QuestionGenerator.Core.Domain.Entities
{
    public class Question : Auditables
    {
        public string QuestionText { get; set; } = default!;
        public string Answer { get; set; } = default!;
        public string? Elucidation { get; set; }
        public int AssessmentId { get; set; }

        #region Relationships
        public List<Option> Options { get; set; } = [];
        public Assessment Assessment { get; set; }

        #endregion
    }
}
