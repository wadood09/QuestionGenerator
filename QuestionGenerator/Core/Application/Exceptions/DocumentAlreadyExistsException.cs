namespace QuestionGenerator.Core.Application.Exceptions
{
    public class DocumentAlreadyExistsException : Exception
    {
        private static readonly string DefaultMessage = "A document with this title already exists. Please choose a different title.";

        public DocumentAlreadyExistsException(string message) : base(message)
        {
        }

        public DocumentAlreadyExistsException() : base(DefaultMessage)
        {
        }
    }
}
