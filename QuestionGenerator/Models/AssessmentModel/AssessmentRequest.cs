using QuestionGenerator.Core.Domain.Enums;

namespace QuestionGenerator.Models.AssessmentModel
{
    public class AssessmentRequest
    {
        public int QuestionCount { get; set; }
        public int AssessmentType { get; set; }
        public bool AdvancedPrefences { get; set; }
        public List<string> Prefences { get; set; } = [];
        public int DifficultyLevel { get; set; }
    }
}
