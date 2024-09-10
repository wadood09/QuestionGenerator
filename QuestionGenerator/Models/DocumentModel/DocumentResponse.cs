using QuestionGenerator.Models.AssessmentModel;

namespace QuestionGenerator.Models.DocumentModel
{
    public class DocumentResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DocumentUrl { get; set; }
        public List<AssessmentsResponse> Assessments { get; set; } = new List<AssessmentsResponse>();
    }
}
