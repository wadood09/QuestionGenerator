namespace QuestionGenerator.Models.DocumentModel
{
    public class DocumentRequest
    {
        public string Title { get; set; } = default!;
        public IFormFile Document { get; set; }
    }
}
