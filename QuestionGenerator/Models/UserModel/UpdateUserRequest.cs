namespace QuestionGenerator.Models.UserModel
{
    public class UpdateUserRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public IFormFile? ImageUrl { get; set; }
    }
}
