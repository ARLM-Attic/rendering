using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System.Compilers.ILAST
{
    public class ILAstNode
    {
        public int ILAddress { get; internal set; }

        public override bool Equals(object obj)
        {
            return
                obj.GetType() == this.GetType() &&
                this.ILAddress == ((ILAstNode)obj).ILAddress &&
                typeof(IEquatable<>).MakeGenericType(this.GetType()).IsAssignableFrom (this.GetType()) &&
                (bool)this.GetType().GetMethod("Equals", new Type[] { this.GetType() }).Invoke(this, new object[] { obj });
        }

        public override int GetHashCode()
        {
            return ILAddress;
        }
    }

    public abstract class ILAstExpression : ILAstNode
    {
        public abstract Type StaticType { get; }
    }

    public class ILAstConstantExpression : ILAstExpression, IEquatable<ILAstConstantExpression>
    {
        public object Value { get; internal set; }

        public override Type StaticType
        {
            get { return Value == null ? null : Value.GetType(); }
        }

        public bool Equals(ILAstConstantExpression other)
        {
            return object.Equals(this.Value, other.Value);
        }

        public override string ToString()
        {
            return Value == null ? "null" : ((Value is string) ? "\"" + Value + "\"" : Value.ToString());
        }
    }

    public class ILAstArgumentExpression : ILAstExpression, IEquatable<ILAstArgumentExpression>
    {
        public override Type StaticType
        {
            get { return ParameterInfo.ParameterType; }
        }

        internal ParameterInfo ParameterInfo { get; set; }

        public bool Equals(ILAstArgumentExpression other)
        {
            return object.Equals(this.ParameterInfo, other.ParameterInfo);
        }

        public override string ToString()
        {
            return "param" + ParameterInfo.Position;
        }
    }

    public class ILAstFieldExpression : ILAstExpression, IEquatable<ILAstFieldExpression>
    {
        public override Type StaticType
        {
            get { return FieldInfo.FieldType; }
        }

        public FieldInfo FieldInfo
        { get; internal set; }

        public ILAstExpression LeftSide {get;internal set;}

        public bool Equals(ILAstFieldExpression other)
        {
            return object.Equals(this.LeftSide, other.LeftSide) && object.Equals(this.FieldInfo, other.FieldInfo);
        }

        public override string ToString()
        {
            return LeftSide.ToString() + "." + FieldInfo.Name;
        }
    }

    public class ILAstLocalExpression : ILAstExpression, IEquatable<ILAstLocalExpression>
    {
        public override Type StaticType
        {
            get { return LocalInfo.LocalType; }
        }

        internal LocalVariableInfo LocalInfo
        {
            get;
            set;
        }

        public bool Equals(ILAstLocalExpression other)
        {
            return object.Equals(this.LocalInfo, other.LocalInfo);
        }

        public override string ToString()
        {
            return "local" + LocalInfo.LocalIndex;
        }
    }

    public class ILAstThisExpression : ILAstExpression, IEquatable<ILAstThisExpression>
    {
        internal Type DeclaringType { get; set; }

        public override Type StaticType
        {
            get { return DeclaringType; }
        }

        public bool Equals(ILAstThisExpression other)
        {
            return object.Equals(this.DeclaringType, other.DeclaringType);
        }

        public override string ToString()
        {
            return "this";
        }
    }

    public class ILAstBinaryOperatorExpression : ILAstExpression, IEquatable<ILAstBinaryOperatorExpression>
    {
        public Operators Operator { get; internal set; }

        public ILAstExpression LeftOperand { get; internal set; }

        public ILAstExpression RightOperand { get; internal set; }

        public override Type StaticType
        {
            get
            {
                switch (Operator)
                {
                    case Operators.Addition:
                    case Operators.Subtraction:
                    case Operators.Modulus:
                    case Operators.Multiply:
                    case Operators.Division:
                    case Operators.LogicAnd:
                    case Operators.LogicOr:
                    case Operators.LogicXor:
                        return LeftOperand.StaticType;
                    case Operators.LessThan:
                    case Operators.LessThanOrEquals:
                    case Operators.GreaterThan:
                    case Operators.GreaterThanOrEquals:
                    case Operators.Equality:
                    case Operators.Inequality:
                        return typeof(bool);
                    default: throw new NotImplementedException("Case " + Operator);
                }
            }
        }

        private string OperatorToken(Operators op)
        {
            switch (op)
            {
                case Operators.Addition: return "+";
                case Operators.Subtraction: return "-";
                case Operators.Multiply: return "*";
                case Operators.Division: return "/";
                case Operators.Modulus: return "%";
                case Operators.LessThan: return "<";
                case Operators.LessThanOrEquals: return "<=";
                case Operators.LogicAnd: return "&";
                case Operators.LogicOr: return "|";
                case Operators.LogicXor: return "^";
                case Operators.GreaterThan: return ">";
                case Operators.GreaterThanOrEquals: return ">=";
                case Operators.Equality: return "==";
                case Operators.Inequality: return "!=";
                case Operators.ConditionalAnd: return "&&";
                case Operators.ConditionalOr: return "||";
            }
            return "";
        }
        public override string ToString()
        {
            return "(" + LeftOperand.ToString() + OperatorToken(Operator) + RightOperand.ToString() + ")";
        }

        public bool Equals(ILAstBinaryOperatorExpression other)
        {
            return object.Equals(this.Operator, other.Operator) && 
                object.Equals(this.LeftOperand, other.LeftOperand) &&
                object.Equals(this.RightOperand, other.RightOperand);
        }
    }

    public class ILAstTernaryOperatorExpressionv : ILAstExpression, IEquatable<ILAstTernaryOperatorExpressionv>
    {
        public Operators Operator { get { return Operators.TernaryDecision; } }

        public ILAstExpression Conditional { get; internal set; }

        public ILAstExpression WhenTrue { get; internal set; }

        public ILAstExpression WhenFalse { get; internal set; }

        public override Type StaticType
        {
            get { return WhenTrue.StaticType; }
        }

        public bool Equals(ILAstTernaryOperatorExpressionv other)
        {
            return object.Equals(this.Conditional, other.Conditional) &&
                object.Equals(this.WhenTrue, other.WhenTrue) &&
                object.Equals(this.WhenFalse, other.WhenFalse);
        }

        public override string ToString()
        {
            return "(" + Conditional.ToString() + ")?" + WhenTrue.ToString() + ":" + WhenFalse + ")";
        }
    }

    public class ILAstUnaryOperatorExpression : ILAstExpression, IEquatable<ILAstUnaryOperatorExpression>
    {
        public Operators Operator { get; internal set; }

        public ILAstExpression Operand { get; internal set; }

        public override Type StaticType
        {
            get { return Operand.StaticType; }
        }

        public bool Equals(ILAstUnaryOperatorExpression other)
        {
            return object.Equals(this.Operator, other.Operator) &&
                object.Equals(this.Operand, other.Operand);
        }

        private string OperatorToken(Operators op)
        {
            switch (op)
            {
                case Operators.Not: return "!";
                case Operators.UnaryNegation: return "-";
                case Operators.UnaryPlus: return "+";
            }

            return "";
        }
        public override string ToString()
        {
            return "(" + OperatorToken(Operator) + Operand.ToString() + ")";
        }
    }

    public class ILAstConvertOperatorExpression : ILAstUnaryOperatorExpression, IEquatable<ILAstConvertOperatorExpression>
    {
        public override Type StaticType
        {
            get
            {
                return TargetType;
            }
        }

        public Type TargetType { get; internal set; }

        public bool Equals(ILAstConvertOperatorExpression other)
        {
            return object.Equals(this.TargetType, other.TargetType);
        }

        public override string ToString()
        {
            return "((" + TargetType.Name + ")" + Operand.ToString() + ")";
        }
    }

    public abstract class ILAstMethodBaseCallExpression : ILAstExpression, IEquatable<ILAstMethodBaseCallExpression>
    {
        public MethodBase MethodBase { get; internal set; }

        public ILAstExpression[] Arguments { get; internal set; }

        public bool Equals(ILAstMethodBaseCallExpression other)
        {
            return object.Equals(this.MethodBase, other.MethodBase) &&
                this.Arguments.SequenceEqual(other.Arguments);
        }
    }

    public class ILAstMethodCallExpression : ILAstMethodBaseCallExpression, IEquatable<ILAstMethodCallExpression>
    {
        public MethodInfo MethodInfo { get { return base.MethodBase as MethodInfo; } }

        public override Type StaticType
        {
            get { return MethodInfo.ReturnType; }
        }

        public bool Equals(ILAstMethodCallExpression other)
        {
            return base.Equals(other);
        }

        public override string ToString()
        {
            return MethodInfo.Name + "(" + string.Join(",", Arguments.Select(a => a.ToString()).ToArray()) + ")";
        }
    }

    public class ILAstConstructorCallExpression : ILAstMethodBaseCallExpression, IEquatable<ILAstConstructorCallExpression>
    {
        public ConstructorInfo ConstructorInfo { get { return base.MethodBase as ConstructorInfo; } }

        public override Type StaticType
        {
            get { return ConstructorInfo.DeclaringType; }
        }

        public bool Equals(ILAstConstructorCallExpression other)
        {
            return base.Equals(other);
        }

        public override string ToString()
        {
            return "new " + ConstructorInfo.DeclaringType.Name + "(" + string.Join(",", Arguments.Select(a => a.ToString()).ToArray()) + ")";
        }
    }

    public class ILAstInitObjectExpression : ILAstExpression, IEquatable<ILAstInitObjectExpression>
    {
        public Type TargetType { get; internal set; }

        public override Type StaticType
        {
            get { return TargetType; }
        }

        public bool Equals(ILAstInitObjectExpression other)
        {
            return object.Equals(this.TargetType, other.TargetType);
        }

        public override string ToString()
        {
            return "new " + TargetType.Name + "()";
        }
    }

    public abstract class ILAstAssignament : ILAstNode
    {
        public ILAstExpression Value { get; internal set; }
    }

    public class ILAstFieldAssignament : ILAstAssignament, IEquatable<ILAstFieldAssignament>
    {
        public ILAstExpression LeftSide { get; internal set; }

        public FieldInfo FieldInfo { get; internal set; }

        public bool Equals(ILAstFieldAssignament other)
        {
            return object.Equals(this.FieldInfo, other.FieldInfo) &&
                object.Equals(this.LeftSide, other.LeftSide);
        }

        public override string ToString()
        {
            return LeftSide.ToString() + "." + FieldInfo.Name + "=" + Value.ToString();
        }
    }

    public class ILAstLocalAssignament : ILAstAssignament, IEquatable<ILAstLocalAssignament>
    {
        public LocalVariableInfo LocalInfo { get; internal set; }

        public bool Equals(ILAstLocalAssignament other)
        {
            return object.Equals(this.LocalInfo, other.LocalInfo);
        }

        public override string ToString()
        {
            return "local" + LocalInfo.LocalIndex + " = " + Value.ToString();
        }
    }

    public class ILAstArgumentAssignament : ILAstAssignament, IEquatable<ILAstArgumentAssignament>
    {
        public ParameterInfo ParameterInfo { get; internal set; }

        public bool Equals(ILAstArgumentAssignament other)
        {
            return object.Equals(this.ParameterInfo, other.ParameterInfo);
        }

        public override string ToString()
        {
            return "param" + ParameterInfo.Position + " = " + Value.ToString();
        }
    }

    public class ILAstGoto : ILAstNode, IEquatable<ILAstGoto>
    {
        public int GotoAddress { get; internal set; }

        public bool Equals(ILAstGoto other)
        {
            return this.GotoAddress == other.GotoAddress;
        }

        public override string ToString()
        {
            return "goto " + "IL_" + GotoAddress.ToString("X");
        }
    }

    public class ILAstConditionalGoto : ILAstGoto, IEquatable<ILAstConditionalGoto>
    {
        public ILAstExpression Condition { get; internal set; }

        public bool Equals(ILAstConditionalGoto other)
        {
            return base.Equals(other) &&
                object.Equals(this.Condition, other.Condition);
        }

        public override string ToString()
        {
            return "if (" + Condition.ToString() + ") goto IL_" + GotoAddress.ToString("X");
        }
    }

    public class ILAstReturn : ILAstNode, IEquatable<ILAstReturn>
    {
        public ILAstExpression Expression { get; internal set; }

        public bool Equals(ILAstReturn other)
        {
            return object.Equals(this.Expression, other.Expression);
        }

        public override string ToString()
        {
            return "return " + (Expression != null ? Expression.ToString() : "");
        }
    }

    
}
