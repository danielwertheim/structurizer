using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Structurizer
{
    [Serializable]
    public class StructurizerException : AggregateException
    {
        public StructurizerException(string message)
            : base(message)
        {
        }

        public StructurizerException(string message, IEnumerable<Exception> innerExceptions)
            : base(message, innerExceptions)
        {
        }

        protected StructurizerException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        { }
    }
}