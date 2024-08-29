using QuestionGenerator.Core.Domain.Enums;

namespace QuestionGenerator.Core.Application.Exceptions
{
    public class FileTooLargeException : Exception
    {
        private static readonly string DefaultMessageBasicUser = "File size exceeds the 5MB limit. Please upload a smaller file or upgrade your account.";
        private static readonly string DefaultMessageStandardUser = "File size exceeds the 20MB limit. Please upload a smaller file or upgrade your account.";
        private static readonly string DefaultMessagePremiumUser = "File size exceeds the 50MB limit. Please upload a smaller file.";

        public FileTooLargeException(string message) : base(message)
        {
        }

        public FileTooLargeException(UserType userType) : base(GetDefaultMessage(userType))
        {
        }

        // Private method to select the appropriate default message based on user type
        private static string GetDefaultMessage(UserType userType)
        {
            return userType switch
            {
                UserType.Basic => DefaultMessageBasicUser,
                UserType.Standard => DefaultMessageStandardUser,
                UserType.Premium => DefaultMessagePremiumUser,
                _ => "File size exceeds the allowed limit."
            };
        }
    }
}
