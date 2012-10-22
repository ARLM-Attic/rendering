using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Compilers
{
    using System.Compilers.Shaders.Info;

    public static class ShaderTypeExtensors
    {
        abstract class ShaderTypeBase : ShaderType
        {
            public virtual bool IsArray
            {
                get { return false; }
            }

            public virtual bool IsGlobal
            {
                get { return false; }
            }

            public virtual int Rank
            {
                get { return 0; }
            }

            public virtual int[] Ranks
            {
                get { return null; }
            }

            public virtual bool IsGenericType
            {
                get { return false; }
            }

            public virtual bool IsGenericTypeDefinition
            {
                get { return false; }
            }

            public virtual bool IsGenericParameter
            {
                get { return false; }
            }

            public virtual ShaderType[] GetGenericParameters()
            {
                return new ShaderType[0];
            }

            public virtual ShaderType GetGenericDefinition()
            {
                return null;
            }

            public virtual ShaderType BaseType
            {
                get { return null; }
            }

            public virtual ShaderType ElementType
            {
                get { return null; }
            }

            public virtual IEnumerable<ShaderMember> Members
            {
                get { yield break; }
            }

            public virtual string Name
            {
                get { return ""; }
            }

            public virtual bool IsBuiltin
            {
                get { return false; }
            }

            public ShaderType DeclaringType
            {
                get { return null; }
            }

            public ShaderType MakeGenericType(params ShaderType[] typeArguments)
            {
                throw new NotSupportedException();
            }
        }

        #region Arrays 

        class ShaderArrayType : ShaderTypeBase
        {
            ShaderType elementType;
            int rank;
            int[] ranks;

            public ShaderArrayType(ShaderType elementType, int rank, int[] ranks)
            {
                this.elementType = elementType;
                this.rank = rank;
                this.ranks = ranks != null ? ranks.Clone() as int[] : null;
            }

            public override ShaderType ElementType
            {
                get
                {
                    return elementType;
                }
            }

            public override bool IsArray
            {
                get
                {
                    return true;
                }
            }

            public override int Rank
            {
                get
                {
                    return rank;
                }
            }

            public override int[] Ranks
            {
                get
                {
                    return ranks;
                }
            }

            public override string Name
            {
                get
                {
                    return elementType.Name + "[" +
                        ((ranks == null) ?
                        new string(',', rank-1) :
                        string.Join(",", ranks.Select (r=>r.ToString()).ToArray()))
                        + "]";
                }
            }

            public override string ToString()
            {
                return Name;
            }
        }

        public static ShaderType MakeArray(this ShaderType type, int rank)
        {
            return new ShaderArrayType(type, rank, null);
        }

        public static ShaderType MakeFixedArray(this ShaderType type, int[] ranks)
        {
            return new ShaderArrayType(type, ranks.Length, ranks);
        }

        #endregion

        internal static bool InternalIsSubClass(this ShaderType type, ShaderType other, out int distance)
        {
            distance = 0;
            ShaderType baseType;
            do
            {
                baseType = other.BaseType;
                distance++;
                if (baseType == type)
                    return true;
            }
            while (baseType != null);

            return false;
        }

        public static bool IsSubclass(this ShaderType type, ShaderType other)
        {
            int dist;
            return type.InternalIsSubClass(other, out dist);
        }

        #region Genericity

        class ShaderGenericParameter : ShaderTypeBase
        {
            ShaderType declaringType;
            ShaderMethod declaringMethod;
            string name;

            public ShaderGenericParameter(ShaderType declaringType, string name)
            {
                this.declaringType = declaringType;
                this.name = name;
            }

            public ShaderGenericParameter(ShaderMethod declaringMethod, string name)
            {
                this.declaringMethod = declaringMethod;
                this.name = name;
            }

            public override bool IsGenericParameter
            {
                get
                {
                    return true;
                }
            }

            public override string Name
            {
                get
                {
                    return name;
                }
            }

            public override string ToString()
            {
                return Name;
            }
        }

        class ShaderGenericField : ShaderField
        {
            internal ShaderGenericField(ShaderField field, Dictionary<ShaderType, ShaderType> genericityMap)
            {
                this.field = field;
                this.genericityMap = genericityMap;
            }

            ShaderField field;
            Dictionary<ShaderType, ShaderType> genericityMap;

            public Shaders.Semantic Semantic
            {
                get { return field.Semantic; }
            }

            public ShaderType Type
            {
                get { return ResolveGenericType(field.Type, genericityMap); }
            }

            public string Name
            {
                get { return field.Name; }
            }

            public bool IsBuiltin
            {
                get { return field.IsBuiltin; }
            }

            public bool IsGlobal
            {
                get { return field.IsGlobal; }
            }

            public ShaderType DeclaringType
            {
                get { return ResolveGenericType(field.DeclaringType, genericityMap); }
            }
        }

        class ShaderGenericMethod : ShaderMethod
        {
            internal ShaderGenericMethod(ShaderMethod declaration, Dictionary<ShaderType, ShaderType> genericityMap)
            {
                this.declaration = declaration;
                this.genericityMap = genericityMap;
            }

            ShaderMethod declaration;
            Dictionary<ShaderType, ShaderType> genericityMap;

            public ShaderType ReturnType
            {
                get { return ResolveGenericType (declaration.ReturnType, genericityMap); }
            }

            public bool IsGenericDefinition
            {
                get { return false; }
            }

            public bool IsGenericMethod
            {
                get { return true; }
            }

            public ShaderMethod GetGenericDefinition()
            {
                return this.declaration;
            }

            public ShaderType[] GetGenericParameters()
            {
                return this.declaration.GetGenericParameters().Select(t => ResolveGenericType(t, genericityMap)).ToArray();
            }

            public Operators Operator
            {
                get { return declaration.Operator; }
            }

            public IEnumerable<ShaderParameter> Parameters
            {
                get
                {
                    return declaration.Parameters.Select(p => new ShaderParameter(ResolveGenericType(p.ParameterType, genericityMap), p.Name, p.Semantic, p.Modifier));
                }
            }

            public string Name
            {
                get { return declaration.Name; }
            }

            public bool IsBuiltin
            {
                get { return declaration.IsBuiltin; }
            }

            public bool IsGlobal
            {
                get { return declaration.IsGlobal; }
            }

            public ShaderType DeclaringType
            {
                get { return ResolveGenericType(declaration.DeclaringType, genericityMap); }
            }

            public bool IsAbstract { get { return declaration.IsAbstract; } }
        }

        class ShaderGenericConstructor : ShaderConstructor
        {
            internal ShaderGenericConstructor(ShaderConstructor constructor, Dictionary<ShaderType, ShaderType> genericityMap)
            {
                this.declaration = constructor;
                this.genericityMap = genericityMap;
            }

            ShaderConstructor declaration;
            Dictionary<ShaderType, ShaderType> genericityMap;

            public IEnumerable<ShaderParameter> Parameters
            {
                get { return declaration.Parameters.Select(p => new ShaderParameter(ResolveGenericType(p.ParameterType, genericityMap), p.Name, p.Semantic, p.Modifier)); }
            }

            public string Name
            {
                get { return declaration.Name; }
            }

            public bool IsBuiltin
            {
                get { return declaration.IsBuiltin; }
            }

            public bool IsGlobal
            {
                get { return declaration.IsGlobal; }
            }

            public ShaderType DeclaringType
            {
                get
                {
                    return ResolveGenericType(declaration.DeclaringType, genericityMap);
                }
            }
        }

        class ShaderGenericType : ShaderTypeBase
        {
            public ShaderGenericType(ShaderType genericDefinition, Dictionary<ShaderType, ShaderType> genericityMap)
            {
                this.genericDefinition = genericDefinition;
                this.genericityMap = genericityMap;

                this.genericParameters = this.genericDefinition.GetGenericParameters().Select
                    (t => ShaderTypeExtensors.ResolveGenericType(t, genericityMap)).ToArray();
            }

            ShaderType genericDefinition;
            Dictionary<ShaderType, ShaderType> genericityMap;
            ShaderType[] genericParameters;

            public override bool IsGenericType
            {
                get
                {
                    return true;
                }
            }

            public override ShaderType GetGenericDefinition()
            {
                return genericDefinition;
            }

            public override ShaderType[] GetGenericParameters()
            {
                return genericParameters;
            }

            public override ShaderType BaseType
            {
                get
                {
                    return ResolveGenericType(genericDefinition.BaseType, genericityMap);
                }
            }

            public override string Name
            {
                get
                {
                    return genericDefinition.Name + "[" + string.Join(",", genericParameters.Select(a => a.Name).ToArray()) + "]";
                }
            }

            public override IEnumerable<ShaderMember> Members
            {
                get
                {
                    foreach (var m in genericDefinition.Members)
                    {
                        if (m is ShaderField)
                            yield return new ShaderGenericField(m as ShaderField, genericityMap);
                        else
                            if (m is ShaderMethod)
                                yield return new ShaderGenericMethod(m as ShaderMethod, genericityMap);
                            else
                                if (m is ShaderConstructor)
                                    yield return new ShaderGenericConstructor(m as ShaderConstructor, genericityMap);
                                else
                                    if (m is ShaderType)
                                        yield return new ShaderGenericType(m as ShaderType, genericityMap);
                    }
                }
            }

            public override string ToString()
            {
                return "Generic: " + Name;
            }
        }

        internal static ShaderType CreateGenericParameter(ShaderType genericDefinition, string name)
        {
            return new ShaderGenericParameter(genericDefinition, name);
        }

        internal static ShaderType CreateGenericParameter(ShaderMethod genericDefinition, string name)
        {
            return new ShaderGenericParameter(genericDefinition, name);
        }

        struct ShaderMemberGenericInstanceKey : IEquatable<ShaderMemberGenericInstanceKey>
        {
            public ShaderMember GenericDefinition;

            public Dictionary<ShaderType, ShaderType> GenericityMap;

            public override bool Equals(object obj)
            {
                return obj is ShaderMemberGenericInstanceKey && this.Equals((ShaderMemberGenericInstanceKey)obj);
            }

            public bool Equals(ShaderMemberGenericInstanceKey other)
            {
                return this.GenericDefinition.Equals(other.GenericDefinition) && this.GenericityMap.SequenceEqual(other.GenericityMap);
            }

            public override int GetHashCode()
            {
                return GenericDefinition.GetHashCode() * GenericityMap.Aggregate(1, (a, s) => (a + 7) * (s.GetHashCode() + 13));
            }
        }

        private static readonly Dictionary<ShaderMemberGenericInstanceKey, ShaderMember> cache = new Dictionary<ShaderMemberGenericInstanceKey, ShaderMember>();

        internal static ShaderType MakeGenericType(ShaderType type, params ShaderType[] typeArguments)
        {
            typeArguments = typeArguments.Clone() as ShaderType[];

            ShaderMemberGenericInstanceKey key = new ShaderMemberGenericInstanceKey
            {
                GenericDefinition = type,
                GenericityMap = GetGenericityMap(type.GetGenericParameters(), typeArguments)
            };

            if (!cache.ContainsKey(key))
                cache.Add(key, BuildGenericType(type, typeArguments));

            return (ShaderType)cache[key];
        }

        internal static ShaderMethod MakeGenericMethod(ShaderMethod method, params ShaderType[] typeArguments)
        {
            typeArguments = typeArguments.Clone() as ShaderType[];

            ShaderMemberGenericInstanceKey key = new ShaderMemberGenericInstanceKey
            {
                GenericDefinition = method,
                GenericityMap = GetGenericityMap(method.GetGenericParameters(), typeArguments)
            };

            if (!cache.ContainsKey(key))
                cache.Add(key, BuildGenericMethod(method, typeArguments));

            return (ShaderMethod)cache[key];
        }

        internal static Dictionary<ShaderType, ShaderType> GetGenericityMap(ShaderType[] genericParameters, ShaderType[] typeArguments)
        {
            Dictionary<ShaderType, ShaderType> genericityMap = new Dictionary<ShaderType, ShaderType>();

            for (int i = 0; i < genericParameters.Length; i++)
                genericityMap.Add(genericParameters[i], typeArguments[i]);

            return genericityMap;
        }

        private static ShaderType BuildGenericType(ShaderType type, params ShaderType[] typeArguments)
        {
            if (!type.IsGenericTypeDefinition)
                throw new ArgumentException();

            ShaderType[] args = type.GetGenericParameters();

            if (typeArguments.Length != args.Length)
                throw new ArgumentException("typeArguments parameters doesnt contain same length than generic definition");

            var gMap = GetGenericityMap(args, typeArguments);


            return new ShaderGenericType(type, gMap);
        }

        private static ShaderMethod BuildGenericMethod(ShaderMethod method, params ShaderType[] typeArguments)
        {
            if (!method.IsGenericDefinition)
                throw new ArgumentException();

            ShaderType[] args = method.GetGenericParameters();

            if (typeArguments.Length != args.Length)
                throw new ArgumentException("typeArguments parameters doesnt contain same length than generic definition");

            var gMap = GetGenericityMap(args, typeArguments);

            return new ShaderGenericMethod(method, gMap);
        }

        internal static ShaderType ResolveGenericType(ShaderType t, Dictionary<ShaderType, ShaderType> genericityMap)
        {
            if (t == null)
                return null;

            if (genericityMap.ContainsKey(t))
                return genericityMap[t];

            if (!(t.IsGenericType || t.IsGenericTypeDefinition))
                return t;

            ShaderType[] genericParameters = t.GetGenericDefinition().GetGenericParameters();

            ShaderType[] argumentsForBase = t.GetGenericParameters().Select(type =>
                ResolveGenericType(type, genericityMap)).ToArray();

            var gMap = ShaderTypeExtensors.GetGenericityMap(genericParameters, argumentsForBase);

            return new ShaderGenericType(t.GetGenericDefinition(), gMap);
        }


        #endregion
    }
}
