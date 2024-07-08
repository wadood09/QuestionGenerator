using QuestionGenerator.Models.OptionModel;

namespace QuestionGenerator.Models.QuestionModel
{
    public class QuestionResponse
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = default!;
        public string Answer { get; set; } = default!;
        public string? Elucidation { get; set; }
        public List<OptionResponse> Options { get; set; } = [];

    }
}
