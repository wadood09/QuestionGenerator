namespace QuestionGenerator.Core.Application.Exceptions
{
    public class UnAuthenticatedUserException : Exception
    {
        private static readonly string DefaultMessage = "User is not authenticated. Please log in to continue.";

        public UnAuthenticatedUserException(string message) : base(message)
        {
        }

        public UnAuthenticatedUserException() : base(DefaultMessage)
        {
        }
    }
}
