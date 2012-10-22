using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Rendering.Resourcing
{
    public class InvalidDescriptionException : Exception
    {
        public InvalidDescriptionException(Type type, string message)
            : base("Bad description of type " + type + "\n" + message)
        {
        }
    }
}
