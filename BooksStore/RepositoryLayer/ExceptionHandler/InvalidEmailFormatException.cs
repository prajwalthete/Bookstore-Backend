using System.Runtime.Serialization;

namespace RepositoryLayer.ExceptionHandler
{
    [Serializable]
    internal class InvalidEmailFormatException : Exception
    {
        public InvalidEmailFormatException()
        {
        }

        public InvalidEmailFormatException(string? message) : base(message)
        {
        }

        public InvalidEmailFormatException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidEmailFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}