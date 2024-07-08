namespace QuestionGenerator.Models
{
    public class BaseResponse<T>
    {
        public string? Message { get; set; }
        public bool Status { get; set; }
        public T? Value { get; set; }
    }

    public class BaseResponse
    {
        public string? Message { get; set; }
        public bool Status { get; set; }
    }
}
