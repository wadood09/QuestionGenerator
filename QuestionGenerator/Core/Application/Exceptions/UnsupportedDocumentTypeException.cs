namespace QuestionGenerator.Core.Application.Exceptions
{
    public class UnsupportedDocumentTypeException : Exception
    {
        private static readonly string DefaultMessage = "The file type is not supported. Please upload a document with a valid extension.";

        public UnsupportedDocumentTypeException(string message) : base(message)
        {
        }

        public UnsupportedDocumentTypeException() : base(DefaultMessage)
        {
        }
    }
}
