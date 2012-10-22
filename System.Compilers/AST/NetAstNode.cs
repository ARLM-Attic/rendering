using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;
using System.IO;

namespace System.Compilers.AST
{
    public abstract class NetAstNode
    {
        public NetAstNode()
        {
            if (!this.GetType().IsAbstract && this.GetType().GetMethod("Equals", new Type[] { this.GetType() }) == null)
                throw new NotImplementedException("This class is missing from implementing Equals with a parameter of same type");
        }

        internal int ILAddress { get; set; }

        public override bool Equals(object obj)
        {
            return
                obj.GetType() == this.GetType() &&
                this.ILAddress == ((NetAstNode)obj).ILAddress &&
                (bool)this.GetType().GetMethod("Equals", new Type[] { this.GetType() }).Invoke(this, new object[] { obj });
        }

        public IEnumerable<T> GetSelfAndChildrenRecursive<T>() where T : NetAstNode
        {
            List<T> result = new List<T>(16);
            AccumulateSelfAndChildrenRecursive(result);
            return result;
        }

        void AccumulateSelfAndChildrenRecursive<T>(List<T> list) where T : NetAstNode
        {
            if (this is T)
                list.Add((T)this);
            foreach (NetAstNode node in this.GetChildren())
            {
                if (node != null)
                    node.AccumulateSelfAndChildrenRecursive(list);
            }
        }

        public virtual IEnumerable<NetAstNode> GetChildren()
        {
            return GetOperands();
        }

        public override int GetHashCode()
        {
            return ILAddress;
        }

        public virtual NetAstExpression[] GetOperands()
        {
            return new NetAstExpression[] { };
        }
        
        public virtual void SetOperandAt(int index, NetAstExpression operand)
        {
            // OVERRIDEN IF NEEDED
        }

        static System.Compilers.Generators.CSharp.CSharpCodeGenerator codeGenerator = new Generators.CSharp.CSharpCodeGenerator();

        public override string ToString()
        {
            try
            {
                return codeGenerator.GetCode(this);
            }
            catch
            {
                return base.ToString();
            }
        }
    }

    public class NetAstStatement : NetAstNode
    {
    }
   
    public class NetAstLocalDeclaration : NetAstStatement, IEquatable<NetAstLocalDeclaration>
    {
        public NetLocalVariable LocalInfo { get; internal set; }
        
        public bool Equals(NetAstLocalDeclaration other)
        {
            return object.Equals (other.LocalInfo, this.LocalInfo);
        }
    }

    public abstract class NetAstExpression : NetAstNode
    {
        public NetAstExpression()
        {
            ParentPrecedence = -1;
        }
        public abstract Type StaticType { get; }

        protected bool IsNormalized { get; set; }

        protected int Precedence { get; set; }

        protected int ParentPrecedence { get; set; }

        public virtual void Normalize()
        {
            IsNormalized = true;
        }

        public virtual void SetPrecedences(int parentPrecedence)
        {
            if (ParentPrecedence < 0)
                ParentPrecedence = parentPrecedence;
        }

        protected int GetOperatorPrecedence(Operators op)
        {
            switch (op)
            {
                case Operators.ConditionalOr:
                    return 1;
                case Operators.ConditionalAnd: 
                    return 2;
                case Operators.LogicOr: 
                    return 3;
                case Operators.LogicXor: 
                    return 4;
                case Operators.LogicAnd: 
                    return 5;
                
                case Operators.Equality: 
                case Operators.Inequality: 
                    return 6;
                case Operators.LessThan:
                case Operators.LessThanOrEquals:
                case Operators.GreaterThan:
                case Operators.GreaterThanOrEquals: 
                    return 7;
                case Operators.Addition:
                case Operators.Subtraction: 
                    return 8;
                case Operators.Multiply:
                case Operators.Division:
                case Operators.Modulus:
                    return 9;
                case Operators.Not:
                case Operators.UnaryPlus:
                case Operators.UnaryNegation:
                case Operators.PreIncrement:
                case Operators.PreDecrement:
                    return 10;
                case Operators.PostIncrement: return 11;
                case Operators.PostDecrement: return 11;
                default:
                    return 0;
            }
        }

        public abstract bool CanBeEvaluated { get; }

        protected void CheckIfCanBeEvaluated()
        {
            if (!CanBeEvaluated)
                throw new InvalidOperationException("Can not evaluate this expression");
        }

        public virtual object GetValue(object target) { throw new NotSupportedException(); }
    }

    public class NetAstExpressionStatement : NetAstStatement, IEquatable<NetAstExpressionStatement>
    {
        public NetAstExpression Expression { get; set; }

        public bool Equals(NetAstExpressionStatement other)
        {
            return this.Expression.Equals(other.Expression);
        }

        public override IEnumerable<NetAstNode> GetChildren()
        {
            yield return Expression;
        }

        public override string ToString()
        {
            return Expression.ToString() + ";";
        }
    }

    public class NetAstConstantExpression : NetAstExpression, IEquatable<NetAstConstantExpression>
    {
        public NetAstConstantExpression(object value)
        {
            this._Value = value;
        }

        object _Value;
        public override object GetValue (object target) { return _Value; }

        public object Value { get { return _Value; } }

        public override Type StaticType
        {
            get { return _Value == null ? null : _Value.GetType(); }
        }

        public bool Equals(NetAstConstantExpression other)
        {
            return object.Equals(this._Value, other._Value);
        }

        public override string ToString()
        {
            return _Value == null ? "null" : ((_Value is string) ? "\"" + _Value + "\"" : _Value.ToString().ToLower());
        }

        public override bool CanBeEvaluated
        {
            get { return true; }
        }
    }

    public class NetAstArgumentExpression : NetAstExpression, IEquatable<NetAstArgumentExpression>
    {
        public override Type StaticType
        {
            get { return ParameterInfo.ParameterType; }
        }

        public ParameterInfo ParameterInfo { get; set; }

        public bool Equals(NetAstArgumentExpression other)
        {
            return object.Equals(this.ParameterInfo, other.ParameterInfo);
        }

        public override string ToString()
        {
            return "param" + ParameterInfo.Position;
        }

        public override bool CanBeEvaluated
        {
            get { return false; }
        }
    }

    public abstract class NetAstInvokeExpression : NetAstExpression
    {
        public MemberInfo Member { get; internal set; }

        NetAstExpression leftSide;
        public virtual NetAstExpression LeftSide { get { return leftSide; } internal set { leftSide = value; } }
    }

    public class NetAstFieldExpression : NetAstInvokeExpression, IEquatable<NetAstFieldExpression>
    {
        public override Type StaticType
        {
            get { return FieldInfo.FieldType; }
        }

        public FieldInfo FieldInfo
        {
            get { return Member as FieldInfo; }
        }

        public bool Equals(NetAstFieldExpression other)
        {
            return object.Equals(this.LeftSide, other.LeftSide) && object.Equals(this.FieldInfo, other.FieldInfo);
        }

        public override string ToString()
        {
            return (LeftSide != null ? LeftSide.ToString() + "." : FieldInfo.DeclaringType + ".") + FieldInfo.Name;
        }

        public override IEnumerable<NetAstNode> GetChildren()
        {
            if (LeftSide != null)
                yield return LeftSide;
        }

        public override NetAstExpression[] GetOperands()
        {
            return new NetAstExpression[] { LeftSide };
        }

        public override void SetOperandAt(int index, NetAstExpression operand)
        {
            if (index == 0)
                LeftSide = operand;
            else
                throw new IndexOutOfRangeException();
        }

        public override bool CanBeEvaluated
        {
            get { return LeftSide == null /*static field*/ || LeftSide.CanBeEvaluated; }
        }

        public override object GetValue(object target)
        {
            if (FieldInfo.IsStatic)
                return FieldInfo.GetValue(null);
            return FieldInfo.GetValue(LeftSide == null ? target : LeftSide.GetValue(target));
        }
    }

    public static class ReflectionExtensors
    {
        public static object GetValue (this FieldInfo field, object target)
        {
            return null;
        }
    }

    public class NetAstPropertyExpression : NetAstInvokeExpression, IEquatable<NetAstPropertyExpression>
    {
        public override Type StaticType
        {
            get { return PropertyInfo.PropertyType; }
        }

        public PropertyInfo PropertyInfo
        { get { return Member as PropertyInfo; } }

        public bool Equals(NetAstPropertyExpression other)
        {
            return object.Equals(this.LeftSide, other.LeftSide) && object.Equals(this.PropertyInfo, other.PropertyInfo);
        }

        public override string ToString()
        {
            return LeftSide.ToString() + "." + PropertyInfo.Name;
        }

        public override bool CanBeEvaluated
        {
            get { return LeftSide == null /*static property*/ || LeftSide.CanBeEvaluated; }
        }

        public override object GetValue(object target)
        {
            return PropertyInfo.GetValue(LeftSide == null ? null : LeftSide.GetValue(target), null);
        }
    }

    public class NetAstLocalExpression : NetAstExpression, IEquatable<NetAstLocalExpression>
    {
        public override Type StaticType
        {
            get { return LocalInfo.Type; }
        }

        public NetLocalVariable LocalInfo
        {
            get;
            set;
        }

        public bool Equals(NetAstLocalExpression other)
        {
            return object.Equals(this.LocalInfo, other.LocalInfo);
        }

        public override string ToString()
        {
            return LocalInfo.Name;
        }

        public override bool CanBeEvaluated
        {
            get { return false; }
        }
    }

    public class NetAstThisExpression : NetAstExpression, IEquatable<NetAstThisExpression>
    {
        public NetAstThisExpression()
        {
        }

        internal Type DeclaringType { get; set; }

        public override Type StaticType
        {
            get { return DeclaringType; }
        }

        public bool Equals(NetAstThisExpression other)
        {
            return object.Equals(this.DeclaringType, other.DeclaringType);
        }

        public override string ToString()
        {
            return "this";
        }

        public override bool CanBeEvaluated
        {
            get { return true; }
        }

        public override object GetValue(object target)
        {
            return target;
        }
    }

    public class NetAstBinaryOperatorExpression : NetAstExpression, IEquatable<NetAstBinaryOperatorExpression>
    {
        public Operators Operator { get; internal set; }

        public NetAstExpression LeftOperand { get; internal set; }

        public NetAstExpression RightOperand { get; internal set; }

        public Type ReturnType
        {
            get;
            internal set;
        }

        public override Type StaticType
        {
            get
            {
                if (ReturnType != null)
                    return ReturnType; // operators...

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
                        return Evaluator.MoreGeneral(LeftOperand.StaticType, RightOperand.StaticType);
                    case Operators.LessThan:
                    case Operators.LessThanOrEquals:
                    case Operators.GreaterThan:
                    case Operators.GreaterThanOrEquals:
                    case Operators.Equality:
                    case Operators.Inequality:
                    case Operators.ConditionalOr:
                    case Operators.ConditionalAnd:
                        return typeof(bool);
                    default: throw new NotImplementedException("Case " + Operator);
                }
            }
        }

        public override string ToString()
        {
            bool addParenthesis = ParentPrecedence > Precedence;
            return (addParenthesis ? "(" : "") + LeftOperand.ToString() + " " + Operator.Token() + " " + RightOperand.ToString() + (addParenthesis ? ")" : "");
        }

        public override void Normalize()
        {
            if (!IsNormalized)
            {
                base.Normalize();

                LeftOperand.Normalize();
                RightOperand.Normalize();

                var unary = LeftOperand as NetAstUnaryOperatorExpression;
                if (unary != null && unary.Operator == Operators.None)
                    LeftOperand = unary.Operand;

                unary = RightOperand as NetAstUnaryOperatorExpression;
                if (unary != null && unary.Operator == Operators.None)
                    RightOperand = unary.Operand;
            }
        }

        public bool Equals(NetAstBinaryOperatorExpression other)
        {
            return object.Equals(this.Operator, other.Operator) &&
                object.Equals(this.LeftOperand, other.LeftOperand) &&
                object.Equals(this.RightOperand, other.RightOperand);
        }

        public override void SetPrecedences(int parentPrecedence)
        {
            base.SetPrecedences(parentPrecedence);
            Precedence = GetOperatorPrecedence(Operator);
            LeftOperand.SetPrecedences(Precedence);
            RightOperand.SetPrecedences(Precedence);
        }

        public override NetAstExpression[] GetOperands()
        {
            return new NetAstExpression[]{LeftOperand, RightOperand};
        }

        public override void SetOperandAt(int index, NetAstExpression operand)
        {
            if (index == 0)
                LeftOperand = operand;
            else if (index == 1)
                RightOperand = operand;
            else
                throw new IndexOutOfRangeException();
        }

        public override bool CanBeEvaluated
        {
            get { return LeftOperand.CanBeEvaluated && RightOperand.CanBeEvaluated; }
        }

        public override object GetValue(object target)
        {
            return Evaluator.Eval(this.Operator, LeftOperand.GetValue(target), RightOperand.GetValue(target));
        }
    }

    public class NetAstTernaryOperatorExpressionv : NetAstExpression, IEquatable<NetAstTernaryOperatorExpressionv>
    {
        public Operators Operator { get { return Operators.TernaryDecision; } }

        public NetAstExpression Conditional { get; internal set; }

        public NetAstExpression WhenTrue { get; internal set; }

        public NetAstExpression WhenFalse { get; internal set; }

        public override Type StaticType
        {
            get { return WhenTrue.StaticType; }
        }

        public bool Equals(NetAstTernaryOperatorExpressionv other)
        {
            return object.Equals(this.Conditional, other.Conditional) &&
                object.Equals(this.WhenTrue, other.WhenTrue) &&
                object.Equals(this.WhenFalse, other.WhenFalse);
        }

        public override string ToString()
        {
            return "((" + Conditional.ToString() + ")?" + WhenTrue.ToString() + ":" + WhenFalse + ")";
        }

        public override NetAstExpression[] GetOperands()
        {
            return new NetAstExpression[] { Conditional, WhenTrue, WhenFalse };
        }

        public override void SetOperandAt(int index, NetAstExpression operand)
        {
            if (index == 0)
                Conditional = operand;
            else if (index == 1)
                WhenTrue = operand;
            else if (index == 2)
                WhenFalse = operand;
            else
                throw new IndexOutOfRangeException();
        }

        public override bool CanBeEvaluated
        {
            get { return Conditional.CanBeEvaluated && WhenTrue.CanBeEvaluated && WhenFalse.CanBeEvaluated; }
        }

        public override object GetValue(object target)
        {
            bool condition = (bool)Conditional.GetValue(target);
            return condition ? WhenTrue.GetValue(target) : WhenFalse.GetValue(target);
        }
    }

    public class NetAstUnaryOperatorExpression : NetAstExpression, IEquatable<NetAstUnaryOperatorExpression>
    {
        public Operators Operator { get; internal set; }

        public NetAstExpression Operand { get; internal set; }

        public override Type StaticType
        {
            get
            {
                switch (Operator)
                {
                    case Operators.Not:
                    case Operators.UnaryNegation:
                    case Operators.UnaryPlus:
                        return Operand.StaticType;
                    default:
                        throw new InvalidOperationException("Cast and implicit conversions should be treated as Conversion AST ");
                }
            }
        }

        public bool Equals(NetAstUnaryOperatorExpression other)
        {
            return object.Equals(this.Operator, other.Operator) &&
                object.Equals(this.Operand, other.Operand);
        }

        public override void Normalize()
        {
            if (!IsNormalized)
            {
                Operand.Normalize();

                var unary = Operand as NetAstUnaryOperatorExpression;
                if (unary != null && unary.Operator == Operators.None)
                    Operand = unary.Operand;
                
                if (Operator == Operators.Not)
                    NormalizeNot();
                
                
                base.Normalize();
            }
        }

        private void NormalizeNot()
        {
            var unary = Operand as NetAstUnaryOperatorExpression;
            if (unary != null)
            {
                if (unary.Operator == Operators.Not)
                {
                    Operand = unary.Operand;
                    Operator = Operators.None;
                }
                return;
            }

            var binary = Operand as NetAstBinaryOperatorExpression;
            if (binary != null)
            {
                Operators newOp;
                switch (binary.Operator)
                {
                    case Operators.Equality:
                        newOp = Operators.Inequality;
                        break;
                    case Operators.Inequality:
                        newOp = Operators.Equality;
                        break;
                    case Operators.LessThan:
                        newOp = Operators.GreaterThanOrEquals;
                        break;
                    case Operators.LessThanOrEquals:
                        newOp = Operators.GreaterThan;
                        break;
                    case Operators.GreaterThan:
                        newOp = Operators.LessThanOrEquals;
                        break;
                    case Operators.GreaterThanOrEquals:
                        newOp = Operators.LessThan;
                        break;
                    case Operators.ConditionalOr:
                        newOp = Operators.ConditionalAnd;
                        break;
                    case Operators.ConditionalAnd:
                        newOp = Operators.ConditionalOr;
                        break;
                    default:
                        newOp = binary.Operator;
                        break;
                }

                if (newOp != binary.Operator)
                {
                    binary.Operator = newOp;
                    Operator = Operators.None;
                }
            }
        }

        public override void SetPrecedences(int parentPrecedence)
        {
            base.SetPrecedences(parentPrecedence);
            Precedence = GetOperatorPrecedence(Operator);
            Operand.SetPrecedences(Precedence);
        }

        public override string ToString()
        {
            bool addParenthesis = ParentPrecedence > Precedence;
            return (addParenthesis ? "(" : "") + Operator.Token() + Operand.ToString() + (addParenthesis ? "(" : "");
        }

        public override NetAstExpression[] GetOperands()
        {
            return new NetAstExpression[] { Operand };
        }

        public override void SetOperandAt(int index, NetAstExpression operand)
        {
            if (index == 0)
                Operand = operand;
            else throw new IndexOutOfRangeException();
        }

        public override bool CanBeEvaluated
        {
            get { return Operand.CanBeEvaluated; }
        }

        public override object GetValue(object target)
        {
            return Evaluator.Eval(Operator, Operand.GetValue(target));
        }
    }

    public class NetAstConvertOperatorExpression : NetAstUnaryOperatorExpression, IEquatable<NetAstConvertOperatorExpression>
    {
        public override Type StaticType
        {
            get
            {
                return TargetType;
            }
        }

        public NetAstConvertOperatorExpression()
            : base()
        {
            this.Operator = Operators.Cast;
        }

        public Type TargetType { get; internal set; }

        public bool Equals(NetAstConvertOperatorExpression other)
        {
            return object.Equals(this.TargetType, other.TargetType);
        }

        public override string ToString()
        {
            return "((" + TargetType.Name + ")" + Operand.ToString() + ")";
        }

        public override object GetValue(object target)
        {
            return Evaluator.Eval(TargetType, Operand.GetValue(target));
        }
    }

    public abstract class NetAstMethodBaseCallExpression : NetAstInvokeExpression, IEquatable<NetAstMethodBaseCallExpression>
    {
        public MethodBase MethodBase { get { return Member as MethodBase; } }

        internal NetAstExpression[] Arguments { get; set; }

        public override NetAstExpression LeftSide
        {
            get
            {
                return MethodBase.IsStatic || MethodBase.IsConstructor ? null : Arguments[0];
            }
            internal set
            {
                if (value != null && (MethodBase.IsStatic || MethodBase.IsConstructor))
                    throw new InvalidOperationException();

                if (MethodBase.IsStatic || MethodBase.IsConstructor)
                    return;
                else
                    Arguments[0] = value;
            }
        }

        public NetAstExpression[] Parameters
        {
            get
            {
                return MethodBase.IsStatic || MethodBase.IsConstructor ? Arguments :
                    Arguments.Skip(1).ToArray();
            }
        }

        public override IEnumerable<NetAstNode> GetChildren()
        {
            if(Arguments != null)
                foreach (var item in Arguments)
                    yield return item;
        }

        public bool Equals(NetAstMethodBaseCallExpression other)
        {
            return object.Equals(this.MethodBase, other.MethodBase) &&
                this.Arguments.SequenceEqual(other.Arguments);
        }

        public override NetAstExpression[] GetOperands()
        {
            var result = new NetAstExpression[Arguments.Length];
            Arguments.CopyTo(result, 0);
            return result;
        }

        public override void SetOperandAt(int index, NetAstExpression operand)
        {
            if (index >= 0 && index < Arguments.Length)
                Arguments[index] = operand;
            else
                throw new IndexOutOfRangeException();
        }

        public override bool CanBeEvaluated
        {
            get { return Arguments.All(a => a.CanBeEvaluated); }
        }

        public override object GetValue(object target)
        {
            object[] parameters = Arguments.Select(a => a.GetValue(target)).ToArray();

            if (MethodBase.IsStatic)
                return MethodBase.Invoke(null, parameters);
            else
            {
                if (MethodBase.IsConstructor)
                    return ((ConstructorInfo)MethodBase).Invoke(parameters);
                else
                    return MethodBase.Invoke(parameters[0], parameters.Skip(1).ToArray());
            }
        }
    }

    public class NetAstMethodCallExpression : NetAstMethodBaseCallExpression, IEquatable<NetAstMethodCallExpression>
    {
        public MethodInfo MethodInfo { get { return base.MethodBase as MethodInfo; } }

        public override Type StaticType
        {
            get { return MethodInfo.ReturnType; }
        }

        public bool Equals(NetAstMethodCallExpression other)
        {
            return base.Equals(other);
        }

        public override string ToString()
        {
            return MethodInfo.Name + "(" + string.Join(",", GetOperands().Select(o => o.ToString()).ToArray()) + ")";
        }
    }

    public class NetAstConstructorCallExpression : NetAstMethodBaseCallExpression, IEquatable<NetAstConstructorCallExpression>
    {
        public ConstructorInfo ConstructorInfo { get { return base.MethodBase as ConstructorInfo; } }

        public override Type StaticType
        {
            get { return ConstructorInfo.DeclaringType; }
        }

        public bool Equals(NetAstConstructorCallExpression other)
        {
            return base.Equals(other);
        }
    }

    public class NetAstInitObjectExpression : NetAstExpression, IEquatable<NetAstInitObjectExpression>
    {
        public Type TargetType { get; internal set; }

        public override Type StaticType
        {
            get { return TargetType; }
        }

        public bool Equals(NetAstInitObjectExpression other)
        {
            return object.Equals(this.TargetType, other.TargetType);
        }

        public override bool CanBeEvaluated
        {
            get { return true; }
        }

        public override object GetValue(object target)
        {
            return Activator.CreateInstance(TargetType);
        }
    }

    public class NetInitMDArrayExpression : NetAstExpression, IEquatable<NetInitMDArrayExpression>
    {
        public NetAstExpression[] ArraySizes { get; set; }

        public int[] ArraySizesAsInt
        {
            get { return ArraySizes.Select(e => (int)((NetAstConstantExpression)e).Value).ToArray(); }
        }

        public Array InitValues { get; set; } 

        public Type ArrayType { get; set; }

        public override Type StaticType
        {
            get { return ArrayType.MakeArrayType(); }
        }

        //public override string ToString()
        //{
        //    string initializations =  InitValues != null ? GetInitializationString() : "";

        //    return "(new " + ArrayType.GetElementType() + "[" + String.Join(",", ArraySizes.Select(e => e.ToString()).ToArray()) + "] " + initializations + ")";
        //}

        private string GetInitializationString()
        {
            List<string> result = new List<string>();

            int totalPositions = InitValues.Length;
            int rank = InitValues.Rank;
            int[] arrayDimensions = new int[rank];

            for (int i = 0; i < rank; i++)
            {
                arrayDimensions[i] = InitValues.GetLength(i);
            }
            
            int[] indices = new int[rank];
            for (int j = 0; j < totalPositions; j++)
            {
                int divisor = totalPositions / arrayDimensions[0];
                int reminder = j;
                for (int k = 0; k < rank; k++)
                {
                    indices[k] = reminder / divisor;
                    reminder = reminder % divisor;

                    if (k < rank - 1)
                        divisor /= arrayDimensions[k + 1];
                }

                result.Add(((NetAstConstantExpression)InitValues.GetValue(indices)).Value.ToString());
            }

            for (int i = 0; i < result.Count; i++)
            {
                int divisor = 1;
                for (int j = 1; j <= rank; j++)
                {
                    divisor *= arrayDimensions[rank - j];
                    if (i % divisor == 0)
                        result[i] = "{ " + result[i];
                    if((i+1)%divisor == 0)
                        result[i] += " }";
                }
            }

            return String.Join(", ", result.ToArray());
        }

        public override IEnumerable<NetAstNode> GetChildren()
        {
            if (this.ArraySizes != null)
                foreach (var item in ArraySizes)
                    yield return item;
        }

        public bool Equals(NetInitMDArrayExpression other)
        {
            return other.ArrayType.Equals(ArrayType) && other.ArraySizes.Equals(ArraySizes);
        }

        public override NetAstExpression[] GetOperands()
        {
            var result = new NetAstExpression[ArraySizes.Length];
            ArraySizes.CopyTo(result, 0);
            return result;
        }

        public override void SetOperandAt(int index, NetAstExpression operand)
        {
            if (index >= 0 && index < ArraySizes.Length)
                ArraySizes[index] = operand;
            else
                throw new IndexOutOfRangeException();
        }

        public override bool CanBeEvaluated
        {
            get { return true; }
        }

        public override object GetValue(object target)
        {
            return InitValues;
        }
    }

    public class NetMDArrayElementAssignament : NetAstAssignamentStatement, IEquatable<NetMDArrayElementAssignament>
    {
        public NetAstExpression ArrayExpression { get; set; }
       
        public NetAstExpression[] ArrayIndices { get; set; }

        public bool Equals(NetMDArrayElementAssignament other)
        {
            return ArrayExpression.Equals(other.ArrayExpression) &&
                ArrayIndices.Equals(ArrayIndices) &&
                Value.Equals(Value);
        }

        public override IEnumerable<NetAstNode> GetChildren()
        {
            if (this.ArrayExpression != null)
                yield return this.ArrayExpression;
            if (this.ArrayIndices != null)
                foreach (var item in ArrayIndices)
                    yield return item;
            if (this.Value != null)
                yield return this.Value;
        }

        public override string ToString()
        {
            return ArrayExpression + "[" + String.Join(", ", ArrayIndices.Select(e => e.ToString()).ToArray()) + "]" + " = " + Value;
        }

        public override NetAstExpression[] GetOperands()
        {
            var result = new NetAstExpression[ArrayIndices.Length + 2];
            result[0] = ArrayExpression;
            result[result.Length - 1] = Value;
            Array.Copy(ArrayIndices,0,result,1,ArrayIndices.Length);
            return result;
        }

        public override void SetOperandAt(int index, NetAstExpression operand)
        {
            if (index == 0)
                ArrayExpression = operand;
            else if (index >= 1 && index <=ArrayIndices.Length)
                ArrayIndices[index-1] = operand;
            else if (index == ArrayIndices.Length+1)
                Value = operand;
            else
                throw new IndexOutOfRangeException();
        }
    }

    public class NetMDArrayAccessExpression : NetAstExpression, IEquatable<NetMDArrayAccessExpression>
    {
        public NetAstExpression ArrayExpression { get; set; }
        public NetAstExpression[] IndicesExpressions { get; set; }

        public override Type StaticType
        {
            get { return ArrayExpression.StaticType.GetElementType(); }
        }

        public bool Equals(NetMDArrayAccessExpression other)
        {
            return ArrayExpression.Equals(other.ArrayExpression) &&
                IndicesExpressions.Equals(other.IndicesExpressions);
        }

        public override string ToString()
        {
            return ArrayExpression + "[" + String.Join(", ", IndicesExpressions.Select(e => e.ToString()).ToArray())+ "]";
        }

        public override IEnumerable<NetAstNode> GetChildren()
        {
            if (this.ArrayExpression != null)
                yield return this.ArrayExpression;
            if (this.IndicesExpressions != null)
                foreach (var item in IndicesExpressions)
                    yield return item;
        }

        public override NetAstExpression[] GetOperands()
        {
            var result = new NetAstExpression[IndicesExpressions.Length + 1];
            result[0] = ArrayExpression;
            Array.Copy(IndicesExpressions, 0, result, 1, IndicesExpressions.Length);
            return result;
        }

        public override void SetOperandAt(int index, NetAstExpression operand)
        {
            if (index == 0)
                ArrayExpression = operand;
            else if (index >= 1 && index <= IndicesExpressions.Length)
                IndicesExpressions[index - 1] = operand;
            else
                throw new IndexOutOfRangeException();
        }

        public override bool CanBeEvaluated
        {
            get { return ArrayExpression.CanBeEvaluated && IndicesExpressions.All(i => i.CanBeEvaluated); }
        }

        public override object GetValue(object target)
        {
            return ((Array)ArrayExpression.GetValue(target)).GetValue(IndicesExpressions.Select(i => (int)i.GetValue(target)).ToArray());
        }
    }

    public class NetInitArrayExpression : NetAstExpression, IEquatable<NetInitArrayExpression>
    {
        public int ArraySizeAsInt
        {
            get { return (int)ArraySize.GetValue(null); }
        }
        
        public NetAstExpression ArraySize { get; set; }

        public Type ArrayType { get; set; }

        public NetAstConstantExpression[] InitValues { get; set; }

        public override Type StaticType
        {
            get { return ArrayType.MakeArrayType(); }
        }

        public override string ToString()
        {
            return "(new " + ArrayType + "[" + ArraySize + "]" + (InitValues!= null ? "{" +  String.Join(", ", InitValues.Select(e => e.ToString()).ToArray()) + "}" : "") + ")";
        }

        public override IEnumerable<NetAstNode> GetChildren()
        {
            if (this.ArraySize != null)
                yield return this.ArraySize;
        }

        public bool Equals(NetInitArrayExpression other)
        {
            return other.ArrayType.Equals(ArrayType) && other.ArraySize.Equals(ArraySize);
        }

        public override NetAstExpression[] GetOperands()
        {
            return new NetAstExpression[]{ ArraySize };
        }

        public override void SetOperandAt(int index, NetAstExpression operand)
        {
            if (index == 0)
                ArraySize = operand;
            else
                throw new IndexOutOfRangeException();
        }

        public override bool CanBeEvaluated
        {
            get { return ArraySize.CanBeEvaluated && (InitValues == null || InitValues.All(v=>v.CanBeEvaluated)); }
        }

        public override object GetValue(object target)
        {
            Array a = Array.CreateInstance(ArrayType, (int)ArraySize.GetValue(target));
            if (InitValues != null)
                InitValues.Select(v => v.GetValue(target)).ToArray().CopyTo(a, 0);

            return a;
        }
    }

    public class NetArrayElementAssignament : NetAstAssignamentStatement, IEquatable<NetArrayElementAssignament>
    {
        public NetAstExpression ArrayExpression { get; set; }
        public NetAstExpression ArrayIndex { get; set; }

        public bool Equals(NetArrayElementAssignament other)
        {
            return ArrayExpression.Equals(other.ArrayExpression) &&
                ArrayIndex.Equals(ArrayIndex) &&
                Value.Equals(Value);
        }

        public override IEnumerable<NetAstNode> GetChildren()
        {
            if (this.ArrayExpression != null)
                yield return this.ArrayExpression;
            if (this.ArrayIndex != null)
                yield return this.ArrayIndex;
            if (this.Value != null)
                yield return this.Value;
        }

        public override string ToString()
        {
            return ArrayExpression + "[" + ArrayIndex + "]" + "=" + Value;
        }

        public override NetAstExpression[] GetOperands()
        {
            return new NetAstExpression[] { ArrayExpression, ArrayIndex, Value };
        }

        public override void SetOperandAt(int index, NetAstExpression operand)
        {
            if (index == 0)
                ArrayExpression = operand;
            else if (index == 1)
                ArrayIndex = operand;
            else if (index == 2)
                Value = operand;
            else
                throw new IndexOutOfRangeException();
        }
    }

    public class NetArrayAccessExpression : NetAstExpression, IEquatable<NetArrayAccessExpression>
    {
        public NetAstExpression ArrayExpression { get; set; }
        public NetAstExpression IndexExpression { get; set; }
        
        public override Type StaticType
        {
            get { return ArrayExpression.StaticType.GetElementType(); }
        }

        public bool Equals(NetArrayAccessExpression other)
        {
            return ArrayExpression.Equals(other.ArrayExpression) &&
                IndexExpression.Equals(other.IndexExpression);
        }

        public override string ToString()
        {
            return ArrayExpression + "[" + IndexExpression + "]";
        }

        public override IEnumerable<NetAstNode> GetChildren()
        {
            if (this.ArrayExpression != null)
                yield return this.ArrayExpression;

            if (this.IndexExpression != null)
                yield return this.IndexExpression;
        }

        public override NetAstExpression[] GetOperands()
        {
            return new NetAstExpression[]{ ArrayExpression, IndexExpression };
        }

        public override void SetOperandAt(int index, NetAstExpression operand)
        {
            if (index == 0)
                ArrayExpression = operand;
            else if (index == 1)
                IndexExpression = operand;
            else
                throw new IndexOutOfRangeException();
        }

        public override bool CanBeEvaluated
        {
            get { return ArrayExpression.CanBeEvaluated && IndexExpression.CanBeEvaluated; }
        }

        public override object GetValue(object target)
        {
            return ((Array)ArrayExpression.GetValue(target)).GetValue((int)IndexExpression.GetValue(target));
        }
    }

    public class NetAstAssignamentStatement : NetAstStatement, IEquatable<NetAstAssignamentStatement>
    {
        public NetAstExpression Value { get; internal set; }

        public NetAstExpression LeftValue { get; internal set; }

        public override IEnumerable<NetAstNode> GetChildren()
        {
            if (this.LeftValue != null)
                yield return this.LeftValue;

            if (this.Value != null)
                yield return this.Value;
        }

        public bool Equals(NetAstAssignamentStatement other)
        {
            return this.LeftValue.Equals(other.LeftValue) && this.Value.Equals(other.Value);
        }
    }

    public class NetAstIf : NetAstStatement, IEquatable<NetAstIf>
    {
        public NetAstExpression Condition { get; internal set; }

        public NetAstBlock WhenTrue { get; internal set; }

        public NetAstBlock WhenFalse { get; internal set; }

        public bool Equals(NetAstIf other)
        {
            return object.Equals(this.Condition, other.Condition) &&
                object.Equals(this.WhenTrue, other.WhenTrue) &&
                object.Equals(this.WhenFalse, other.WhenFalse);
        }

        public override IEnumerable<NetAstNode> GetChildren()
        {
            if (this.Condition != null)
                yield return this.Condition;
            if (WhenTrue != null)
                yield return WhenTrue;
            if (WhenFalse != null)
                yield return WhenFalse;
        }

        public override string ToString()
        {
            return "if (" + Condition.ToString() + ") " + WhenTrue +  (WhenFalse!= null ? " else " + WhenFalse : "");
        }

        public override NetAstExpression[] GetOperands()
        {
            return new NetAstExpression[] { Condition };
        }

        public override void SetOperandAt(int index, NetAstExpression operand)
        {
            if (index == 0)
                Condition = operand;
            else
                throw new IndexOutOfRangeException();
        }
    }

    public class NetAstFor : NetAstStatement, IEquatable<NetAstFor>
    {
        public NetAstStatement InitAssign { get; set; }

        public NetAstExpression Condition { get; set; }

        public NetAstStatement IterationStatement { get; set; }

        public NetAstBlock Body { get; set; }

        public override IEnumerable<NetAstNode> GetChildren()
        {
            if (this.InitAssign != null)
                yield return this.InitAssign;

            if (this.Condition != null)
                yield return this.Condition;

            if (Body != null)
                yield return Body;

            if (this.IterationStatement != null)
                yield return this.IterationStatement;
        }

        public override string ToString()
        {
            return "for (" + InitAssign + "; " + Condition + "; " + IterationStatement + ")\r\n" + Body;
        }

        public bool Equals(NetAstFor other)
        {
            throw new NotImplementedException();
        }

        public override NetAstExpression[] GetOperands()
        {
            return new NetAstExpression[] { Condition };
        }

        public override void SetOperandAt(int index, NetAstExpression operand)
        {
            if (index == 0)
                Condition = operand;
            else
                throw new IndexOutOfRangeException();
        }
    }

    public class NetAstDoWhile : NetAstStatement, IEquatable<NetAstNode>
    {
        public NetAstExpression Condition { get; internal set; }

        public NetAstBlock Body { get; internal set; }

        public bool Equals(NetAstNode other)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<NetAstNode> GetChildren()
        {
            if (Body != null)
                yield return Body;
            if (this.Condition != null)
                yield return this.Condition;
        }

        public override string ToString()
        {
            return "do\r\n" + Body + "while( " + Condition + " )"; 
        }

        public override NetAstExpression[] GetOperands()
        {
            return new NetAstExpression[] { Condition };
        }

        public override void SetOperandAt(int index, NetAstExpression operand)
        {
            if (index == 0)
                Condition = operand;
            else
                throw new IndexOutOfRangeException();
        }
    }

    public class NetAstWhile : NetAstStatement, IEquatable<NetAstWhile>
    {
        public NetAstExpression Condition { get; internal set; }

        public NetAstBlock Body { get; internal set; }
        public bool Equals(NetAstWhile other)
        {
            return object.Equals(this.Condition, other.Condition) &&
                object.Equals(this.Body, other.Body);
        }

        public override IEnumerable<NetAstNode> GetChildren()
        {
            if (this.Condition != null)
                yield return this.Condition;
            if (Body != null)
                yield return Body;
        }

        public override string ToString()
        {
            return "while (" + (Condition == null ? "true" : Condition.ToString()) + ")" + Body;
        }

        public override NetAstExpression[] GetOperands()
        {
            return new NetAstExpression[] { Condition };
        }

        public override void SetOperandAt(int index, NetAstExpression operand)
        {
            if (index == 0)
                Condition = operand;
            else
                throw new IndexOutOfRangeException();
        }
    }

    public class NetAstContinue : NetAstStatement, IEquatable<NetAstContinue>
    {
        #region IEquatable<NetAstContinue> Members

        public bool Equals(NetAstContinue other)
        {
            return true;
        }

        #endregion

        public override string ToString()
        {
            return "continue";
        }
    }

    public class NetAstBreak : NetAstStatement, IEquatable<NetAstBreak>
    {
        #region IEquatable<NetAstContinue> Members

        public bool Equals(NetAstBreak other)
        {
            return true;
        }

        #endregion

        public override string ToString()
        {
            return "break";
        }
    }

    public class NetAstBlock : NetAstStatement, IEquatable<NetAstBlock>
    {
        public NetAstUnconditionalGoto EntryGoto;

        public List<NetAstStatement> Instructions { get; internal set; }

        public NetAstBlock()
        {
            Instructions = new List<NetAstStatement>();
        }

        public bool Equals(NetAstBlock other)
        {
            return this.Instructions.SequenceEqual(other.Instructions);
        }

        public override IEnumerable<NetAstNode> GetChildren()
        {
            if (this.EntryGoto != null)
                yield return this.EntryGoto;
            foreach (var child in Instructions)
                yield return child;
        }

        public override string ToString()
        {
            if (Instructions.Count == 0) return "{/*empty*/}";
            if (Instructions.Count == 1) return Instructions[0].ToString()+";";

            return "{ \n\r" + string.Join("", Instructions.Select(i => i.ToString() + ";\n\r").ToArray())+"}";
        }
    }

    public class NetAstNullComparitionExpression : NetAstUnaryOperatorExpression, IEquatable<NetAstNullComparitionExpression>
    {
        public override Type StaticType
        {
            get { return typeof(bool); }
        }

        #region IEquatable<NetAstNullTest> Members

        public bool Equals(NetAstNullComparitionExpression other)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "(" + Operand + (Operator.Equals(Operators.Equality) ? "==" : "!=") + "null" + ")";
        }
        #endregion
    }

    public class NetAstReturn : NetAstStatement, IEquatable<NetAstReturn>
    {
        public NetAstExpression Expression { get; internal set; }

        public bool Equals(NetAstReturn other)
        {
            return object.Equals(this.Expression, other.Expression);
        }

        public override string ToString()
        {
            return "return " + (Expression != null ? Expression.ToString() : "");
        }

        public override IEnumerable<NetAstNode> GetChildren()
        {
            if (this.Expression != null)
                yield return this.Expression;
        }

        public override NetAstExpression[] GetOperands()
        {
            return new NetAstExpression[] { Expression };
        }

        public override void SetOperandAt(int index, NetAstExpression operand)
        {
            if (index == 0)
                Expression = operand;
            else
                throw new IndexOutOfRangeException();
        }
    }

    public abstract class NetAstGoto : NetAstStatement
    {
        public NetAstLabel Destination { get; set; }
    }

    public class NetAstUnconditionalGoto : NetAstGoto, IEquatable<NetAstUnconditionalGoto>
    {
        public bool Equals(NetAstUnconditionalGoto other)
        {
            return object.Equals (other.Destination, this.Destination);
        }

        public override string ToString()
        {
            return "goto " + Destination.ToString();
        }
    }

    public class NetAstConditionalGoto : NetAstGoto, IEquatable<NetAstConditionalGoto>
    {
        public NetAstExpression Condition { get; internal set; }

        public override IEnumerable<NetAstNode> GetChildren()
        {
            if (this.Condition != null)
                yield return this.Condition;
        }

        public bool Equals(NetAstConditionalGoto other)
        {
            return object.Equals(this.Condition, other.Condition) && object.Equals(this.Destination, other.Destination);
        }

        public override string ToString()
        {
            return "if (" + Condition.ToString() + ") goto " + Destination.ToString();
        }

        public override NetAstExpression[] GetOperands()
        {
            return new NetAstExpression[] { Condition };
        }

        public override void SetOperandAt(int index, NetAstExpression operand)
        {
            if (index == 0)
                Condition = operand;
            else
                throw new IndexOutOfRangeException();
        }
    }

    public class NetAstLabel : NetAstStatement, IEquatable<NetAstLabel>
    {
        public string Name { get; set; }
        public bool Equals(NetAstLabel other)
        {
            if (Name != null && other.Name != null)
                return other.Name.Equals(Name);
            return other.ILAddress.Equals(ILAddress);
        }

        public override string ToString()
        {
            if (Name != null)
                return Name + ":";
            return "label" + ILAddress+": ";
        }
    }

    public class NetAstSwitch : NetAstStatement, IEquatable<NetAstSwitch>
    {
        public NetAstExpression Condition { get; internal set; }

        public bool Equals(NetAstSwitch other)
        {
            throw new NotImplementedException();
        }

        public NetAstNode[] Cases { get; set; }
    }

    internal class OptBlock : NetAstStatement, IEquatable<OptBlock>
    {
        public List<NetAstStatement> Instructions { get; internal set; }
        public NetAstLabel EntryLabel { get; set; }
        public NetAstUnconditionalGoto FallthoughGoto { get; set; }

        public OptBlock()
        {
            Instructions = new List<NetAstStatement>();
        }
        public bool Equals(OptBlock other)
        {
            return this == other;
        }

        public override IEnumerable<NetAstNode> GetChildren()
        {
            if (this.EntryLabel != null)
                yield return this.EntryLabel;
            foreach (var child in Instructions)
            {
                yield return child;
            }
            if (FallthoughGoto != null)
                yield return this.FallthoughGoto;
        }

        public override string ToString()
        {
            StringWriter sb = new StringWriter();

            
            if (EntryLabel != null)
                sb.WriteLine(EntryLabel.ToString());

            foreach (var item in Instructions)
            {
                sb.WriteLine(item.ToString() + "; ");
            }

            if (FallthoughGoto != null)
                sb.WriteLine(FallthoughGoto.ToString() + "; ");
            
            return sb.ToString();
        }
    }
}
