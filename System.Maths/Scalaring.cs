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

namespace System.Maths.Scalaring
{
    internal interface IOperations<T> where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        T Zero { get; }

        T One { get; }

        T Add(T e1, T e2);

        T Substract(T e1, T e2);

        T Multiply(T e1, T e2);

        T Divide(T e1, T e2);

        T InverseOf(T e);

        T NegativeOf(T e);

        FLOATINGTYPE ConvertToFloating(T e);

        T ConvertFromFloating(FLOATINGTYPE f);
    }

    public static class Scalars
    {
        class Int32Operations : IOperations<int>
        {
            #region IOperations<int> Members

            public int Zero
            {
                get { return 0; }
            }

            public int One
            {
                get { return 1; }
            }

            public int Add(int e1, int e2)
            {
                return e1 + e2;
            }

            public int Substract(int e1, int e2)
            {
                return e1 - e2;
            }

            public int Multiply(int e1, int e2)
            {
                return e1 * e2;
            }

            public int Divide(int e1, int e2)
            {
                return e1 / e2;
            }

            public int InverseOf(int e)
            {
                throw new NotSupportedException("Int32 type can not support inverse value of the elements");
            }

            public int NegativeOf(int e)
            {
                return -e;
            }

            public FLOATINGTYPE ConvertToFloating(int e)
            {
                return (FLOATINGTYPE)e;
            }

            public int ConvertFromFloating(FLOATINGTYPE f)
            {
                return (int)f;
            }

            #endregion
        }

        class BoolOperations : IOperations<bool>
        {
            #region IOperations<bool> Members

            public bool Zero
            {
                get { return false; }
            }

            public bool One
            {
                get { return true; }
            }

            public bool Add(bool e1, bool e2)
            {
                return e1 || e2;
            }

            public bool Multiply(bool e1, bool e2)
            {
                return e1 && e2;
            }

            public bool Substract(bool e1, bool e2)
            {
                return e1 ^ e2;
            }

            public bool Divide(bool e1, bool e2)
            {
                throw new NotSupportedException();
            }

            public bool InverseOf(bool e)
            {
                throw new NotSupportedException("Bool type can not support inverse of its elements.");
            }

            public bool NegativeOf(bool e)
            {
                return e;
            }

            public FLOATINGTYPE ConvertToFloating(bool e)
            {
                return e ? 1 : 0;
            }

            public bool ConvertFromFloating(FLOATINGTYPE f)
            {
                return f != 0;
            }

            #endregion
        }

        class FloatOperations : IOperations<float>
        {
            #region IOperations<float> Members

            public float Zero
            {
                get { return 0f; }
            }

            public float One
            {
                get { return 1f; }
            }

            public float Add(float e1, float e2)
            {
                return e1 + e2;
            }

            public float Substract(float e1, float e2)
            {
                return e1 - e2;
            }

            public float Multiply(float e1, float e2)
            {
                return e1 * e2;
            }

            public float Divide(float e1, float e2)
            {
                return e1 / e2;
            }

            public float InverseOf(float e)
            {
                return 1.0f / e;
            }

            public float NegativeOf(float e)
            {
                return -e;
            }

            public FLOATINGTYPE ConvertToFloating(float e)
            {
                return (FLOATINGTYPE)e;
            }

            public float ConvertFromFloating(FLOATINGTYPE f)
            {
                return (float)f;
            }

            #endregion
        }

        class DoubleOperations : IOperations<double>
        {
            #region IOperations<double> Members

            public double Zero
            {
                get { return 0.0; }
            }

            public double One
            {
                get { return 1.0; }
            }

            public double Add(double e1, double e2)
            {
                return e1 + e2;
            }

            public double Substract(double e1, double e2)
            {
                return e1 - e2;
            }

            public double Multiply(double e1, double e2)
            {
                return e1 * e2;
            }

            public double Divide(double e1, double e2)
            {
                return e1 / e2;
            }

            public double InverseOf(double e)
            {
                return 1.0 / e;
            }

            public double NegativeOf(double e)
            {
                return -e;
            }

            public FLOATINGTYPE ConvertToFloating(double e)
            {
                return (FLOATINGTYPE)e;
            }

            public double ConvertFromFloating(FLOATINGTYPE f)
            {
                return (double)f;
            }

            #endregion
        }

        static class Operations<T> where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
        {
            public static T Zero;
            public static T One;

            public static IOperations<T> strategy;

            static Operations()
            {
                if (typeof(T) == typeof(int))
                    strategy = (IOperations<T>)new Int32Operations();
                if (typeof(T) == typeof(float))
                    strategy = (IOperations<T>)new FloatOperations();
                if (typeof(T) == typeof(bool))
                    strategy = (IOperations<T>)new BoolOperations();
                if (typeof(T) == typeof(double))
                    strategy = (IOperations<T>)new DoubleOperations();

                if (strategy == null)
                    throw new NotSupportedException("Type " + typeof(T).Name + " cannot be used as Scalar type. Please use int, float, bool or double types instead.");

                Zero = strategy.Zero;

                One = strategy.One;
            }
        }

        internal static IOperations<T> GetStrategy<T>() where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return Operations<T>.strategy;
        }

        private static T[] Zip<T>(T[] e1, T[] e2, Func<T, T, T> operation)
        {
            T[] result = new T[e1.Length];
            for (int i = 0; i < e1.Length; i++)
                result[i] = operation(e1[i], e2[i]);
            return result;
        }

        private static T[,] Zip<T>(T[,] e1, T[,] e2, Func<T, T, T> operation)
        {
            T[,] result = new T[e1.GetLength(0), e1.GetLength(1)];
            for (int i = 0; i < e1.GetLength(0); i++)
                for (int j = 0; j < e1.GetLength(1); j++)
                    result[i, j] = operation(e1[i, j], e2[i, j]);
            return result;
        }

        public static T Add<T>(this T e1, T e2) where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return Operations<T>.strategy.Add(e1, e2);
        }

        public static T[] Add<T>(this T[] v1, T[] v2) where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return Zip(v1, v2, (x1, x2) => x1.Add(x2));   
        }

        public static T Multiply<T>(this T e1, T e2) where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return Operations<T>.strategy.Multiply(e1, e2);
        }

        public static T Divide<T>(this T e1, T e2) where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return Operations<T>.strategy.Divide (e1, e2);
        }

        public static T Substract<T>(this T e1, T e2) where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return Operations<T>.strategy.Substract(e1, e2);
        }

        public static FLOATINGTYPE AsFloating<T>(this T e) where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return Operations<T>.strategy.ConvertToFloating(e);
        }

        public static T As<T>(this FLOATINGTYPE f) where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return Operations<T>.strategy.ConvertFromFloating(f);
        }

        public static T Zero<T>() where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return Operations<T>.Zero;
        }

        public static T One<T>() where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return Operations<T>.One;
        }

        public static T Max<T>(T e1, T e2) where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return e1.CompareTo(e2) >= 0 ? e1 : e2;
        }

        public static T Min<T>(T e1, T e2) where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return e1.CompareTo(e2) <= 0 ? e1 : e2;
        }

        public static T Clamp<T>(T e) where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return Max (Zero<T>(), Min (One<T>(), e));
        }
    }
}
