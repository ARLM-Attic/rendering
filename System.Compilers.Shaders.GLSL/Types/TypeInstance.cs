using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Utils;

namespace GLSLCompiler.Types
{
  public abstract class TypeInstance
  {
    public static TypeInstance operator +(TypeInstance first, TypeInstance second)
    {
      return first.AddOverride(second);
    }

    public static TypeInstance operator -(TypeInstance first, TypeInstance second)
    {
      return first.SubOverride(second);
    }

    public static TypeInstance operator *(TypeInstance first, TypeInstance second)
    {
      return first.MulOverride(second);
    }

    public static TypeInstance operator /(TypeInstance first, TypeInstance second)
    {
      return first.DivOverride(second);
    }

    public static TypeInstance operator <(TypeInstance first, TypeInstance second)
    {
      return first.LessOverride(second);
    }

    public static TypeInstance operator <=(TypeInstance first, TypeInstance second)
    {
      return first.LessEqualOverride(second);
    }

    public static TypeInstance operator >(TypeInstance first, TypeInstance second)
    {
      return first.GreaterOverride(second);
    }

    public static TypeInstance operator >=(TypeInstance first, TypeInstance second)
    {
      return first.GreaterEqualOverride(second);
    }

    public static TypeInstance operator !=(TypeInstance first, TypeInstance second)
    {
      return first.EqualOverride(second);
    }

    public static TypeInstance operator ==(TypeInstance first, TypeInstance second)
    {
      return first.NotEqualOverride(second);
    }

    public static TypeInstance operator -(TypeInstance first)
    {
      return first.MinusUnaryOverride();
    }

    public static TypeInstance operator !(TypeInstance first)
    {
      return first.NotUnaryOverride();
    }

    public virtual TypeInstance NotUnaryOverride()
    {
      throw new InvalidOperationException("This type doesn't support this operation");
    }

    public virtual TypeInstance MinusUnaryOverride()
    {
      throw new InvalidOperationException("This type doesn't support this operation");
    }

    public virtual TypeInstance NotEqualOverride(TypeInstance second)
    {
      throw new InvalidOperationException("This type doesn't support this operation");
    }

    public virtual TypeInstance EqualOverride(TypeInstance second)
    {
      throw new InvalidOperationException("This type doesn't support this operation");
    }

    public virtual TypeInstance MulOverride(TypeInstance second)
    {
      throw new InvalidOperationException("This type doesn't support this operation");
    }

    public virtual TypeInstance AddOverride(TypeInstance second)
    {
      throw new InvalidOperationException("This type doesn't support this operation");
    }

    public virtual TypeInstance SubOverride(TypeInstance second)
    {
      throw new InvalidOperationException("This type doesn't support this operation");
    }

    public virtual TypeInstance DivOverride(TypeInstance second)
    {
      throw new InvalidOperationException("This type doesn't support this operation");
    }

    public virtual TypeInstance GreaterEqualOverride(TypeInstance second)
    {
      throw new InvalidOperationException("This type doesn't support this operation");
    }

    public virtual TypeInstance LessOverride(TypeInstance second)
    {
      throw new InvalidOperationException("This type doesn't support this operation");
    }

    public virtual TypeInstance LessEqualOverride(TypeInstance second)
    {
      throw new InvalidOperationException("This type doesn't support this operation");
    }

    public virtual TypeInstance GreaterOverride(TypeInstance second)
    {
      throw new InvalidOperationException("This type doesn't support this operation");
    }

    public virtual TypeInstance this[int index]
    {
      get { throw new InvalidOperationException("This type doesn't support this operation"); }
      set { throw new InvalidOperationException("This type doesn't support this operation"); }
    }

    public virtual TypeInstance DotOverride(string fieldName)
    {
      throw new InvalidOperationException("This type doesn't support this operation");
    }

    public abstract IEnumerable<TypeInstance> Flat();

    protected object _value;
    public virtual object Value
    {
      get { return _value; }
      set { _value = value; }
    }
  }

  public class IntTypeInstance : ScalarTypeInstance
  {
    public override TypeInstance AddOverride(TypeInstance second)
    {
      return new IntTypeInstance()
      {
        Value = Value.ToInt32() + second.Value.ToInt32()
      };
    }

    public override TypeInstance SubOverride(TypeInstance second)
    {
      return new IntTypeInstance()
      {
        Value = Value.ToInt32() - second.Value.ToInt32()
      };
    }

    public override TypeInstance MulOverride(TypeInstance second)
    {
      return new IntTypeInstance()
      {
        Value = Value.ToInt32() * second.Value.ToInt32()
      };
    }

    public override TypeInstance DivOverride(TypeInstance second)
    {
      return new IntTypeInstance()
      {
        Value = Value.ToInt32() / second.Value.ToInt32()
      };
    }

    public override TypeInstance MinusUnaryOverride()
    {
      return new BoolTypeInstance()
      {
        Value = -Value.ToInt32()
      };
    }

    public override TypeInstance LessOverride(TypeInstance second)
    {
      return new BoolTypeInstance()
      {
        Value = Value.ToInt32() < second.Value.ToInt32()
      };
    }

    public override TypeInstance LessEqualOverride(TypeInstance second)
    {
      return new BoolTypeInstance()
      {
        Value = Value.ToInt32() <= second.Value.ToInt32()
      };
    }

    public override TypeInstance GreaterOverride(TypeInstance second)
    {
      return new BoolTypeInstance()
      {
        Value = Value.ToInt32() > second.Value.ToInt32()
      };
    }

    public override TypeInstance GreaterEqualOverride(TypeInstance second)
    {
      return new BoolTypeInstance()
      {
        Value = Value.ToInt32() >= second.Value.ToInt32()
      };
    }

    public override TypeInstance EqualOverride(TypeInstance second)
    {
      return new BoolTypeInstance()
      {
        Value = Value.ToInt32() == second.Value.ToInt32()
      };
    }

    public override TypeInstance NotEqualOverride(TypeInstance second)
    {
      return new BoolTypeInstance()
      {
        Value = Value.ToInt32() != second.Value.ToInt32()
      };
    }

    public override IEnumerable<TypeInstance> Flat()
    {
      yield return this;
    }

    public override object Value
    {
      get
      {
        return _value.ToInt32();
      }
    }
  }

  public class BoolTypeInstance : ScalarTypeInstance
  {
    public override TypeInstance EqualOverride(TypeInstance second)
    {
      return new BoolTypeInstance()
      {
        Value = Value.ToBool() == second.Value.ToBool()
      };
    }

    public override TypeInstance NotEqualOverride(TypeInstance second)
    {
      return new BoolTypeInstance()
      {
        Value = Value.ToBool() != second.Value.ToBool()
      };
    }

    public override TypeInstance NotUnaryOverride()
    {
      return new BoolTypeInstance()
      {
        Value = !Value.ToBool()
      };
    }

    public override IEnumerable<TypeInstance> Flat()
    {
      yield return this;
    }

    public override object Value
    {
      get
      {
        return _value.ToBool();
      }
    }
  }

  public class FloatTypeInstance : ScalarTypeInstance
  {
    public override TypeInstance AddOverride(TypeInstance second)
    {
      return new FloatTypeInstance()
      {
        Value = Value.ToFloat() + second.Value.ToFloat()
      };
    }

    public override TypeInstance SubOverride(TypeInstance second)
    {
      return new FloatTypeInstance()
      {
        Value = Value.ToFloat() - second.Value.ToFloat()
      };
    }

    public override TypeInstance MulOverride(TypeInstance second)
    {
      return new FloatTypeInstance()
      {
        Value = Value.ToFloat() * second.Value.ToFloat()
      };
    }

    public override TypeInstance DivOverride(TypeInstance second)
    {
      return new FloatTypeInstance()
      {
        Value = Value.ToFloat() / second.Value.ToFloat()
      };
    }

    public override TypeInstance MinusUnaryOverride()
    {
      return new BoolTypeInstance()
      {
        Value = -Value.ToFloat()
      };
    }

    public override TypeInstance LessOverride(TypeInstance second)
    {
      return new BoolTypeInstance()
      {
        Value = Value.ToFloat() < second.Value.ToFloat()
      };
    }

    public override TypeInstance LessEqualOverride(TypeInstance second)
    {
      return new BoolTypeInstance()
      {
        Value = Value.ToFloat() <= second.Value.ToFloat()
      };
    }

    public override TypeInstance GreaterOverride(TypeInstance second)
    {
      return new BoolTypeInstance()
      {
        Value = Value.ToFloat() > second.Value.ToFloat()
      };
    }

    public override TypeInstance GreaterEqualOverride(TypeInstance second)
    {
      return new BoolTypeInstance()
      {
        Value = Value.ToFloat() >= second.Value.ToFloat()
      };
    }

    public override TypeInstance EqualOverride(TypeInstance second)
    {
      return new BoolTypeInstance()
      {
        Value = Value.ToFloat() == second.Value.ToFloat()
      };
    }

    public override TypeInstance NotEqualOverride(TypeInstance second)
    {
      return new BoolTypeInstance()
      {
        Value = Value.ToFloat() != second.Value.ToFloat()
      };
    }

    public override IEnumerable<TypeInstance> Flat()
    {
      yield return this;
    }

    public override object Value
    {
      get
      {
        return _value.ToFloat();
      }
    }
  }

  public class ArrayTypeInstance : NonScalarTypeInstance
  {
    public override TypeInstance this[int index]
    {
      get
      {
        if (index < 0 || index > Values.Count)
          throw new IndexOutOfRangeException();
        return Values[index];
      }
      set
      {
        if (index < 0 || index > Values.Count)
          throw new IndexOutOfRangeException();
        Values[index] = value;
      }
    }

    public override IEnumerable<TypeInstance> Flat()
    {
      foreach (var tInstance in Values)
        yield return tInstance;
    }
  }

  public class StructTypeInstance : TypeInstance
  {
    public List<FieldInstance> Values { get; set; }

    public override TypeInstance DotOverride(string fieldName)
    {
      foreach (var fInst in Values)
        if (fInst.FieldName == fieldName)
          return fInst.Value;
      throw new InvalidOperationException();
    }

    public override TypeInstance EqualOverride(TypeInstance second)
    {
      bool result = true;
      foreach (var fInst in Values)
      {
        if ((bool)(fInst.Value != second.DotOverride(fInst.FieldName).Value))
          result = false;
      }
      return new BoolTypeInstance()
      {
        Value = result
      };
    }

    public override TypeInstance NotEqualOverride(TypeInstance second)
    {
      bool result = false;
      foreach (var fInst in Values)
      {
        if ((bool)(fInst.Value != second.DotOverride(fInst.FieldName).Value))
          result = true;
      }
      return new BoolTypeInstance()
      {
        Value = result
      };
    }

    public override IEnumerable<TypeInstance> Flat()
    {
      foreach (var fInst in Values)
        yield return fInst.Value;
    }

    public class FieldInstance
    {
      public string FieldName;
      public TypeInstance Value;
    }
  }

  public class VecTypeInstance : NonScalarTypeInstance
  {
    private List<char> components = new List<char>() { 'x', 'y', 'z', 'w', 'r', 'g', 'b', 'a', 's', 't', 'p', 'q' };

    public override TypeInstance this[int index]
    {
      get
      {
        if (index < 0 || index > Values.Count)
          throw new IndexOutOfRangeException();
        return Values[index];
      }
      set
      {
        if (index < 0 || index > Values.Count)
          throw new IndexOutOfRangeException();
        Values[index] = value;
      }
    }

    public override TypeInstance DotOverride(string fieldName)
    {
      char[] c = fieldName.ToCharArray();
      if (c.Length > 1)
        throw new InvalidOperationException();
      if(!components.Contains(c[0]))
        throw new InvalidOperationException();
      int index = components.IndexOf(c[0]);
      return this[index % 4];
    }

    public override TypeInstance AddOverride(TypeInstance second)
    {
      List<TypeInstance> resultValues = new List<TypeInstance>();
      for (int i = 0; i < Values.Count; i++)
        resultValues.Add(Values[i] + second[i]);
      return new VecTypeInstance()
      {
        Values = resultValues
      };
    }

    public override TypeInstance SubOverride(TypeInstance second)
    {
      List<TypeInstance> resultValues = new List<TypeInstance>();
      for (int i = 0; i < Values.Count; i++)
        resultValues.Add(Values[i] - second[i]);
      return new VecTypeInstance()
      {
        Values = resultValues
      };
    }

    public override TypeInstance MulOverride(TypeInstance second)
    {
      List<TypeInstance> resultValues = new List<TypeInstance>();
      for (int i = 0; i < Values.Count; i++)
        resultValues.Add(Values[i] * second[i]);
      return new VecTypeInstance()
      {
        Values = resultValues
      };
    }

    public override TypeInstance DivOverride(TypeInstance second)
    {
      List<TypeInstance> resultValues = new List<TypeInstance>();
      for (int i = 0; i < Values.Count; i++)
        resultValues.Add(Values[i] / second[i]);
      return new VecTypeInstance()
      {
        Values = resultValues
      };
    }

    public override TypeInstance EqualOverride(TypeInstance second)
    {
      bool result = true;
      for (int i = 0; i < Values.Count; i++)
      {
        if ((bool)(Values[i] != second[i]).Value)
          result = false;
      }
      return new BoolTypeInstance()
      {
        Value = result
      };
    }

    public override TypeInstance NotEqualOverride(TypeInstance second)
    {
      bool result = false;
      for (int i = 0; i < Values.Count; i++)
      {
        if ((bool)(Values[i] != second[i]).Value)
          result = true;
      }
      return new BoolTypeInstance()
      {
        Value = result
      };
    }

    public override TypeInstance MinusUnaryOverride()
    {
      List<TypeInstance> resultValues = new List<TypeInstance>();
      for (int i = 0; i < Values.Count; i++)
        resultValues.Add(-Values[i]);
      return new VecTypeInstance()
      {
        Values = resultValues
      };
    }

    public override IEnumerable<TypeInstance> Flat()
    {
      foreach (var tInstance in Values)
        yield return tInstance;
    }
  }

  public class MatTypeInstance : NonScalarTypeInstance
  {
    public override TypeInstance this[int index]
    {
      get
      {
        return Values[index];
      }
      set
      {
        Values[index] = value;
      }
    }

    public override IEnumerable<TypeInstance> Flat()
    {
      foreach (var tInstance in Values)
        yield return tInstance;
    }
  }

  public abstract class ScalarTypeInstance : TypeInstance
  {
    public override string ToString()
    {
      return Value.ToString();
    }
  }

  public abstract class NonScalarTypeInstance : TypeInstance
  {
    public override string ToString()
    {
      return "[{0}]".Fmt(Values.ToStringList(", "));
    }

    public List<TypeInstance> Values
    {
      get { return Value as List<TypeInstance>; }
      set { Value = value; }
    }
  }
}
