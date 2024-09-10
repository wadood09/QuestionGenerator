using QuestionGenerator.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace QuestionGenerator.Core.Domain.Entities
{
    public class Document : Auditables
    {
        public string Title { get; set; } = default!;
        public string DocumentUrl { get; set; } = default!;
        public int UserId { get; set; }
        public string TableOfContentsJson { get; set; } = default!;

        [NotMapped]
        public List<string> TableOfContents
        {
            get => string.IsNullOrEmpty(TableOfContentsJson) ? new List<string>() : TableOfContentsJson.ExtractJson<List<string>>();
            set => TableOfContentsJson = JsonSerializer.Serialize(value);
        }

        #region Relationships
        public User User { get; set; }
        public List<Assessment> Assessments { get; set; } = [];
        public List<AssessmentSubmission> AssessmentSubmissions { get; set; } = [];

        #endregion
    }
}
