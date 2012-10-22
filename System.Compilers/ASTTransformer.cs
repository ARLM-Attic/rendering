using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System.Compilers
{
    public abstract class ASTTransformer<S, D> where S:class where D:class
    {
        public DAST Transform<SAST, DAST>(SAST sourceAST)
            where SAST : S
            where DAST : D
        {
            if (object.Equals(sourceAST, default(SAST)))
                return default(DAST);

            if (this is ITransformerOf<SAST, DAST>)
                return ((ITransformerOf<SAST, DAST>)this).Transform(sourceAST);

            return default(DAST);
        }

        public DAST TryTransform<SAST, DAST>(S sourceAST)
            where SAST : S
            where DAST : D
        {
            if (sourceAST is SAST)
                return Transform<SAST, DAST>((SAST)sourceAST);

            return default(DAST);
        }

        public ASTTransformer()
        {
            transformMethods = this.GetType().GetMethods().Where(m => m.Name.Equals("Transform") && m.GetParameters().Length == 1).ToList();
        }

        IEnumerable<MethodInfo> transformMethods;

        protected DAST ResolveDynamically<SAST, DAST>(SAST ast)
            where SAST : S
            where DAST : D
        {
            foreach (var m in transformMethods)
                if (m.GetParameters()[0].ParameterType.Equals(ast.GetType()) &&
                    (m.ReturnType.IsSubclassOf(typeof(DAST)) || m.ReturnType.Equals(typeof(DAST))))
                {
                    var value = (DAST)m.Invoke(this, new object[] { ast });
                    if (!object.Equals(value, null))
                        return value;
                }

            return default (DAST);
        }
    }


    public interface ITransformerOf<S,D>
    {
        D Transform(S source);
    }
}
