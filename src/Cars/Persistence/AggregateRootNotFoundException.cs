namespace Cars.Persistence
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class AggregateRootNotFoundException : PersistenceException
    {
        public AggregateRootNotFoundException()
            : this((Exception)null)
        {
        }

        public AggregateRootNotFoundException(string message)
            : this(message, null)
        {
        }

        public AggregateRootNotFoundException(Exception inner)
            : this("Cannot find the aggregate root.", inner)
        {
        }

        public AggregateRootNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected AggregateRootNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
