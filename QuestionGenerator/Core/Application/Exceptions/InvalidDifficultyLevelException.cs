using QuestionGenerator.Core.Domain.Enums;

namespace QuestionGenerator.Core.Application.Exceptions
{
    public class InvalidDifficultyLevelException : Exception
    {
        private static readonly string DefaultMessageBasicUser = "Basic users can only take assessments with Easy difficulty.";
        private static readonly string DefaultMessageStandardUser = "Standard users can only take assessments with Easy or Medium difficulty.";

        public InvalidDifficultyLevelException(string message) : base(message)
        {
        }

        public InvalidDifficultyLevelException(UserType userType) : base(GetDefaultMessage(userType))
        {
        }

        private static string GetDefaultMessage(UserType userType)
        {
            return userType switch
            {
                UserType.Basic => DefaultMessageBasicUser,
                UserType.Standard => DefaultMessageStandardUser,
                _ => "Difficulty level is not supported for this user role."
            };
        }
    }
}
