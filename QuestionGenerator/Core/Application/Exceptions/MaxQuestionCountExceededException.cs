using QuestionGenerator.Core.Domain.Enums;

namespace QuestionGenerator.Core.Application.Exceptions
{
    public class MaxQuestionCountExceededException : Exception
    {
        private static readonly string DefaultMessageBasicUser = "Basic users can only have a maximum of 10 questions per assessment.";
        private static readonly string DefaultMessageStandardUser = "Standard users can only have a maximum of 50 questions per assessment.";
        private static readonly string DefaultMessagePremiumUser = "Premium users can only have a maximum of 75 questions per assessment.";

        public MaxQuestionCountExceededException(string message) : base(message)
        {
        }

        public MaxQuestionCountExceededException(UserType userType) : base(GetDefaultMessage(userType))
        {
        }

        private static string GetDefaultMessage(UserType userType)
        {
            return userType switch
            {
                UserType.Basic => DefaultMessageBasicUser,
                UserType.Standard => DefaultMessageStandardUser,
                UserType.Premium => DefaultMessagePremiumUser,
                _ => "Question count exceeds the allowed limit."
            };
        }
    }
}
