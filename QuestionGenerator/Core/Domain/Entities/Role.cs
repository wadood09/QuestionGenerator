namespace QuestionGenerator.Core.Domain.Entities
{
    public class Role : Auditables
    {
        public string Name { get; set; } = default!;

        #region Relationships
        public List<User> Users { get; set; } = [];

        #endregion
    }
}
