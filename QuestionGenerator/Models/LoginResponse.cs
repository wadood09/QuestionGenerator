using QuestionGenerator.Models.UserModel;

namespace QuestionGenerator.Models
{
    public class LoginResponse
    {
        public bool Status { get; set; }
        public bool HasSignedInWithGoogle { get; set; }
        public string Message { get; set; }
        public UserResponse User { get; set; }
    }
}
