#define FLOAT

#if DECIMAL
using FLOATINGTYPE = System.Decimal;
#endif
#if DOUBLE
using FLOATINGTYPE = System.Double;
#endif
#if FLOAT
using FLOATINGTYPE = System.Single;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Compilers.Shaders.Reflection;
using System.Collections.ObjectModel;

namespace System.Compilers.Shaders.AST
{
    public abstract class ShaderNodeAST
    {
        /// <summary>
        /// Gets or sets a debugging label for this node.
        /// </summary>
        public string Label { get; set; }

        public virtual IEnumerable<ShaderNodeAST> Children
        {
            get
            {
                yield break;
            }
        }

        public override string ToString()
        {
            if (Label == null)
                return GetType().Name;
            return Label;
        }
    }

    /// <summary>
    /// Represents a shader program, vertex, pixel or geometry shader.
    /// </summary>
    public class ShaderProgramAST : ShaderNodeAST
    {
        /// <summary>
        /// Gets the declaration list of this program.
        /// </summary>
        public List<DeclarationAST> Declarations { get; private set; }

        private FunctionDeclarationAST _Main;
        /// <summary>
        /// Gets or sets the function will be main.
        /// </summary>
        public FunctionDeclarationAST Main
        {
            get { return _Main; }
            set
            {
                if (value != null && !Declarations.Contains(value))
                    throw new ArgumentOutOfRangeException("Main function should be in declaration list");
                _Main = value;
            }
        }

        /// <summary>
        /// Gets access to global declarations in this shader.
        /// </summary>
        public IEnumerable<GlobalDeclarationAST> Globals { get { return Declarations.OfType<GlobalDeclarationAST>(); } }

        /// <summary>
        /// Gets access to function declarations in this shader.
        /// </summary>
        public IEnumerable<FunctionDeclarationAST> Functions { get { return Declarations.OfType<FunctionDeclarationAST>(); } }

        /// <summary>
        /// Initializes a program ast with an empty declaration list.
        /// </summary>
        public ShaderProgramAST() { Declarations = new List<DeclarationAST>(); }

        /// <summary>
        /// Renames all declaration names CLR compilant using a builtin set to check keywords.
        /// </summary>
        /// <param name="builtins">Builtins object to check keywords.</param>
        public void FixNames(Builtins builtins)
        {
            foreach (var d in Declarations)
                d.FixNames(builtins);

            DeclarationAST.FixDeclarationNames(Declarations, builtins);
        }

        #region Lo que añadí

        public StructDeclarationAST In { get { return (StructDeclarationAST)Declarations.Where(d => d is StructDeclarationAST && ((StructDeclarationAST)d).Type == Main.Parameters[0].Type).First(); } }

        public StructDeclarationAST Out { get { return (StructDeclarationAST)Declarations.Where(d => d is StructDeclarationAST && ((StructDeclarationAST)d).Type == Main.ReturnType).First(); } }

        public static ShaderProgramAST Join(ShaderProgramAST first, ShaderProgramAST second)
        {
            if (first == null)
                return second;

            StructDeclarationAST inout = StructDeclarationAST.Join(first.In, first.Out, second.In, second.Out);

            FunctionDeclarationAST newMain = new FunctionDeclarationAST();
            newMain.Parameters.Add(new ParameterDeclarationAST { Label = "main", Type = inout.Type, IsIn = true });
            newMain.ReturnType = inout.Type;

            newMain.Body = GetJoinFunctionBody(first, second, newMain);

            ShaderProgramAST shader = new ShaderProgramAST();
            foreach (var declaration in first.Declarations.Concat(second.Declarations))
            {
                if (!shader.Declarations.Contains(declaration))
                    shader.Declarations.Add(declaration);
            }

            shader.Declarations.Add(inout);
            shader.Declarations.Add(newMain);
            shader.Main = newMain;

            return shader;
        }

        public static ShaderProgramAST Join(IEnumerable<ShaderProgramAST> shaders)
        {
            ShaderProgramAST result = null;
            foreach (var shader in shaders)
            {
                result = ShaderProgramAST.Join(result, shader);
            }
            return result;
        }

        public void RestrictInOutSemantics(IEnumerable<Semantics> supportedInSemantics, IEnumerable<Semantics> supportedOutSemantics)
        {
            List<FieldDeclarationAST> validInSemantics = new List<FieldDeclarationAST>();
            List<FieldDeclarationAST> validOutSemantics = new List<FieldDeclarationAST>();
            bool inOk = true, outOk = true;

            #region Verify if current in and out semantics are valid.
            //Verify if new In struct is needed
            foreach (var field in In.Fields)
            {
                if (!supportedInSemantics.Contains(field.Semantic))
                    inOk = false;
                else
                    validInSemantics.Add(field);
            }

            //Verify if new Out struct is needed
            foreach (var field in Out.Fields)
            {
                if (!supportedOutSemantics.Contains(field.Semantic))
                    outOk = false;
                else
                    validOutSemantics.Add(field);
            }
            #endregion

            #region Create new in and out structs if needed and new Main function.
            if (!inOk || !outOk)
            {
                FunctionDeclarationAST newMain = new FunctionDeclarationAST();

                StructDeclarationAST newInStruct = In;
                if (!inOk)
                {
                    newInStruct = new StructDeclarationAST();
                    foreach (var field in validInSemantics)
                        newInStruct.Fields.Add(new FieldDeclarationAST() { Type = field.Type, Semantic = field.Semantic });
                    Declarations.Add(newInStruct);
                }
                newMain.Parameters.Add(new ParameterDeclarationAST { Type = newInStruct.Type, IsIn = true });
                StructDeclarationAST newOutStruct = Out;
                if (!outOk)
                {
                    newOutStruct = new StructDeclarationAST();
                    foreach (var field in validOutSemantics)
                        newOutStruct.Fields.Add(new FieldDeclarationAST() { Type = field.Type, Semantic = field.Semantic });
                    Declarations.Add(newOutStruct);
                }

                newMain.ReturnType = newOutStruct.Type;
                newMain.Body = GetRestrictedFunctionBody(newMain, this);
                Declarations.Add(newMain);
                Main = newMain;
            }
            #endregion
        }

        private static BlockStatementAST GetJoinFunctionBody(ShaderProgramAST first, ShaderProgramAST second, FunctionDeclarationAST newMain)
        {
            BlockStatementAST result = new BlockStatementAST();

            BlockStatementAST firstCallCode = GetShaderMainCallBody(first, newMain);
            BlockStatementAST secondCallCode = GetShaderMainCallBody(second, newMain);
            ReturnStatementAST returnStatement = new ReturnStatementAST() { Value = new ParameterExpressionAST { Parameter = newMain.Parameters[0].Parameter } };
            foreach (var statement in firstCallCode.Statements.Concat(secondCallCode.Statements))
            {
                result.Statements.Add(statement);
            }
            result.Statements.Add(returnStatement);
            return result;
        }

        private static BlockStatementAST GetRestrictedFunctionBody(FunctionDeclarationAST newMain, ShaderProgramAST program)
        {
            BlockStatementAST result = new BlockStatementAST();

            //VSIn in = (VSIn)0;
            LocalDeclarationAST inShader = new LocalDeclarationAST() { Type = program.In.Type };
            ConstructionExpressionAST constructor = new ConstructionExpressionAST() { Member = program.In.Type.GetDefaultConstructor(), IsInitializing = false };
            AssignamentStatementAST setInDefaultValue = new AssignamentStatementAST() { Identifier = new LocalExpressionAST() { Local = inShader.Local }, Value = constructor };

            //Actualizar los campos comunes entre la entrada de newMain y la entrada del main del shader
            BlockStatementAST updateInShaderCode = GetUpdateValueCode(new ParameterExpressionAST { Parameter = newMain.Parameters[0].Parameter }, new LocalExpressionAST { Local = inShader.Local }, program);

            FunctionExpressionAST callShaderMain = new FunctionExpressionAST() { Member = program.Main.Function, Children = new List<ExpressionAST> { new LocalExpressionAST { Local = inShader.Local } } };
            //VSOut out;            
            LocalDeclarationAST outFirstShader = new LocalDeclarationAST() { Type = program.Out.Type };

            //out = Shader.Main(in);            
            AssignamentStatementAST setShaderOutValue = new AssignamentStatementAST() { Identifier = new LocalExpressionAST { Local = outFirstShader.Local }, Value = callShaderMain };

            //Atualizar los campos comunes entre out y el nuevo tipo de salida.            
            LocalDeclarationAST outValue = new LocalDeclarationAST() { Type = newMain.ReturnType };
            BlockStatementAST updateOutValueCode = GetUpdateValueCode(new LocalExpressionAST { Local = outFirstShader.Local }, new LocalExpressionAST { Local = outValue.Local }, program);

            result.Statements.Add(inShader);
            result.Statements.Add(setInDefaultValue);
            foreach (var statement in updateInShaderCode.Statements)
            {
                result.Statements.Add(statement);
            }

            result.Statements.Add(outFirstShader);
            result.Statements.Add(setShaderOutValue);
            result.Statements.Add(outValue);
            foreach (var statement in updateOutValueCode.Statements)
            {
                result.Statements.Add(statement);
            }
            result.Statements.Add(new ReturnStatementAST() { Value = new LocalExpressionAST() { Local = outValue.Local } });

            return result;
        }

        private static BlockStatementAST GetShaderMainCallBody(ShaderProgramAST shader, FunctionDeclarationAST newMain)
        {
            BlockStatementAST result = new BlockStatementAST();
            //VSIn in;
            LocalDeclarationAST inShader = new LocalDeclarationAST() { Type = shader.In.Type };
            //Actualizar los campos comunes entre la entrada de newMain y la entrada del main del shader
            BlockStatementAST updateInShaderCode = GetUpdateValueCode(new ParameterExpressionAST { Parameter = newMain.Parameters[0].Parameter }, new LocalExpressionAST { Local = inShader.Local }, shader);

            FunctionExpressionAST callShaderMain = new FunctionExpressionAST() { Member = shader.Main.Function, Children = new List<ExpressionAST> { new LocalExpressionAST { Local = inShader.Local } } };
            //VSOut out;
            LocalDeclarationAST outFirstShader = new LocalDeclarationAST() { Type = shader.Out.Type };
            //out = Shader.Main(in);
            AssignamentStatementAST setShaderOutValue = new AssignamentStatementAST() { Identifier = new LocalExpressionAST { Local = outFirstShader.Local }, Value = callShaderMain };
            //Atualizar los campos comunes entre out y el parametro de newMain
            BlockStatementAST updateOutValueCode = GetUpdateValueCode(new LocalExpressionAST { Local = outFirstShader.Local }, new ParameterExpressionAST { Parameter = newMain.Parameters[0].Parameter }, shader);

            result.Statements.Add(inShader);
            foreach (var statement in updateInShaderCode.Statements)
            {
                result.Statements.Add(statement);
            }

            result.Statements.Add(outFirstShader);
            result.Statements.Add(setShaderOutValue);

            foreach (var statement in updateOutValueCode.Statements)
            {
                result.Statements.Add(statement);
            }

            return result;
        }

        private static BlockStatementAST GetUpdateValueCode(VariableExpressionAST origin, VariableExpressionAST destiny, ShaderProgramAST program)
        {
            BlockStatementAST result = new BlockStatementAST();
            StructDeclarationAST originDeclaration = program.Declarations.OfType<StructDeclarationAST>().First(s => s.Type == ((FieldExpressionAST)origin).Field.Type);
            StructDeclarationAST destinyDeclaration = program.Declarations.OfType<StructDeclarationAST>().First(s => s.Type == ((FieldExpressionAST)destiny).Field.Type);
            foreach (var field in originDeclaration.Fields)
            {
                FieldDeclarationAST similar = destinyDeclaration.GetFieldBySemantic(field.Semantic);
                if (similar != null)
                {
                    //Si existe un campo comun en el origen y el destino hacer:
                    //destiny.similar = origin.field

                    //destiny.Similar
                    FieldExpressionAST leftExpression = new FieldExpressionAST() { LeftSide = destiny, Field = similar.Field };
                    //origin.field
                    FieldExpressionAST rightExpression = new FieldExpressionAST() { LeftSide = origin, Field = field.Field };

                    //destiny.similar = origin.field;
                    AssignamentStatementAST assignament = new AssignamentStatementAST() { Identifier = leftExpression, Value = rightExpression };

                    result.Statements.Add(assignament);
                }
            }
            return result;
        }

        #endregion
    }

    #region Declarations

    /// <summary>
    /// Represents an abstract declaration of a shader member. Suitable for methods, globals, parameters, fields, types, etc.
    /// </summary>
    public abstract class DeclarationAST : ShaderNodeAST
    {
        internal DeclarationAST() { }

        internal static void FixDeclarationNames<D>(IEnumerable<D> declarations, Builtins builtins) where D : DeclarationAST
        {
            HashSet<string> names = new HashSet<string>();

            foreach (var d in declarations)
            {
                if (d.Label == null || names.Contains(d.Label) || builtins.Keywords.Contains(d.Label))
                {
                    string futureName = (d.Label == null) ? "noname" : d.Label;
                    int i = 0;
                    while (names.Contains(futureName + i) || builtins.Keywords.Contains(futureName + i))
                        i++;
                    d.Label = futureName + i;
                }
                names.Add(d.Label);
            }
        }

        internal abstract void FixNames(Builtins set);
    }

    /// <summary>
    /// Represents an abstract type declaration.
    /// </summary>
    public abstract class TypeDeclarationAST : DeclarationAST
    {
        internal TypeDeclarationAST()
        {
        }

        /// <summary>
        /// When overriden, gets all type information for declared type.
        /// </summary>
        public abstract ShaderType Type { get; }
    }

    /// <summary>
    /// Represents a struct declaration.
    /// </summary>
    public sealed class StructDeclarationAST : TypeDeclarationAST
    {
        public StructDeclarationAST()
        {
            this.Fields = new List<FieldDeclarationAST>();
            _Type = new UserShaderType(this);
        }

        ShaderType _Type;
        public override ShaderType Type
        {
            get { return _Type; }
        }

        /// <summary>
        /// Gets the list with the field declarations of this struct. 
        /// </summary>
        public List<FieldDeclarationAST> Fields { get; private set; }

        internal override void FixNames(Builtins set)
        {
            FixDeclarationNames<FieldDeclarationAST>(Fields, set);
        }

        #region Esto lo puse yo (Javiel)
        //Metodo para dados dos declaraciones de struct unirlas en la declaracion de un solo struct
        public static StructDeclarationAST Join(params StructDeclarationAST[] declarations)
        {
            StructDeclarationAST union = new StructDeclarationAST() { Label = "union" };
            foreach (var declaration in declarations)
            {
                foreach (var field in declaration.Fields)
                {
                    if (union.GetFieldBySemantic(field.Semantic) == null)
                        union.Fields.Add(new FieldDeclarationAST() { Semantic = field.Semantic, Type = field.Type });
                }
            }
            return union;
        }


        public FieldDeclarationAST GetFieldBySemantic(Semantics semantic)
        {
            if (semantic == null)
                return null;
            foreach (var field in Fields)
            {
                if (field.Semantic != null && field.Semantic.Equals(semantic))
                    return field;
            }
            return null;
        }
        #endregion
    }

    public abstract class VariableDeclarationAST : DeclarationAST
    {
        internal VariableDeclarationAST() { }

        public ShaderType Type { get; set; }

        internal override void FixNames(Builtins set)
        {
        }
    }

    public abstract class SemantizedVariableDeclarationAST : VariableDeclarationAST
    {
        internal SemantizedVariableDeclarationAST() { }

        public Semantic Semantic { get; set; }
    }

    public sealed class GlobalDeclarationAST : SemantizedVariableDeclarationAST
    {
        public event Func<object> RequestData;

        public object Value
        {
            get
            {
                if (RequestData != null)
                    return RequestData();
                throw new InvalidOperationException("Data is not available");
            }
        }

        public GlobalDeclarationAST()
        {
            _Global = new UserShaderGlobal(this);
        }

        private UserShaderGlobal _Global;

        public ShaderGlobal Global
        {
            get { return _Global; }
        }
    }

    public sealed class FieldDeclarationAST : SemantizedVariableDeclarationAST
    {
        public FieldDeclarationAST()
        {
            _Field = new UserShaderField(this);
        }

        private UserShaderField _Field;

        public ShaderField Field { get { return _Field; } }
    }

    public sealed class ParameterDeclarationAST : SemantizedVariableDeclarationAST
    {
        public bool IsIn { get; set; }

        public bool IsOut { get; set; }

        public bool IsInOut { get { return !IsIn && !IsOut; } }

        public bool IsConst { get; set; }

        public ParameterDeclarationAST()
        {
            _Parameter = new UserShaderParameter(this);
        }

        private UserShaderParameter _Parameter;
        public ShaderParameter Parameter { get { return _Parameter; } }
    }

    public sealed class LocalDeclarationAST : VariableDeclarationAST, IStatementAST
    {
        public LocalDeclarationAST()
        {
            _Local = new UserShaderLocal(this);
        }

        private UserShaderLocal _Local;
        public ShaderLocal Local
        {
            get { return _Local; }
        }
    }

    public sealed class FunctionDeclarationAST : DeclarationAST
    {
        public FunctionDeclarationAST()
        {
            Body = new BlockStatementAST();
            Parameters = new List<ParameterDeclarationAST>();
            _Function = new UserShaderFunction(this);
        }

        ShaderMethod _Function;
        public ShaderMethod Function { get { return _Function; } }

        public ShaderType ReturnType { get; set; }

        public List<ParameterDeclarationAST> Parameters { get; set; }

        public BlockStatementAST Body { get; set; }

        internal override void FixNames(Builtins set)
        {
            FixDeclarationNames<ParameterDeclarationAST>(Parameters, set);
            FixDeclarationNames<LocalDeclarationAST>(Body.Statements.OfType<LocalDeclarationAST>(), set);
        }
    }

    #endregion

    #region Expressions

    public abstract class ExpressionAST : ShaderNodeAST
    {
        internal ExpressionAST()
        {
            SetBlankChildren();
        }

        public List<ExpressionAST> Children { get; set; }

        public virtual int PreferedNumberOfChildren { get { return 0; } }

        protected void SetBlankChildren()
        {
            Children = new List<ExpressionAST>();
            for (int i = 0; i < PreferedNumberOfChildren; i++)
                Children.Add(null);
        }
    }

    public abstract class UnaryExpressionAST : ExpressionAST
    {
        internal UnaryExpressionAST() { }

        public override int PreferedNumberOfChildren
        {
            get
            {
                return 1;
            }
        }

        public ExpressionAST Child { get { return Children[0]; } set { Children[0] = value; } }
    }

    public abstract class BinaryExpressionAST : ExpressionAST
    {
        internal BinaryExpressionAST() { }

        public override int PreferedNumberOfChildren
        {
            get
            {
                return 2;
            }
        }

        public ExpressionAST LeftChild
        {
            get { return Children[0]; }
            set { Children[0] = value; }
        }

        public ExpressionAST RightChild
        {
            get { return Children[1]; }
            set { Children[1] = value; }
        }
    }

    public sealed class BinaryOperatorExpressionAST : BinaryExpressionAST
    {
        public Operators Operator { get; set; }
    }

    public sealed class UnaryOperatorExpressionAST : UnaryExpressionAST
    {
        public Operators Operator { get; set; }
    }

    public abstract class NonArgumentExpressionAST : ExpressionAST
    {
        internal NonArgumentExpressionAST() { }

        public override int PreferedNumberOfChildren
        {
            get
            {
                return 0;
            }
        }
    }

    public abstract class CallExpressionAST : ExpressionAST, IStatementAST
    {
        internal CallExpressionAST() { }

        ShaderMethodBase _Function;
        internal virtual ShaderMethodBase Member
        {
            get { return _Function; }
            set
            {
                _Function = value;
            }
        }
    }

    public sealed class FunctionExpressionAST : CallExpressionAST
    {
        public FunctionExpressionAST() { }

        public ShaderMethod Function
        {
            get { return (ShaderMethod)Member; }
            set { Member = value; }
        }

        internal override ShaderMethodBase Member
        {
            get
            {
                return base.Member;
            }
            set
            {
                if (value is ShaderMethod)
                    base.Member = value;
                else
                    throw new InvalidOperationException();
            }
        }
    }

    public class ConstructionExpressionAST : CallExpressionAST
    {
        public ConstructionExpressionAST()
            : base()
        {
        }

        /// <summary>
        /// Gets or sets when the construction is initializing the struct. This method implies that Child 0 will be the expression to initialize.
        /// </summary>
        public bool IsInitializing { get; set; }

        public ShaderConstructor Constructor
        {
            get { return Member as ShaderConstructor; }
            set { Member = value; }
        }

        internal override ShaderMethodBase Member
        {
            get
            {
                return base.Member;
            }
            set
            {
                if (value is ShaderConstructor)
                    base.Member = value;
                else
                    throw new InvalidOperationException();
            }
        }
    }

    public sealed class ConstantExpressionAST : NonArgumentExpressionAST
    {
        public object Value { get; set; }
    }

    public sealed class ThisExpressionAST : NonArgumentExpressionAST
    {
    }

    public abstract class VariableExpressionAST : NonArgumentExpressionAST
    {
        internal VariableExpressionAST() { }

        public ShaderVariable Variable { get; protected set; }
    }

    public sealed class GlobalExpressionAST : VariableExpressionAST
    {
        public ShaderGlobal Global
        {
            get { return (ShaderGlobal)base.Variable; }
            set { base.Variable = value; }
        }
    }

    public sealed class ParameterExpressionAST : VariableExpressionAST
    {
        public ShaderParameter Parameter
        {
            get { return (ShaderParameter)base.Variable; }
            set { base.Variable = value; }
        }
    }

    public sealed class LocalExpressionAST : VariableExpressionAST
    {
        public ShaderLocal Local
        {
            get { return (ShaderLocal)base.Variable; }
            set { base.Variable = value; }
        }
    }

    public sealed class FieldExpressionAST : VariableExpressionAST
    {
        public FieldExpressionAST() { Children = new List<ExpressionAST>(); Children.Add(null); }

        public ExpressionAST LeftSide { get { return Children[0]; } set { Children[0] = value; } }

        public ShaderField Field
        {
            get { return (ShaderField)base.Variable; }
            set { base.Variable = value; }
        }
    }

    public class ArrayAccessExpressionAST : ExpressionAST
    {
        public ExpressionAST LeftSide { get { return Children[0]; } set { Children[0] = value; } }

        public ExpressionAST Index { get { return Children[1]; } set { Children[1] = value; } }

        public override int PreferedNumberOfChildren
        {
            get
            {
                return 2;
            }
        }
    }

    public class CastExpressionAST : UnaryExpressionAST
    {
        public ShaderType Target { get; internal set; }
    }

    #endregion

    public interface IStatementAST
    {
    }

    #region Statements

    public abstract class StatementAST : ShaderNodeAST, IStatementAST
    {
    }

    public class BlockStatementAST : StatementAST
    {
        public BlockStatementAST() { Statements = new List<IStatementAST>(); }

        public List<IStatementAST> Statements { get; set; }
    }

    /// <summary>
    /// if (condition) OnTrue else OnFalse
    /// </summary>
    public class ConditionalStatementAST : StatementAST
    {
        public ExpressionAST Condition { get; set; }

        public IStatementAST OnTrue { get; set; }

        public IStatementAST OnFalse { get; set; }
    }

    /// <summary>
    /// [Variable] = [Expression]
    /// </summary>
    public class AssignamentStatementAST : StatementAST
    {
        public VariableExpressionAST Identifier { get; set; }

        public ExpressionAST Value { get; set; }
    }

    public class ReturnStatementAST : StatementAST
    {
        public ExpressionAST Value { get; set; }
    }

    public class WhileStatementAST : StatementAST
    {
        public ExpressionAST Condition { get; set; }

        public IStatementAST Body { get; set; }
    }

    public class DoWhileStatementAST : StatementAST
    {
        public ExpressionAST Condition { get; set; }

        public IStatementAST Body { get; set; }
    }


    #endregion
}

