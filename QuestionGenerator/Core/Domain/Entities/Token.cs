using QuestionGenerator.Core.Domain.Enums;

namespace QuestionGenerator.Core.Domain.Entities
{
    public class Token : Auditables
    {
        public string TokenValue { get; set; }
        public int UserId { get; set; }
        public DateTime Expires { get; set; }
        public TokenType TokenType { get; set; }

        #region Relationships
        public User User { get; set; }

        #endregion
    }
}
