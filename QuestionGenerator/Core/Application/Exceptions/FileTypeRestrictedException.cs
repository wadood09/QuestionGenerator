using QuestionGenerator.Core.Domain.Enums;

namespace QuestionGenerator.Core.Application.Exceptions
{
    public class FileTypeRestrictedException : Exception
    {
        private static readonly string DefaultMessageBasicUser = "The uploaded file type is restricted for Basic Users. Only .txt files are allowed. Please upgrade your account for additional file type support.";
        private static readonly string DefaultMessageStandardUser = "The uploaded file type is restricted for Standard Users. Please upgrade your account for additional file type support.";
        private static readonly string DefaultMessagePremiumUser = "The uploaded file type is restricted. Please contact support for assistance.";

        public FileTypeRestrictedException(string message) : base(message)
        {
        }

        public FileTypeRestrictedException(UserType userType) : base(GetDefaultMessage(userType))
        {
        }

        // Method to get the default message based on the user type
        private static string GetDefaultMessage(UserType userType)
        {
            return userType switch
            {
                UserType.Basic => DefaultMessageBasicUser,
                UserType.Standard => DefaultMessageStandardUser,
                UserType.Premium => DefaultMessagePremiumUser,
                _ => "The uploaded file type is restricted."
            };
        }
    }
}
