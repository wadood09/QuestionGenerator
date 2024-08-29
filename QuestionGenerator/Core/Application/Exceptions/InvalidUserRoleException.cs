namespace QuestionGenerator.Core.Application.Exceptions
{
    public class InvalidUserRoleException : Exception
    {
        private static readonly string DefaultMessage = "The user role is invalid or not recognized. Please contact support for assistance.";

        public InvalidUserRoleException() : base(DefaultMessage)
        {
        }

        public InvalidUserRoleException(string message) : base(message)
        {
        }
    }
}
