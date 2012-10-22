//#define DIAGNOSE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.ShaderAST;
using System.Reflection;
using System.Compilers.Shaders.Info;
using System.Compilers.AST;
using System.Compilers.Transformers;
using System.Diagnostics;

namespace System.Compilers.Shaders
{
    public partial class ShaderProgramFactory
    {
        public static ShaderProgramAST Build<VIN, VOUT>(Func<VIN, VOUT> main, Builtins builtins)
        {
            return Build((Delegate)main, builtins);
        }

        public static ShaderProgramAST Build(Delegate main, Builtins builtins)
        {
            return Build(main.Method, builtins);
        }

        private static Dictionary<MethodInfo, Dictionary<Builtins, ShaderProgramAST>> __internalCache = new Dictionary<MethodInfo, Dictionary<Builtins, ShaderProgramAST>>();

        public static ShaderProgramAST Build(MethodInfo main, Builtins builtins)
        {
#if DIAGNOSE
            Stopwatch clock = new Stopwatch();
            clock.Start();

            Debug.WriteLine("Building " + main.Name + " as a Shader.");
#endif
            if (!__internalCache.ContainsKey(main))
                __internalCache.Add(main, new Dictionary<Builtins, ShaderProgramAST>());

            if (!__internalCache[main].ContainsKey(builtins))
            {
                NetToShaderASTTransformer transformer = new NetToShaderASTTransformer(builtins);

                var mainDeclaration = transformer.Transform(NetMethodDeclarationAST.CreateReadonly(main));

                var program = transformer.Program;

                program.Main = mainDeclaration.Method;

                program.Constants = transformer.ConstantProxy;

                program.Dynamics = transformer.Dynamics;

                __internalCache[main].Add(builtins, program);
            }

#if DIAGNOSE
            Debug.WriteLine("Building succed in " + clock.Elapsed.TotalMilliseconds + " ms");
#endif

            return __internalCache[main][builtins];
        }
    }

    /// <summary>
    /// Allows to access to global variable values from the application that creates the Shader Program.
    /// </summary>
    public interface IConstantProxy
    {
        IEnumerable<ShaderField> Fields { get; }

        object GetValueFor(object target, ShaderField field);
    }

    /// <summary>
    /// Allows to access to global method values from the application that creates the Shader Program.
    /// </summary>
    public interface IDynamicMembers
    {
        IEnumerable<ShaderMethod> Methods { get; }

        Delegate GetMethod(object target, ShaderMethod method);
    }

    public abstract class ShaderCompilantAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Struct)]
    public class ShaderTypeAttribute : ShaderCompilantAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ShaderMethodAttribute : ShaderCompilantAttribute
    {
    }
    
}


