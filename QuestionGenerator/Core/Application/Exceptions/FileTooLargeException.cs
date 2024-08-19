namespace QuestionGenerator.Core.Application.Exceptions
{
    public class FileTooLargeException : Exception
    {
        private static readonly string DefaultMessageBasicUser = "File size exceeds the 10MB limit. Please upload a smaller file or upgrade your account.";
        private static readonly string DefaultMessagePremiumUser = "File size exceeds the 30MB limit. Please upload a smaller file.";

        public FileTooLargeException(string message) : base(message)
        {
        }

        public FileTooLargeException(bool isBasicUser) : base(isBasicUser ? DefaultMessageBasicUser : DefaultMessagePremiumUser)
        {
        }
    }
}
