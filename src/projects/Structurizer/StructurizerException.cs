using System;
using System.Collections.Generic;

namespace Structurizer
{
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
    }
}