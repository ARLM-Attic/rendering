using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System.Rendering.Effects.Shaders
{
    /// <summary>
    /// Defines functionallities for a Shader Program compiler.
    /// </summary>
    /// <typeparam name="TInstruction"></typeparam>
    public interface IASTCompiler<TInstruction>
    {
        /// <summary>
        /// Gets the current compiling shader program AST.
        /// </summary>
        ShaderProgramAST CompilingAST { get; }

        /// <summary>
        /// Compiles the current program to a secuence of instructions.
        /// </summary>
        /// <returns>A IEnumerable object with the instructions.</returns>
        IEnumerable<TInstruction> Compile ();
    }

    public class CompilerBase<TInstruction> : IASTCompiler<TInstruction>
    {
        MethodInfo[] compillingMethods;

        public ShaderProgramAST CompilingAST { get; private set; }

        public CompilerBase(ShaderProgramAST ast)
        {
            this.CompilingAST = ast;

            Type thisType = GetType();
            MethodInfo[] methods = thisType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            List<MethodInfo> compillingMethods = new List<MethodInfo>();
            foreach (MethodInfo method in methods)
                if (method.Name == "Compile" && method.ReturnType == typeof(IEnumerable<TInstruction>) &&
                    method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType.IsSubclassOf(typeof(ShaderNodeAST)))
                    compillingMethods.Add(method);
            this.compillingMethods = compillingMethods.ToArray();
        }

        int DistanceBetween(Type a, Type b)
        {
            if (b == a) return 0;
            if (b.IsSubclassOf(a))
                return DistanceBetween(a, b.BaseType)+1;
            return int.MaxValue;
        }

        MethodInfo ResolveClosestMethod(ShaderNodeAST ast)
        {
            int closest = int.MaxValue;
            MethodInfo method = null;
            foreach (MethodInfo m in compillingMethods)
            {
                int dist = DistanceBetween(m.GetParameters()[0].ParameterType, ast.GetType());
                if (dist < closest)
                {
                    closest = dist;
                    method = m;
                }
            }
            return method;
        }

        #region IASTCompiler<TInstruction> Members

        protected virtual IEnumerable<TInstruction> CompileUnknown(ShaderNodeAST ast)
        {
            yield return default(TInstruction);
        }

        public IEnumerable<TInstruction> Compile()
        {
            return Compile(CompilingAST);
        }

        protected IEnumerable<TInstruction> Compile(ShaderNodeAST ast)
        {
            if (ast.GetType() == typeof(ShaderNodeAST))
                return CompileUnknown(ast);
            MethodInfo method = ResolveClosestMethod(ast);
            if (method == null)
                return CompileUnknown(ast);
            return (IEnumerable<TInstruction>)method.Invoke(this, new object[] { ast });
        }

        #endregion
    }

    public class CodeCompiler : CompilerBase<string>
    {
        public CodeCompiler(ShaderProgramAST ast) : base(ast) { }
    }
}
