namespace QuestionGenerator.Core.Application.Exceptions
{
    public class FileTypeRestrictedException : Exception
    {
        private static readonly string DefaultMessage = "The uploaded file type is restricted for basic users. Please upgrade your account to upload this type of document.";

        public FileTypeRestrictedException(string message) : base(message)
        {
        }

        public FileTypeRestrictedException() : base(DefaultMessage)
        {
        }
    }
}
