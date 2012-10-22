using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Compilers.IL;

namespace System.Compilers.AST
{

    public enum ProgramCreation
    {
        Composition,
        Declaration
    }

    public abstract class NetAstProgramBase : NetAstNode
    {
        ProgramCreation creationFlag;

        internal NetAstProgramBase(ProgramCreation creationFlag)
        {
            this.creationFlag = creationFlag;
        }

        public ProgramCreation CreationMode
        {
            get { return creationFlag; }
        }

        public IEnumerable<NetMemberDeclarationAST> Declarations { get { return _Members; } }

        List<NetMemberDeclarationAST> _Members = new List<NetMemberDeclarationAST>();

        protected void Add(NetMemberDeclarationAST member)
        {
            this._Members.Add(member);
        }

        NetMethodDeclarationAST _Main;
        public NetMethodDeclarationAST Main
        {
            get { return _Main; }
            set
            {
                if (value == null || Declarations.Contains(value))
                    _Main = value;
                else
                    throw new ArgumentException();
            }
        }

        public static NetAstProgramByDeclaration DeclareProgram()
        {
            return new NetAstProgramByDeclaration();
        }

        public static NetAstProgramByComposition CompoundProgram()
        {
            return new NetAstProgramByComposition();
        }
    }

    public class NetAstProgramByDeclaration : NetAstProgramBase
    {
        Type _ProgramType;
        Assembly _Assembly;
        Module _Module;

        static int ProgramID = 0;

        public NetMethodDeclarationAST DeclareNewMethod(string method, MethodAttributes attributes)
        {
            var methodBuilder = Builder.DefineMethod(method, attributes);

            var member = new NetMethodDeclarationAST(methodBuilder);

            Add(member);

            return member;
        }

        public NetFieldDeclarationAST DeclareNewField(string name, Type fieldType, FieldAttributes attributes)
        {
            var fieldBuilder = Builder.DefineField(name, fieldType, attributes);

            var member = new NetFieldDeclarationAST(fieldBuilder);

            Add(member);

            return member;
        }

        TypeBuilder Builder { get { return _ProgramType as TypeBuilder; } }

        internal NetAstProgramByDeclaration()
            : base(ProgramCreation.Declaration)
        {
            ProgramID++;

            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("Program_" + ProgramID), AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("Module_" + ProgramID);
            TypeBuilder typeBuilder = moduleBuilder.DefineType("Program" + ProgramID);

            _ProgramType = typeBuilder;
            _Module = moduleBuilder;
            _Assembly = assemblyBuilder;
        }
    }

    public class NetAstProgramByComposition : NetAstProgramBase
    {
        internal NetAstProgramByComposition()
            : base(ProgramCreation.Composition)
        {
        }

        public NetTypeDeclarationAST Include(Type type)
        {
            NetTypeDeclarationAST typeDec = new NetTypeDeclarationAST(type);

            Add(typeDec);

            return typeDec;
        }

        public NetMethodDeclarationAST Include(MethodInfo method)
        {
            var instructions = ILDecompiler.GetMethodBody (method).Instructions;
            NetMethodDeclarationAST methodDec = new NetMethodDeclarationAST(method, instructions);

            Add(methodDec);

            return methodDec;
        }

        public NetMethodDeclarationAST Include(Delegate d)
        {
            return Include(d.Method);
        }
    }
}
