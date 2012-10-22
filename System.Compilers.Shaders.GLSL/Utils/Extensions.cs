using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.AST.Declarations;
using System.Threading;
using GLSLCompiler.AST.Expressions;
using Antlr.Runtime;
using System.Globalization;

namespace GLSLCompiler.Utils
{
  public static class Extensions
  {
    #region [ Object ]

    public static T Cast<T>(this object obj)
    {
      T result;
      if (!TryCast<T>(obj, out result))
        throw new InvalidCastException();
      return result;
    }

    public static bool TryCast<T>(this object obj, out T result)
    {
      result = default(T);
      if (obj is T)
      {
        result = (T)obj;
        return true;
      }
      return false;
    }

    public static bool Is<T>(this object obj)
    {
      return obj is T;
    }

    public static int ToInt32(this object obj)
    {
      return Convert.ToInt32(obj, CultureInfo.GetCultureInfo("en-us"));
    }

    public static float ToFloat(this object obj)
    {
      return Convert.ToSingle(obj, CultureInfo.GetCultureInfo("en-us"));
    }

    public static bool ToBool(this object obj)
    {
      return Convert.ToBoolean(obj, CultureInfo.GetCultureInfo("en-us"));
    }

    #endregion

    #region [ String ]

    public static string Fmt(this string fmt, params object[] args)
    {
      return String.Format(fmt, args);
    }

    public static StringBuilder AppendIfNotEmpty(this StringBuilder str, string toAppend)
    {
      return str.Length == 0 ? str : str.Append(toAppend);
    }

    public static int ToInt32(this string value)
    {
      if (String.IsNullOrEmpty(value))
        return  int.MinValue;
      if (value.StartsWith("0x") || value.StartsWith("0X"))
        return Convert.ToInt32(value, 16);
      if (value.StartsWith("0"))
        return Convert.ToInt32(value, 8);
      return Convert.ToInt32(value, 10);
    }

    public static bool IsNullOrEmpty(this string value)
    {
      return String.IsNullOrEmpty(value);
    }

    public static bool IsBuiltInType(this string typeid)
    {
      return Enum.IsDefined(typeof(GLSLTypeCode), typeid);
    }

    #endregion

    #region [ ToStringList ]

    public static string ToString(this IEnumerable<char> e)
    {
      if (e == null) return "";
      return new string(e.ToArray());
    }
    public static string ToStringList<T>(this IEnumerable<T> e, string separator, Func<T, string> toStringFunc)
    {
      if (e == null) return "";
      return e.ToStringList(separator, toStringFunc, "...", int.MaxValue);
    }
    public static string ToStringList<T>(this IEnumerable<T> e, string separator, Func<T, string> toStringFunc, string etcetera, int maxElementCount)
    {
      return e.ToStringList(separator, toStringFunc, etcetera, maxElementCount, "", "", null);
    }
    public static string ToStringList<T>(this IEnumerable<T> e, string separator)
    {
      if (e == null) return "";
      return ToStringList(e, separator, t => t.ToString());
    }
    public static string ToStringList<T>(this IEnumerable<T> e, Func<T, string> toStringFunc)
    {
      if (e == null) return "";
      var separator = Thread.CurrentThread.CurrentUICulture.TextInfo.ListSeparator + " ";
      return ToStringList(e, separator, toStringFunc);
    }
    public static string ToStringList<T>(this IEnumerable<T> e)
    {
      if (e == null) return "";
      var separator = Thread.CurrentThread.CurrentUICulture.TextInfo.ListSeparator + " ";
      return ToStringList(e, separator, t => t.ToString());
    }
    public static string ToStringList<T>(this IEnumerable<T> e, string separator, Func<T, string> toStringFunc, string etcetera, int maxElementCount, string opening, string closing, string empty)
    {
      if (maxElementCount < 0) maxElementCount = int.MaxValue;
      var sb = new StringBuilder();
      if (e != null)
        foreach (var t in e)
        {
          sb.AppendIfNotEmpty(separator);
          if (maxElementCount-- <= 0)
          {
            sb.Append(etcetera);
            break;
          }
          sb.Append(toStringFunc(t));
        }
      if (sb.Length == 0 && empty != null)
        return empty;
      return opening + sb.Append(closing);
    }

    #endregion

    #region [ Enumerable ]

    public static IEnumerable<T> Singleton<T>(this T obj)
    {
      yield return obj;
    }

    public static IEnumerable<T> NonSingleton<T>(this T obj, int count)
    {
      return obj.NonSingleton(count, 1);
    }

    public static IEnumerable<T> NonSingleton<T>(this T obj, int count, int stepSize)
    {
      if (count < 0)
        throw new ArgumentException("The parameter must be greater than zero", "count");
      if (stepSize < 1)
        throw new ArgumentException("The parameter must be greater or equal than 1", "stepSize");
      int currentStep = 0;
      for (int i = 0; i < count; i++)
      {
        if (currentStep == 0)
        {
          yield return obj;
          currentStep = stepSize;
        }
        currentStep--;
      }
    }

    public static IEnumerable<TResult> Compact<TSource, TResult>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, TResult> compacter)
    {
      if (first == null)
        throw new ArgumentNullException("first");
      if (second == null)
        throw new ArgumentNullException("second");

      IEnumerator<TSource> enumerator1 = first.GetEnumerator();
      IEnumerator<TSource> enumerator2 = second.GetEnumerator();

      while (enumerator1.MoveNext() && enumerator2.MoveNext())
        yield return compacter(enumerator1.Current, enumerator2.Current);

      enumerator1.Dispose();
      enumerator2.Dispose();
    }

    #endregion

    #region [ FunctionInfo ]

    public static string GetSignature(this FunctionInfo fInfo)
    {
      return "{0}({1})".Fmt(fInfo.Name, fInfo.ParamInfos.Select(pInfo => pInfo.Type).ToStringList(", ", pType => pType.Name));
    }

    #endregion

    #region [ Declaration ]

    public static string GetTypeSpecifierName(this DeclarationAST declaration)
    {
      if (declaration.TypeSpecifier != null)
        return declaration.TypeSpecifier.Name;
      if (declaration is StructDeclarationAST)
        return ((StructDeclarationAST)declaration).Name;
      return string.Empty;
    }

    #endregion

    #region  [ GLSLType ]

    public static bool IsArithmeticType(this GLSLType type)
    {
      GLSLTypeCode typeCode = type.GetTypeCode();
      return typeCode == GLSLTypeCode.Integer || typeCode == GLSLTypeCode.Float;
    }

    public static bool IsArithmeticTypeBased(this GLSLType type)
    {
      GLSLTypeCode typeCode = type.GetTypeCode();
      switch (typeCode)
      {
        case GLSLTypeCode.Integer:
        case GLSLTypeCode.Float:
        case GLSLTypeCode.FloatVec2:
        case GLSLTypeCode.FloatVec3:
        case GLSLTypeCode.FloatVec4:
        case GLSLTypeCode.IntegerVec2:
        case GLSLTypeCode.IntegerVec3:
        case GLSLTypeCode.IntegerVec4:
        case GLSLTypeCode.Mat2x2:
        case GLSLTypeCode.Mat2x3:
        case GLSLTypeCode.Mat2x4:
        case GLSLTypeCode.Mat3x2:
        case GLSLTypeCode.Mat3x3:
        case GLSLTypeCode.Mat3x4:
        case GLSLTypeCode.Mat4x2:
        case GLSLTypeCode.Mat4x3:
        case GLSLTypeCode.Mat4x4:
          return true;
        default:
          return false;
      }
    }

    public static bool IsBool(this GLSLType type)
    {
      return type.GetTypeCode() == GLSLTypeCode.Bool;
    }

    public static bool IsFloat(this GLSLType type)
    {
      return type.GetTypeCode() == GLSLTypeCode.Float;
    }

    public static bool IsInteger(this GLSLType type)
    {
      return type.GetTypeCode() == GLSLTypeCode.Integer;
    }

    public static bool IsScalar(this GLSLType type)
    {
      return type.IsFloat() || type.IsInteger() || type.IsBool();
    }

    public static bool IsVec(this GLSLType type)
    {
      GLSLTypeCode typeCode = type.GetTypeCode();
      switch (typeCode)
      {
        case GLSLTypeCode.FloatVec2:
        case GLSLTypeCode.FloatVec3:
        case GLSLTypeCode.FloatVec4:
        case GLSLTypeCode.IntegerVec2:
        case GLSLTypeCode.IntegerVec3:
        case GLSLTypeCode.IntegerVec4:
        case GLSLTypeCode.BoolVec2:
        case GLSLTypeCode.BoolVec3:
        case GLSLTypeCode.BoolVec4:
          return true;
        default:
          return false;
      }
    }

    public static bool IsMat(this GLSLType type)
    {
      GLSLTypeCode typeCode = type.GetTypeCode();
      switch (typeCode)
      {
        case GLSLTypeCode.Mat2x2:
        case GLSLTypeCode.Mat2x3:
        case GLSLTypeCode.Mat2x4:
        case GLSLTypeCode.Mat3x2:
        case GLSLTypeCode.Mat3x3:
        case GLSLTypeCode.Mat3x4:
        case GLSLTypeCode.Mat4x2:
        case GLSLTypeCode.Mat4x3:
        case GLSLTypeCode.Mat4x4:
          return true;
        default:
          return false;
      }
    }

    public static bool IsSampler1D(this GLSLType type)
    {
      return type.GetTypeCode() == GLSLTypeCode.Sampler1D;
    }

    public static bool IsSampler2D(this GLSLType type)
    {
      return type.GetTypeCode() == GLSLTypeCode.Sampler2D;
    }

    public static bool IsSampler3D(this GLSLType type)
    {
      return type.GetTypeCode() == GLSLTypeCode.Sampler3D;
    }

    public static bool IsSampler(this GLSLType type)
    {
      GLSLTypeCode typeCode = type.GetTypeCode();
      switch (typeCode)
      {
        case GLSLTypeCode.Sampler1D:
        case GLSLTypeCode.Sampler2D:
        case GLSLTypeCode.Sampler3D:
        case GLSLTypeCode.SamplerCube:
        case GLSLTypeCode.Sampler1DShadow:
        case GLSLTypeCode.Sampler2DShadow:
          return true;
        default:
          return false;
      }
    }

    public static bool IsVoid(this GLSLType type)
    {
      return type.GetTypeCode() == GLSLTypeCode.Void;
    }

    public static bool IsArray(this GLSLType type)
    {
      return type.GetTypeCode() == GLSLTypeCode.Array;
    }

    public static bool IsArrayOf(this GLSLType type, GLSLType elementType)
    {
      return type.IsArray() && type.Cast<ArrayType>().ElementType.Equals(elementType);
    }

    public static bool IsStruct(this GLSLType type)
    {
      return type.GetTypeCode() == GLSLTypeCode.Struct;
    }

    public static bool IsBuilInType(this GLSLType type)
    {
      return Enum.IsDefined(typeof(GLSLTypeCode), type.GetTypeCode());
    }

    public static bool IsFloatVec(this GLSLType type)
    {
      GLSLTypeCode typeCode = type.GetTypeCode();
      switch (typeCode)
      {
        case GLSLTypeCode.FloatVec2:
        case GLSLTypeCode.FloatVec3:
        case GLSLTypeCode.FloatVec4:
          return true;
        default:
          return false;
      }
    }

    public static bool IsIntegerVec(this GLSLType type)
    {
      GLSLTypeCode typeCode = type.GetTypeCode();
      switch (typeCode)
      {
        case GLSLTypeCode.IntegerVec2:
        case GLSLTypeCode.IntegerVec3:
        case GLSLTypeCode.IntegerVec4:
          return true;
        default:
          return false;
      }
    }

    public static bool IsBoolVec(this GLSLType type)
    {
      GLSLTypeCode typeCode = type.GetTypeCode();
      switch (typeCode)
      {
        case GLSLTypeCode.BoolVec2:
        case GLSLTypeCode.BoolVec3:
        case GLSLTypeCode.BoolVec4:
          return true;
        default:
          return false;
      }
    }

    public static bool IsFloatBased(this GLSLType type)
    {
      GLSLTypeCode typeCode = type.GetTypeCode();
      switch (typeCode)
      {
        case GLSLTypeCode.Float:
        case GLSLTypeCode.FloatVec2:
        case GLSLTypeCode.FloatVec3:
        case GLSLTypeCode.FloatVec4:
        case GLSLTypeCode.Mat2x2:
        case GLSLTypeCode.Mat2x3:
        case GLSLTypeCode.Mat2x4:
        case GLSLTypeCode.Mat3x2:
        case GLSLTypeCode.Mat3x3:
        case GLSLTypeCode.Mat3x4:
        case GLSLTypeCode.Mat4x2:
        case GLSLTypeCode.Mat4x3:
        case GLSLTypeCode.Mat4x4:
          return true;
        case GLSLTypeCode.Array:
          return ((ArrayType)type).ElementType.IsFloatBased();
        default:
          return false;
      }
    }

    public static GLSLType GetFloatBased(this GLSLType type)
    {
      GLSLTypeCode typeCode = type.GetTypeCode();
      switch (typeCode)
      {
        case GLSLTypeCode.Integer:
          return GLSLTypes.FloatType;
        case GLSLTypeCode.IntegerVec2:
          return GLSLTypes.FloatVec2Type;
        case GLSLTypeCode.IntegerVec3:
          return GLSLTypes.FloatVec3Type;
        case GLSLTypeCode.IntegerVec4:
          return GLSLTypes.FloatVec4Type;
        default:
          return null;
      }
    }

    public static bool IsIntegerBased(this GLSLType type)
    {
      GLSLTypeCode typeCode = type.GetTypeCode();
      switch (typeCode)
      {
        case GLSLTypeCode.Integer:
        case GLSLTypeCode.IntegerVec2:
        case GLSLTypeCode.IntegerVec3:
        case GLSLTypeCode.IntegerVec4:
          return true;
        default:
          return false;
      }
    }

    public static bool IsBoolBased(this GLSLType type)
    {
      GLSLTypeCode typeCode = type.GetTypeCode();
      switch (typeCode)
      {
        case GLSLTypeCode.Bool:
        case GLSLTypeCode.BoolVec2:
        case GLSLTypeCode.BoolVec3:
        case GLSLTypeCode.BoolVec4:
          return true;
        default:
          return false;
      }
    }

    #endregion

    #region [ Expressions ]

    public static bool IsLValue(this ExpressionAST expr)
    {
      return expr is LValueExpressionAST;
    }

    #endregion
 
  }

  public static class Helper
  {
    public static ExpressionAST GetConstructionExpressionAST(IToken tokenId, IEnumerable<ExpressionAST> args, int line, int column)
    {
      switch (tokenId.Type)
      {
        case GLSLLexer.INTEGER:
          return new IntegerConstructionAST(tokenId.Text, args, line, column);
        case GLSLLexer.FLOAT:
          return new FloatConstructionAST(tokenId.Text, args, line, column);
        case GLSLLexer.BOOL:
          return new BoolConstructionAST(tokenId.Text, args, line, column);
        case GLSLLexer.VEC2:
        case GLSLLexer.VEC3:
        case GLSLLexer.VEC4:
        case GLSLLexer.IVEC2:
        case GLSLLexer.IVEC3:
        case GLSLLexer.IVEC4:
        case GLSLLexer.BVEC2:
        case GLSLLexer.BVEC3:
        case GLSLLexer.BVEC4:
          return new VecConstructionAST(tokenId.Text, args, line, column);
        case GLSLLexer.MAT2:
        case GLSLLexer.MAT3:
        case GLSLLexer.MAT4:
        case GLSLLexer.MAT2X2:
        case GLSLLexer.MAT2X3:
        case GLSLLexer.MAT2X4:
        case GLSLLexer.MAT3X2:
        case GLSLLexer.MAT3X3:
        case GLSLLexer.MAT3X4:
        case GLSLLexer.MAT4X2:
        case GLSLLexer.MAT4X3:
        case GLSLLexer.MAT4X4:
          return new MatConstructionAST(tokenId.Text, args, line, column);
        default:
          return new FunctionCallExpressionAST(tokenId.Text, args, line, column);
      }
    }

    public static ExpressionAST GetConstructionExpressionAST(IToken tokenId, IEnumerable<ExpressionAST> args, ExpressionAST expr, int line, int column)
    {
      return new ArrayConstructionAST(tokenId.Text, expr, args, line, column);
    }

    public static GLSLType GetMatType(int rows, int columns)
    {
      return GetType("Mat{0}x{1}", columns, rows);
    }

    public static GLSLType GetFloatVec(int size)
    {
      return GetType("FloatVec{0}", size);
    }

    public static GLSLType GetType(string formatName, params object[] args)
    {
      GLSLTypeCode typeCode = GLSLTypeCode.None;
      string typeCodeString = formatName.Fmt(args);
      if (Enum.IsDefined(typeof(GLSLTypeCode), typeCodeString))
        typeCode = (GLSLTypeCode)Enum.Parse(typeof(GLSLTypeCode), typeCodeString);
      return GLSLTypes.GetTypeByCode(typeCode);
    }
  }
}
