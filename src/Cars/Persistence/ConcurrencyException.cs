namespace Cars.Persistence
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ConcurrencyException : PersistenceException
    {
        public ConcurrencyException()
            : this((Exception)null)
        {
        }

        public ConcurrencyException(string message)
            : this(message, null)
        {
        }

        public ConcurrencyException(Exception inner)
            : this("A concurrency exception has occurred.", inner)
        {
        }

        public ConcurrencyException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ConcurrencyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
