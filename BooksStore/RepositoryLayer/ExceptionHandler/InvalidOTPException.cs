using System.Runtime.Serialization;

namespace RepositoryLayer.ExceptionHandler
{
    [Serializable]
    public class InvalidOTPException : Exception
    {
        public InvalidOTPException()
        {
        }

        public InvalidOTPException(string? message) : base(message)
        {
        }

        public InvalidOTPException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidOTPException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}