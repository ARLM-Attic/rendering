using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System.Compilers
{
    public class Evaluator
    {
        static Dictionary<Type, int> positions = new Dictionary<Type, int>
            {
                {typeof(bool), 1},
                {typeof(byte), 2},
                {typeof(short), 3},
                {typeof(ushort), 3},
                {typeof(int), 4},
                {typeof(uint), 4},
                {typeof(long), 5},
                {typeof(ulong), 5},
                {typeof(float), 6},
                {typeof(double), 7},
                {typeof(decimal), 8}
            };
        
        public static Type MoreGeneral(Type a, Type b)
        {
            if (positions.ContainsKey(a) && positions.ContainsKey(b))
                return positions[a] < positions[b] ? b : a;
            return a;
        }

        public static object Eval(Operators op, object x, object y)
        {
            if (x is IConvertible && y is IConvertible)
            {
                Type moreGeneral = MoreGeneral(x.GetType(), y.GetType());
                decimal xd = Convert.ToDecimal(x, System.Globalization.NumberFormatInfo.InvariantInfo);
                decimal yd = Convert.ToDecimal(y, System.Globalization.NumberFormatInfo.InvariantInfo);
                long xi = Convert.ToInt64(x, System.Globalization.NumberFormatInfo.InvariantInfo);
                long yi = Convert.ToInt64(y, System.Globalization.NumberFormatInfo.InvariantInfo);
                bool xb = Convert.ToBoolean(x, System.Globalization.NumberFormatInfo.InvariantInfo);
                bool yb = Convert.ToBoolean(y, System.Globalization.NumberFormatInfo.InvariantInfo);

                switch (op)
                {
                    case Operators.Addition: return Convert.ChangeType(xd + yd, moreGeneral);
                    case Operators.Subtraction: return Convert.ChangeType(xd - yd, moreGeneral);
                    case Operators.Multiply: return Convert.ChangeType(xd * yd, moreGeneral);
                    case Operators.Division: return Convert.ChangeType(xd / yd, moreGeneral);
                    case Operators.Equality: return Convert.ToBoolean(xd == yd);
                    case Operators.Inequality: return Convert.ToBoolean(xd != yd);
                    case Operators.LessThan: return Convert.ToBoolean(xd < yd);
                    case Operators.LessThanOrEquals: return Convert.ToBoolean(xd <= yd);
                    case Operators.GreaterThan: return Convert.ToBoolean(xd > yd);
                    case Operators.GreaterThanOrEquals: return Convert.ToBoolean(xd >= yd);
                    case Operators.LogicAnd: return Convert.ChangeType(xi & yi, moreGeneral);
                    case Operators.LogicOr: return Convert.ChangeType(xi | yi, moreGeneral);
                    case Operators.LogicXor: return Convert.ChangeType(xi ^ yi, moreGeneral);
                    case Operators.Modulus: return Convert.ChangeType(xi % yi, moreGeneral);
                    case Operators.ConditionalAnd: return Convert.ToBoolean(xb && yb);
                    case Operators.ConditionalOr: return Convert.ToBoolean(xb || yb);
                    default: throw new NotSupportedException();
                }
            }

            MethodInfo method = null;

            method = x.GetType().GetMethod("op_" + op, new Type[] { x.GetType(), y.GetType() });

            if (method == null)
                method = y.GetType().GetMethod("op_" + op, new Type[] { x.GetType(), y.GetType() });

            if (method == null)
                throw new NotSupportedException();

            return method.Invoke(null, new object[] { x, y });
        }

        public static object Eval(Type castType, object x)
        {
            MethodInfo convertMethod = null;

            convertMethod = castType.GetMethod("op_Implicit", new Type[] { x.GetType() }) ?? castType.GetMethod("op_Explicit", new Type[] { x.GetType() });

            if (convertMethod != null)
                return convertMethod.Invoke(null, new object[] { x });

            if (x is IConvertible)
                return Convert.ChangeType(x, castType);

            throw new NotSupportedException();
        }

        public static object Eval(Operators op, object x)
        {
            if (x is IConvertible)
            {
                decimal xd = Convert.ToDecimal(x, System.Globalization.NumberFormatInfo.InvariantInfo);
                long xi = Convert.ToInt64(x, System.Globalization.NumberFormatInfo.InvariantInfo);
                bool xb = Convert.ToBoolean(x, System.Globalization.NumberFormatInfo.InvariantInfo);

                switch (op)
                {
                    case Operators.UnaryNegation:
                        return Convert.ChangeType(-xd, x.GetType());
                    case Operators.UnaryPlus:
                        return x;
                    case Operators.Not:
                        return !xb;
                    default: throw new NotSupportedException();
                }
            }

            MethodInfo method = null;

            method = x.GetType().GetMethod("op_" + op, new Type[] { x.GetType() });

            if (method == null)
                throw new NotSupportedException();

            return method.Invoke(null, new object[] { x });
        }
    }
}
