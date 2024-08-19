namespace QuestionGenerator.Core.Application.Exceptions
{
    public class InvalidDocumentException : Exception
    {
        private static readonly string DefaultMessage = "The uploaded file is invalid. Please upload a valid document.";

        public InvalidDocumentException(string message) : base(message)
        {
        }

        public InvalidDocumentException() : base(DefaultMessage)
        {
        }
    }
}
