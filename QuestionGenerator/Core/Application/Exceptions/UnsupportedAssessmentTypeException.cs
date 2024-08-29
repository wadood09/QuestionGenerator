using QuestionGenerator.Core.Domain.Enums;

namespace QuestionGenerator.Core.Application.Exceptions
{
    public class UnsupportedAssessmentTypeException : Exception
    {
        private static readonly string DefaultMessageBasicUser = "Basic users can only take multiple-choice assessments.";
        private static readonly string DefaultMessageStandardUser = "Standard users can only take multiple-choice or true/false assessments.";

        public UnsupportedAssessmentTypeException(string message) : base(message)
        {
        }

        public UnsupportedAssessmentTypeException(UserType userType) : base(GetDefaultMessage(userType))
        {
        }

        private static string GetDefaultMessage(UserType userType)
        {
            return userType switch
            {
                UserType.Basic => DefaultMessageBasicUser,
                UserType.Standard => DefaultMessageStandardUser,
                _ => "Assessment type is not supported for this user role."
            };
        }
    }
}
