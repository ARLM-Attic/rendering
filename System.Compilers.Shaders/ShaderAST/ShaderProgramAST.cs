using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.Info;
using System.Compilers.Transformers;
using System.Net.Sockets;
using System.Net;

namespace System.Compilers.Shaders.ShaderAST
{
    /// <summary>
    /// Represents a Shader Program with a main function.
    /// </summary>
    public class ShaderProgramAST : ShaderNodeAST
    {
        public enum AddingMode { Append, Prepend }

        private Stack<AddingMode> _modes = new Stack<AddingMode>();

        public void PushMode(AddingMode mode)
        {
            this._modes.Push(DeclarationMode);
            DeclarationMode = mode;
        }

        public void PopMode()
        {
            this.DeclarationMode = _modes.Pop();
        }

        private static void InsertionSort<T>(List<T> list, Comparison<T> cmp)
        {
            for (int k = 1; k < list.Count; k++)
            {
                int i = k;
                while (i > 0 && cmp(list[i], list[i - 1]) < 0)
                {
                    var t = list[i];
                    list[i] = list[i - 1];
                    list[i - 1] = t;

                    i--;
                }
            }
        }

        public void SortMembers()
        {
            InsertionSort(_Members, (m1, m2) =>
            {
                int n1 = m1 is ShaderTypeDeclarationAST ? 0 : m1 is ShaderFieldDeclarationAST ? 1 : 2;
                int n2 = m2 is ShaderTypeDeclarationAST ? 0 : m2 is ShaderFieldDeclarationAST ? 1 : 2;
                return n1.CompareTo(n2);
            });
        }

        public AddingMode DeclarationMode { get; private set; }

        /// <summary>
        /// Creates a program with a Builtins object used to access the buit-ins functions, variables and types.
        /// </summary>
        public ShaderProgramAST(Builtins builtins)
            : base(null)
        {
            DeclarationMode = AddingMode.Prepend;
            this.Builtins = builtins;
            this.Constants = new ConstantProxy();
            this.Dynamics = new DynamicMembers();
        }

        public IConstantProxy Constants { get; internal set; }

        public IDynamicMembers Dynamics { get; internal set; }
        /// <summary>
        /// Gets the Builtins object used by this program.
        /// </summary>
        public Builtins Builtins { get; private set; }

        public bool IsAbstract { get { return Dynamics.Methods.Count() > 0; } }

        public Semantic[] InputDeclaration
        {
            get
            {
                return this.Main.Parameters.First().ParameterType.Members.OfType<ShaderField>().Select(f => f.Semantic).ToArray();
            }
        }

        private Tuple<Semantic, ShaderType>[] GetSemanticsForData(ShaderType type)
        {
            if (type.IsArray)
                return GetSemanticsForData(type.ElementType);
            else
                return type.Members.OfType<ShaderField>().Select(f => Tuple.Create(f.Semantic, f.Type)).ToArray();
        }

        public Tuple<Semantic, ShaderType>[] OutputDeclaration
        {
            get
            {
                return GetSemanticsForData(this.Main.ReturnType);
            }
        }

        List<ShaderMemberDeclarationAST> _Members = new List<ShaderMemberDeclarationAST>();

        public void Include(ShaderProgramAST program, out Dictionary<ShaderMember, ShaderMember> maps, out Dictionary<ShaderMember, ShaderMember> compilations)
        {
            if (program.Builtins != this.Builtins)
                throw new ArgumentException("To include a program it should have the same builtins than current");

            ShaderToShaderTransformer cloner = new ShaderToShaderTransformer(this);
            cloner.Transform(program);

            maps = cloner.Maps;
            compilations = cloner.Compilations;
        }

        public void Implement(ShaderMethod abstractMethod, Action<ShaderMethodBuilder> body)
        {
            ShaderMethodDeclarationAST dec = Members.OfType<ShaderMethodDeclarationAST>().First(m => m.Method.Equals(abstractMethod));

            var mb = dec.GetMethodBuilder();

            body(mb);
        }

        /// <summary>
        /// Declares a new struct in this program.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="numberOfGenericParameters"></param>
        /// <returns></returns>
        public ShaderTypeDeclarationAST DeclareNewStruct(string name, int numberOfGenericParameters)
        {
            var type = new ShaderTypeDeclarationAST(this, null, numberOfGenericParameters, ShaderTypeType.Struct, name);

            switch (DeclarationMode)
            {
                case AddingMode.Append:
                    _Members.Add(type);
                    break;
                case AddingMode.Prepend:
                    _Members.Insert(0, type);
                    break;
            }

            return type;
        }

        /// <summary>
        /// Declares a new method in this program.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="returnType"></param>
        /// <param name="numberOfGenericParameters"></param>
        /// <returns></returns>
        public ShaderMethodDeclarationAST DeclareNewMethod(string name, bool isAbstract, int numberOfGenericParameters)
        {
            var method = new ShaderMethodDeclarationAST(this, null, name, isAbstract, numberOfGenericParameters);

            switch (DeclarationMode)
            {
                case AddingMode.Append:
                    _Members.Add(method);
                    break;
                case AddingMode.Prepend:
                    _Members.Insert(0, method);
                    break;
            }

            return method;
        }

        /// <summary>
        /// Declares a new method in this program.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="returnType"></param>
        /// <param name="numberOfGenericParameters"></param>
        /// <returns></returns>
        public ShaderMethodDeclarationAST DeclareNewMethod(string name, int numberOfGenericParameters)
        {
            return DeclareNewMethod(name, false, numberOfGenericParameters);
        }

        public void SetReturnType(ShaderMethodDeclarationAST methodDeclaration, ShaderType returnType)
        {
            methodDeclaration.ReturnType = returnType;
        }

        /// <summary>
        /// Declares a new field in this program.
        /// </summary>
        public ShaderFieldDeclarationAST DeclareNewField(string name, ShaderType fieldType, Semantic semantic)
        {
            ShaderFieldDeclarationAST global = new ShaderFieldDeclarationAST(this, null, fieldType, name, semantic);

            switch (DeclarationMode)
            {
                case AddingMode.Append:
                    _Members.Add(global);
                    break;
                case AddingMode.Prepend:
                    _Members.Insert(0, global);
                    break;
            }

            return global;
        }

        ShaderMethod _Main;
        /// <summary>
        /// Gets or sets the main function of this program. 
        /// This function should be declared before as part of this program.
        /// </summary>
        public ShaderMethod Main
        {
            get { return _Main; }
            set
            {
                if (value == null)
                    _Main = null;
                else
                {
                    if (!Members.Any(m => m is ShaderMethodDeclarationAST && ((ShaderMethodDeclarationAST)m).Method == value))
                        throw new ArgumentException("Method Declaration of Main method should be part of the program");

                    if (value.DeclaringType != null)
                        throw new ArgumentException("Main method should be on program scope");

                    _Main = value;
                }
            }
        }

        /// <summary>
        /// Gets the members of this program.
        /// </summary>
        public IEnumerable<ShaderMemberDeclarationAST> Members
        {
            get
            {
                return _Members.ToArray();
            }
        }

        #region AST Creation Methods

        internal ShaderInitializationInvokeAST CreateInitialization(ShaderType type)
        {
            return new ShaderInitializationInvokeAST(this, type);
        }

        internal ShaderFieldInvokeAST CreateInvoke(ShaderField field, ShaderExpressionAST target)
        {
            if (field == null)
                throw new ArgumentNullException("field");

            return new ShaderFieldInvokeAST(this, field, target);
        }

        internal ShaderMethodInvokeAST CreateInvoke(ShaderMethod method, ShaderExpressionAST target, params ShaderExpressionAST[] arguments)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            if (method.Operator != Operators.None)
                throw new ArgumentException("method can not be an operator.");

            return new ShaderMethodInvokeAST(this, method, target, arguments);
        }

        internal ShaderConstructorInvokeAST CreateInvoke(ShaderConstructor constructor, params ShaderExpressionAST[] arguments)
        {
            if (constructor == null)
                throw new ArgumentNullException("constructor");

            return new ShaderConstructorInvokeAST(this, constructor, arguments);
        }

        internal ShaderLocalInvokeAST CreateInvoke(ShaderLocal local)
        {
            if (local == null)
                throw new ArgumentNullException("local");

            return new ShaderLocalInvokeAST(this, local);
        }

        internal ShaderParameterInvokeAST CreateInvoke(ShaderParameter parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException("parameter");

            return new ShaderParameterInvokeAST(this, parameter);
        }

        internal ShaderConstantExpressionAST CreateConstant<T>(T value)
        {
            return CreateConstant(typeof(T), value);
        }

        internal ShaderConstantExpressionAST CreateConstant(ShaderType type, object value)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return new ShaderConstantExpressionAST(this, type, value);
        }

        internal ShaderConstantExpressionAST CreateConstant(Type type, object value)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (Builtins.Contains(type))
                return CreateConstant(Builtins.Resolve(type), value);
            else
                throw new ArgumentException("Type " + type + " is not a builtin type");
        }

        internal ShaderOperationAST CreateOperation(Operators op, params ShaderExpressionAST[] arguments)
        {
            return new ShaderOperationAST(this, op, arguments);
        }

        internal ShaderConversionAST CreateConversion(ShaderType target, ShaderExpressionAST expression)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            if (expression == null)
                throw new ArgumentNullException("expression");

            return new ShaderConversionAST(this, target, expression);
        }

        internal ShaderAssignamentAST CreateAssignament(ShaderExpressionAST leftSide, ShaderExpressionAST rightSide)
        {
            return new ShaderAssignamentAST(leftSide, rightSide);
        }

        internal ShaderExpressionStatementAST CreateExpressionStatement(ShaderExpressionAST expression)
        {
            return new ShaderExpressionStatementAST(this, expression);
        }

        internal ShaderReturnStatementAST CreateReturn(ShaderExpressionAST shaderExpressionAST)
        {
            return new ShaderReturnStatementAST(this, shaderExpressionAST);
        }

        internal ShaderWhileStatementAST CreateWhile(ShaderExpressionAST conditional, ShaderBlockStatementAST shaderBlockStatementAST)
        {
            return new ShaderWhileStatementAST(this, conditional, shaderBlockStatementAST);
        }

        internal ShaderDoWhileStatementAST CreateDoWhile(ShaderExpressionAST conditional, ShaderBlockStatementAST shaderBlockStatementAST)
        {
            return new ShaderDoWhileStatementAST(this, conditional, shaderBlockStatementAST);
        }

        internal ShaderForStatementAST CreateFor(ShaderStatementAST initialization, ShaderExpressionAST conditional, ShaderStatementAST increment,
            ShaderBlockStatementAST body)
        {
            return new ShaderForStatementAST(this, initialization, conditional, increment, body);
        }

        internal ShaderConditionalStatement CreateConditional(ShaderExpressionAST conditional, ShaderBlockStatementAST whenTrueBlock, ShaderBlockStatementAST whenFalseBlock)
        {
            return new ShaderConditionalStatement(this, conditional, whenTrueBlock, whenFalseBlock);
        }

        internal ShaderLocalDeclarationAST CreateLocalDeclaration(ShaderType shaderType, string name)
        {
            return new ShaderLocalDeclarationAST(this, shaderType, name);
        }

        internal ShaderBlockStatementAST CreateBlock(IEnumerable<ShaderStatementAST> statements)
        {
            var block = new ShaderBlockStatementAST(this);
            foreach (var s in statements)
                block.StatementList.Add(s);

            return block;
        }

        #endregion
    }
}
