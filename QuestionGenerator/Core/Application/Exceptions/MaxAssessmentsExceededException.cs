using QuestionGenerator.Core.Domain.Enums;

namespace QuestionGenerator.Core.Application.Exceptions
{
    public class MaxAssessmentsExceededException : Exception
    {
        private static readonly string DefaultMessageBasicUser = "Basic users can only take up to 5 assessments per month.";
        private static readonly string DefaultMessageStandardUser = "Standard users can only take up to 50 assessments per month.";

        public MaxAssessmentsExceededException(string message) : base(message)
        {
        }

        public MaxAssessmentsExceededException(UserType userType) : base(GetDefaultMessage(userType))
        {
        }

        private static string GetDefaultMessage(UserType userType)
        {
            return userType switch
            {
                UserType.Basic => DefaultMessageBasicUser,
                UserType.Standard => DefaultMessageStandardUser,
                _ => "Assessment count exceeds the allowed limit for this month."
            };
        }
    }

}
