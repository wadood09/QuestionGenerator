using QuestionGenerator.Models.OptionModel;

namespace QuestionGenerator.Models.QuestionModel
{
    public class AttemptQuestionResponse
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = default!;
        public string QuestionAnswer { get; set; } = default!;
        public string UserAnswer { get; set; } = default!;
        public string? Elucidation { get; set; }
        public List<OptionResponse> Options { get; set; } = [];
    }
}
