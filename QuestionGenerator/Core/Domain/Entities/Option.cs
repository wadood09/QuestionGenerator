namespace QuestionGenerator.Core.Domain.Entities
{
    public class Option : Auditables
    {
        public string OptionText { get; set; } = default!;
        public int QuestionId { get; set; }

        #region Relationships
        public Question Question { get; set; }

        #endregion
    }
}
