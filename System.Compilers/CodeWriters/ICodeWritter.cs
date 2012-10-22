using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace System.Compilers.CodeWriters
{
    public interface ICodeWriter
    {
        void Indent();
        void UnIndent();
        void Write(object obj);
        void Write(string text, params object[] format);
        void WriteLine();
        void WriteLine(object obj);
        void WriteLine(string text, params object[] format);

        void WriteUnindented(object obj);
        void WriteUnindented(string text, params object[] format);
        void WriteLineUnindented();
        void WriteLineUnindented(object obj);
        void WriteLineUnindented(string text, params object[] format);
    }

    public class CodeWriter : ICodeWriter,IDisposable
    {
        StreamWriter outStreamWriter;
        string prefix;

        public CodeWriter(Stream outStream)
        {
            outStreamWriter = new StreamWriter(outStream);
        }

        public void Indent()
        {
            prefix += "\t";
        }

        public void UnIndent()
        {
            if (prefix.EndsWith("\t"))
                prefix = prefix.Substring(0, prefix.Length - 1);
        }

        public void WriteLine()
        {
            outStreamWriter.WriteLine(prefix);
        }

        public void WriteLine(object obj)
        {
            outStreamWriter.WriteLine(prefix + obj);
        }

        public void WriteLine(string text, params object[] format)
        {
            outStreamWriter.WriteLine(prefix + text, format);
        }

        public void Write(object obj)
        {
            outStreamWriter.Write(prefix + obj);
        }

        public void Write(string text, params object[] format)
        {
            outStreamWriter.Write(prefix + text, format);
        }

        public void Flush()
        {
            outStreamWriter.Flush();
        }

        public void Dispose()
        {
            outStreamWriter.Dispose();
        }


        public void WriteUnindented(object obj)
        {
            outStreamWriter.Write(obj);
        }

        public void WriteUnindented(string text, params object[] format)
        {
            outStreamWriter.Write(text, format);
        }

        public void WriteLineUnindented()
        {
            outStreamWriter.WriteLine();
        }

        public void WriteLineUnindented(object obj)
        {
            outStreamWriter.WriteLine(obj);
        }

        public void WriteLineUnindented(string text, params object[] format)
        {
            outStreamWriter.WriteLine(text, format);
        }
    }
}
