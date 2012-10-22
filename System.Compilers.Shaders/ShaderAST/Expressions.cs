using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.Info;

namespace System.Compilers.Shaders.ShaderAST
{
    public abstract class ShaderExpressionAST : ShaderNodeAST
    {
        public abstract ShaderType Type { get; }

        public abstract bool IsLeftValue { get; }

        internal ShaderExpressionAST(ShaderProgramAST program)
            : base(program)
        {
        }
    }

    public class ShaderConstantExpressionAST : ShaderExpressionAST
    {
        internal ShaderConstantExpressionAST(ShaderProgramAST program, ShaderType type, object value):base(program)
        {
            this.Value = value;
            this._Type = type;
        }

        public object Value { get; private set; }

        ShaderType _Type;

        public override ShaderType Type
        {
            get { return _Type; }
        }

        public override bool IsLeftValue
        {
            get { return false; }
        }
    }

    public abstract class ShaderMemberInvokeAST : ShaderExpressionAST 
    {
        internal ShaderMemberInvokeAST(ShaderProgramAST program, ShaderMember member, ShaderExpressionAST leftSide):base(program)
        {
            this.Member = member;
            this.LeftSide = leftSide;
        }

        public ShaderMember Member { get; private set; }

        public ShaderExpressionAST LeftSide { get; private set; }
    }

    public class ShaderFieldInvokeAST : ShaderMemberInvokeAST
    {
        internal ShaderFieldInvokeAST(ShaderProgramAST program, ShaderField field, ShaderExpressionAST leftSide) : base (program, field, leftSide)
        {
        }

        public ShaderField Field { get { return base.Member as ShaderField; } }

        public override ShaderType Type
        {
            get { return Field.Type; }
        }

        public override bool IsLeftValue
        {
            get { return true; }
        }
    }

    public class ShaderParameterInvokeAST : ShaderExpressionAST
    {
        internal ShaderParameterInvokeAST(ShaderProgramAST program, ShaderParameter parameter) :base(program)
        {
            this.Parameter = parameter;
        }

        public ShaderParameter Parameter { get; private set; }

        public override ShaderType Type
        {
            get { return Parameter.ParameterType; }
        }

        public override bool IsLeftValue
        {
            get { return true; }
        }
    }

    public class ShaderLocalInvokeAST : ShaderExpressionAST
    {
        internal ShaderLocalInvokeAST(ShaderProgramAST program, ShaderLocal local):base(program)
        {
            this.Local = local;
        }

        public ShaderLocal Local { get; private set; }

        public override ShaderType Type
        {
            get { return Local.Type; }
        }

        public override bool IsLeftValue
        {
            get { return true; }
        }
    }

    public class ShaderOperationAST : ShaderExpressionAST
    {
        internal ShaderOperationAST(ShaderProgramAST program, Operators op, params ShaderExpressionAST[] operands)
            : base(program)
        {
            if (op == Operators.Cast)
                throw new InvalidOperationException("Creates Cast with another constructor overload");

            this.operands = operands.Clone() as ShaderExpressionAST[];
            this.Operator = op;

            if (Operator == Operators.Indexer)
            {
                this._Type = operands[0].Type.ElementType;
            }
            else
            {
                this._Type = Program.Builtins.GetBestOverload(Operator, Operands.Select(o => o.Type).ToArray()).ReturnType;
            }
        }

        internal ShaderOperationAST(ShaderProgramAST program, ShaderType castType, ShaderExpressionAST expression):base(program)
        {
            this.operands = new ShaderExpressionAST[] { expression };
            this.Operator = Operators.Cast;
            this._Type = castType;
        }

        public Operators Operator { get; private set; }

        ShaderExpressionAST[] operands;

        public ShaderExpressionAST[] Operands { get { return operands.Clone() as ShaderExpressionAST[]; } }

        public override bool IsLeftValue
        {
            get { return false; }
        }

        ShaderType _Type;

        public override ShaderType Type
        {
            get { return _Type; }
        }
    }

    public class ShaderConversionAST : ShaderOperationAST
    {
        internal ShaderConversionAST(ShaderProgramAST program, ShaderType target, ShaderExpressionAST expression)
            : base(program, target, expression)
        {
            if (!(expression.Type == target || expression.Type.IsSubclass(target) || target.IsSubclass(expression.Type) ||
                program.Builtins.GetConversion(expression.Type, target) != null))
                throw new ArgumentException("Can not convert from " + expression.Type.Name + " to " + target.Name);
            this.Target = target;
        }

        public ShaderType Target { get; private set; }

        public ShaderExpressionAST Expression { get { return Operands[0]; } }

        public override bool IsLeftValue
        {
            get { return false; }
        }

        public override ShaderType Type
        {
            get { return Target; }
        }
    }

    public abstract class ShaderMethodBaseInvokeAST : ShaderMemberInvokeAST
    {
        ShaderExpressionAST[] _Arguments;
        public ShaderExpressionAST[] Arguments
        {
            get
            {
                return _Arguments.Clone() as ShaderExpressionAST[];
            }
        }

        internal ShaderMethodBaseInvokeAST(ShaderProgramAST program, ShaderMethodBase methodBase, ShaderExpressionAST leftSide, ShaderExpressionAST[] arguments)
            : base(program, methodBase, leftSide)
        {
            this._Arguments = arguments.Clone() as ShaderExpressionAST[];
        }

        public override bool IsLeftValue
        {
            get { return false; }
        }
    }

    public class ShaderMethodInvokeAST : ShaderMethodBaseInvokeAST
    {
        internal ShaderMethodInvokeAST(ShaderProgramAST program, ShaderMethod method, ShaderExpressionAST leftSide, ShaderExpressionAST[] arguments)
            : base(program, method, leftSide, arguments)
        {
        }

        public ShaderMethod Method { get { return base.Member as ShaderMethod; } }

        public override ShaderType Type
        {
            get { return Method.ReturnType; }
        }
    }

    public class ShaderConstructorInvokeAST : ShaderMethodBaseInvokeAST
    {
        internal ShaderConstructorInvokeAST(ShaderProgramAST program, ShaderConstructor constructor, ShaderExpressionAST[] arguments)
            : base (program, constructor, null, arguments)
        {
        }

        public ShaderConstructor Constructor { get { return base.Member as ShaderConstructor; } }

        public override ShaderType Type
        {
            get { return Constructor.DeclaringType; }
        }
    }

    public class ShaderInitializationInvokeAST : ShaderExpressionAST
    {
        internal ShaderInitializationInvokeAST(ShaderProgramAST program, ShaderType targetType)
            : base(program)
        {
            this.TargetType = targetType;
        }

        public ShaderType TargetType { get; private set; }
        
        public override ShaderType Type
        {
            get { return TargetType; }
        }

        public override bool IsLeftValue
        {
            get { return false; }
        }
    }

    public class ShaderAssignamentAST : ShaderExpressionAST
    {
        internal ShaderAssignamentAST(ShaderExpressionAST leftValue, ShaderExpressionAST rightValue)
            : base(leftValue.Program)
        {
            if (!leftValue.IsLeftValue)
                throw new ArgumentException("Cannot be used that expression as left value.");

            if (leftValue.Program != rightValue.Program)
                throw new ArgumentException("Different programs.");

            LeftValue = leftValue;
            RightValue = rightValue;
        }

        public ShaderExpressionAST LeftValue { get; private set; }

        public ShaderExpressionAST RightValue { get; private set; }

        public override bool IsLeftValue
        {
            get { return false; }
        }

        public override ShaderType Type
        {
            get { return RightValue.Type; }
        }
    }

    public class ShaderMethodBodyAST : ShaderNodeAST
    {
        List<ShaderStatementAST> statements = new List<ShaderStatementAST>();

        internal IList<ShaderStatementAST> StatementsList { get { return statements; } }

        internal ShaderMethodBodyAST(ShaderMethodBaseDeclarationAST methodDeclaration):base(methodDeclaration.Program)
        {
            MethodBase = methodDeclaration;
        }

        public ShaderMethodBaseDeclarationAST MethodBase { get; private set; }

        public IEnumerable<ShaderParameter> Parameters { get { return MethodBase.Parameters; } }

        public IEnumerable<ShaderStatementAST> Statements { get { return statements; } }
    }

    public class ShaderLocal
    {
        internal ShaderLocal(ShaderLocalDeclarationAST declaration)
        {
            this._declaration = declaration;
        }

        ShaderLocalDeclarationAST _declaration;

        public ShaderType Type { get { return _declaration.Type; } }

        public string Name { get { return _declaration.Name; } }
    }
}
