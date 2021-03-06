﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.Utils
{
  [global::System.Serializable]
  public class CompilingErrorException : Exception
  {
    //
    // For guidelines regarding the creation of new exception types, see
    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
    // and
    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
    //

    public CompilingErrorException() { }
    public CompilingErrorException(string message) : base(message) { }
    public CompilingErrorException(string message, Exception inner) : base(message, inner) { }
    protected CompilingErrorException(
    System.Runtime.Serialization.SerializationInfo info,
    System.Runtime.Serialization.StreamingContext context)
      : base(info, context) { }
  }
}
