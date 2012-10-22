using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Rendering.Effects.Shaders
{
    public class InvalidCodeException : Exception
    {
        public InvalidCodeException(string message) : base(message) { }

        public InvalidCodeException() : base() { }
    }
}
