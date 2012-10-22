using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Rendering.Effects.Shaders;
using System.Compilers.Shaders;
using System.Compilers.Shaders.ShaderAST;
using System.Compilers.Shaders.Info;

namespace System.Rendering.RenderStates
{
    public struct VertexShaderState
    {
        public static readonly VertexShaderState Default = new VertexShaderState();

        internal IEnumerable<ShaderSource> functions;

        private ShaderProgramAST BuildASTFor(IEnumerable<ShaderSource> functions, Builtins builtins, out GlobalBindings globals)
        {
            return ShaderTools.BuildASTFor(functions, builtins, out globals, ShaderKind.Vertex);
        }

        public ShaderProgramAST BuildASTFor(Builtins builtins, out GlobalBindings globals)
        {
            if (functions == null)
            {
                globals = null;
                return null;
            }

            return BuildASTFor(functions, builtins, out globals);
        }

        public VertexShaderState(ShaderSource function)
            : this(function, null)
        {
        }

        internal VertexShaderState(ShaderSource function, IEnumerable<ShaderSource> before)
        {
            this.functions = (before ?? new ShaderSource[0]).Union(new ShaderSource[] { function });
        }

        internal VertexShaderState(IEnumerable<ShaderSource> first, IEnumerable<ShaderSource> then)
        {
            this.functions = then.Union(first);
        }

        public static VertexShaderState Join(VertexShaderState previous, VertexShaderState current)
        {
            return new VertexShaderState(current.functions, previous.functions);
        }
    }

    sealed class LinkedNode <T>
    {
        public T Value { get; set; }

        public LinkedNode<T> Next { get; set; }
    }

    public struct PixelShaderState
    {
        public static readonly PixelShaderState Default = new PixelShaderState();

        internal IEnumerable<ShaderSource> functions;

        private ShaderProgramAST BuildASTFor(IEnumerable<ShaderSource> lastFunction, Builtins builtins, out GlobalBindings globals)
        {
            return ShaderTools.BuildASTFor(lastFunction, builtins, out globals, ShaderKind.Pixel);
        }

        public ShaderProgramAST BuildASTFor(Builtins builtins, out GlobalBindings globals)
        {
            if (functions == null)
            {
                globals = null;
                return null;
            }

            return BuildASTFor(functions, builtins, out globals);
        }

        public PixelShaderState(ShaderSource function) : this(function, null) { }

        internal PixelShaderState(ShaderSource function, IEnumerable<ShaderSource> before)
        {
            this.functions = (before ?? new ShaderSource[0]).Union(new ShaderSource[] { function });
        }

        internal PixelShaderState(IEnumerable<ShaderSource> first, IEnumerable<ShaderSource> then)
        {
            this.functions = first.Union(then);
        }

        public static PixelShaderState Join(PixelShaderState current, PixelShaderState previous)
        {
            List<ShaderSource> shaders = new List<ShaderSource>();

            return new PixelShaderState(current.functions, previous.functions);
        }

    }

    enum ShaderKind {
        Vertex,
        Geometry,
        Pixel
    }

    class ShaderTools
    {
        public static ShaderProgramAST BuildASTFor(IEnumerable<ShaderSource> functions, Builtins builtins, out GlobalBindings globals, ShaderKind kind)
        {
            ShaderProgramAST currentAST = ShaderBuilder.Build(functions.First(), builtins, out globals);

            foreach (var source in functions.Skip(1))
            {
                GlobalBindings previousGlobals;
                ShaderProgramAST next = ShaderBuilder.Build(source, builtins, out previousGlobals);

                Dictionary<ShaderMember, ShaderMember> maps;
                switch (kind)
                {
                    case ShaderKind.Vertex:
                        currentAST = ShaderJoiner.JoinVertexShaderVertexShader(currentAST, next, out maps);
                        break;
                    case ShaderKind.Pixel:
                        currentAST = ShaderJoiner.JoinPixelShaderPixelShader(currentAST, next, out maps);
                        break;
                    default: throw new NotSupportedException();
                }

                globals = GlobalBindings.Concat(previousGlobals.Map(maps), globals.Map(maps));
            }

            return currentAST;
        }
    }
    
}