﻿using System.Runtime.Serialization;

namespace RepositoryLayer.ExceptionHandler
{
    [Serializable]
    public class InvalidLoginException : Exception
    {
        public InvalidLoginException()
        {
        }

        public InvalidLoginException(string? message) : base(message)
        {
        }

        public InvalidLoginException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidLoginException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}