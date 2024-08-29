namespace QuestionGenerator.Core.Application.Exceptions
{
    public class AdvancedPreferencesNotAllowedException : Exception
    {
        private static readonly string DefaultMessage = "Advanced preferences are not allowed for this user role. Please upgrade your account.";

        public AdvancedPreferencesNotAllowedException() : base(DefaultMessage)
        {
        }

        public AdvancedPreferencesNotAllowedException(string message) : base(message)
        {
        }
    }

}
