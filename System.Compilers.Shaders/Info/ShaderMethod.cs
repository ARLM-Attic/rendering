using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System.Compilers.Shaders.Info
{
    /// <summary>
    /// Allows to retrieve information about a method in a shader program
    /// </summary>
    public interface ShaderMethod : ShaderMethodBase
    {
        /// <summary>
        /// Gets the return type of this method.
        /// </summary>
        ShaderType ReturnType { get; }

        /// <summary>
        /// Gets when this method is a generic definition.
        /// </summary>
        bool IsGenericDefinition { get; }
        
        /// <summary>
        /// Gets when this method is an instance of a generic definition.
        /// </summary>
        bool IsGenericMethod { get; }

        /// <summary>
        /// Gets the generic definition this method was created.
        /// </summary>
        /// <returns></returns>
        ShaderMethod GetGenericDefinition();

        /// <summary>
        /// Gets the generic type arguments of this generic method.
        /// </summary>
        ShaderType[] GetGenericParameters();

        /// <summary>
        /// Gets operator type this method represents.
        /// </summary>
        Operators Operator { get; }

        /// <summary>
        /// Gets attributes of this method.
        /// </summary>
        // Note[] Notes { get; }

        /// <summary>
        /// Gets if this definition is an abstract definition.
        /// </summary>
        bool IsAbstract { get; }
    }

    public class MyClass 
    {
        int Slices = 16, Stacks = 16;

        void TessellatorFunction()
        {
            Annotation.LimitOfStreamOutput(Slices * Stacks);


        }
    }

    public class Note
    {
        public int ID { get; private set; }

        static int __id = 0;

        public Note()
        {
            ID = __id++;
        }
    }

    public enum MemberType 
    {
        Statement,
        Method
    }

    [AttributeUsage (AttributeTargets.Method)]
    public class AnnotationAttribute : Attribute
    {
        public MemberType Member { get; set; }
    }

    public class StatementAnnotation : AnnotationAttribute
    {
    }

    public class MethodAnnotation : AnnotationAttribute
    {
    }

    public class UnrollNote : Note
    {
        public int MaxLoops { get; set; }
    }

    public static class Annotation
    {
        [StatementAnnotation]
        public static Note Unroll(int N)
        {
            return new UnrollNote() { MaxLoops = N };
        }

        [MethodAnnotation]
        public static Note LimitOfStreamOutput(int N)
        {
            return new Note();
        }
    }
}
