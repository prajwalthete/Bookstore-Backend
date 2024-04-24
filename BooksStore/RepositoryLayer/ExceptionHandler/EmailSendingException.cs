using System.Runtime.Serialization;

namespace RepositoryLayer.ExceptionHandler
{
    [Serializable]
    public class EmailSendingException : Exception
    {
        public EmailSendingException()
        {
        }

        public EmailSendingException(string? message) : base(message)
        {
        }

        public EmailSendingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
        protected EmailSendingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}