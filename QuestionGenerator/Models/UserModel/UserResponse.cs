namespace QuestionGenerator.Models.UserModel
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? ProfilePictureUrl { get; set; }
        public string RoleName { get; set; } = default!;
    }
}
