using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace System.Compilers
{
    /// <summary>
    /// Implements several ICodeGeneratorOf interface instances to support ast nodes serialization.
    /// </summary>
    /// <typeparam name="ASTBase"></typeparam>
    public abstract class CodeGenerator<ASTBase>
    {
        public CodeGenerator()
        {
            this.IndentationString = "\t";
            this.LineBreakString = "\n";
        }

        private int Distance(Type _base, Type _class)
        {
            if (!_base.IsAssignableFrom(_class))
                return int.MaxValue;

            if (_base.Equals(_class))
                return 0;

            return Distance(_base, _class.BaseType) + 1;
        }

        public string IndentationString
        {
            get;
            set;
        }

        public string LineBreakString { get; set; }

        protected string Indent(string code)
        {
            return string.Join(LineBreakString, code.Split(new string[] { LineBreakString }, StringSplitOptions.None).Select(inst => IndentationString + inst).ToArray());
        }

        protected string Join(IEnumerable<string> code)
        {
            return string.Join(LineBreakString, code.ToArray());
        }

        public string GetCode<T>(T ast) where T : ASTBase
        {
            if (ast == null)
                throw new ArgumentNullException();

            if (this is ICodeGeneratorOf<T>)
                return Join (((ICodeGeneratorOf<T>)this).GetCode(ast));
            else
            { // try resolve closer by reflection.
                Type t = ast.GetType();

                Type implementedInterface = this.GetType().GetInterfaces().Where (i=>i.GetGenericArguments().Length == 1).Aggregate((min, _base) => Distance(_base.GetGenericArguments()[0], t) < Distance(min.GetGenericArguments()[0], t) ? _base : min);

                if (Distance(implementedInterface.GetGenericArguments()[0], t) < int.MaxValue)
                {
                    return Join((IEnumerable<string>)implementedInterface.InvokeMember("GetCode", Reflection.BindingFlags.Public | Reflection.BindingFlags.Instance | Reflection.BindingFlags.InvokeMethod | Reflection.BindingFlags.NonPublic, Type.DefaultBinder, this, new object[] { ast }));
                }
                else
                    return GetUnresolvedCode(ast);
            }
        }

        public abstract string GetUnresolvedCode(ASTBase ast);
    }

    public interface ICodeGeneratorOf<T>
    {
        IEnumerable<string> GetCode(T ast);
    }
}
