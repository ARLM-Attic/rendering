using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Rendering.Effects.Shaders
{
    public abstract class Builtins
    {
        public static Builtins Empty { get; private set; }

        static Builtins() { Empty = new EmptyBuiltins(); }

        internal Dictionary<MemberInfo, ShaderMember> membersMap = new Dictionary<MemberInfo, ShaderMember>();
        internal Dictionary<ShaderMember, MemberInfo> shadersMap = new Dictionary<ShaderMember, MemberInfo>();

        private void Register(System.Reflection.MemberInfo netMember, ShaderMember shaderMember)
        {
            membersMap.Add(netMember, shaderMember);
            shadersMap.Add(shaderMember, netMember);
        }

        Dictionary<Type, ShaderType> arrays = new Dictionary<Type, ShaderType>();

        public string GetName(ShaderType type)
        {
            if (type.IsPrimitive)
                return ResolveType(((PrimitiveShaderType)type).NetType).Name;
            return type.Name;
        }

        public string GetName(ShaderFunction function)
        {
            if (function.IsPrimitive)
                return ResolveFunction(((PrimitiveShaderFunction)function).NetFunction).Name;
            return function.Name;
        }

        public string GetName(ShaderGlobal global)
        {
            return global.Name;
        }

        public string GetName(ShaderField field)
        {
            if (field.IsPrimitive)
                return ResolveField(((PrimitiveShaderField)field).NetField).Name;
            return field.Name;
        }

        public ShaderType ResolveType(Type type)
        {
            if (type.IsArray)
            {
                ShaderType elementType = ResolveType(type.GetElementType());
                if (!arrays.ContainsKey(type))
                    arrays.Add(type, elementType.MakeArrayType());
                return arrays[type];
            }

            if (type.IsGenericParameter)
            {
                throw new NotSupportedException("Can not support generic definitions nor generic arguments.");
            }

            if (type.IsGenericTypeDefinition)
            {
                throw new NotSupportedException("Can not support generic definitions nor generic arguments.");
            }

            if (membersMap.ContainsKey(type))
                return (ShaderType)membersMap[type];

            return null;
        }

        public Type ResolveType(ShaderType type)
        {
            if (type is ShaderArray)
            {
                Type elementType = this.ResolveType(((ShaderArray)type).ElementType);
                return elementType.MakeArrayType();
            }

            if (shadersMap.ContainsKey(type))
                return shadersMap[type] as Type;
            
            return null;
        }

        public object Invoke(ShaderFunction function, params object[] args)
        {
            if (function.IsPrimitive)
            {
                PrimitiveShaderFunction primitiveFunction = function as PrimitiveShaderFunction;
                if (!primitiveFunction.NetFunction.IsStatic)
                    primitiveFunction.NetFunction.Invoke(args[0], args.Skip(1).ToArray());
                else
                    primitiveFunction.NetFunction.Invoke(null, args);
            }
            throw new NotSupportedException();
        }

        public object Invoke(ShaderField field, object target)
        {
            if (field.IsPrimitive)
            {
                return ((PrimitiveShaderField)field).NetField.GetValue(target);
            }
            throw new NotSupportedException();
        }

        public ShaderField ResolveField(System.Reflection.FieldInfo field)
        {
            if (membersMap.ContainsKey(field))
                return (ShaderField)membersMap[field];

            return null;
        }

        public ShaderFunction ResolveFunction(MethodInfo method)
        {
            if (membersMap.ContainsKey(method))
                return (ShaderFunction)membersMap[method];

            return null;
        }

        public ShaderConstructor ResolveConstructor(System.Reflection.ConstructorInfo constructor)
        {
            if (membersMap.ContainsKey(constructor))
                return (ShaderConstructor)membersMap[constructor];

            return null;
        }

        public bool IsDefined(System.Reflection.MemberInfo member)
        {
            return membersMap.ContainsKey(member);
        }

        private void InternalRegisterFunction(System.Reflection.MethodInfo method, string name, Operators op)
        {
            ShaderType returnType = ResolveType(method.ReturnType);

            if (returnType == null)
                throw new ArgumentException("Can not resolve type " + method.ReturnType.Name);

            ShaderFunction function = (op == Operators.None) ? new PrimitiveShaderFunction(this, method, name) :
                new PrimitiveShaderFunction(this, method, op);

            Register(method, function);
        }

        protected void FullRegisterStructMembers(Type structType, string[] fieldNames)
        {
            List<FieldInfo> fields = structType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList();
            fields.Sort((f1, f2) =>
            {
                return Marshal.OffsetOf(structType, f1.Name).ToInt32().CompareTo(
                    Marshal.OffsetOf(structType, f2.Name).ToInt32()
                    );
            });

            if (fields.Count != fieldNames.Length)
                throw new ArgumentException("Field name list provided doesnt match with type field count.");

            for (int i = 0; i < fieldNames.Length; i++)
                RegisterField(fields[i], fieldNames[i]);

            var operators = structType.GetMethods(BindingFlags.Static | BindingFlags.Public).Where(m => m.IsSpecialName && m.Name.StartsWith("op_"));

            foreach (var op in operators)
            {
                Operators oper = GetOperatorByName(op.Name);
                if (oper != Operators.None)
                    RegisterFunction(op, oper);
            }

            foreach (var constructorInfo in structType.GetConstructors())
                RegisterConstructor(constructorInfo);
        }

        private Operators GetOperatorByName(string op)
        {
            switch (op)
            {
                case "op_Addition": return Operators.Addition;
                case "op_Subtraction": return Operators.Subtraction;
                case "op_Multiply": return Operators.Multiply;
                case "op_Division": return Operators.Division;
                case "op_Explicit": return Operators.Cast;
                case "op_Implicit": return Operators.Cast;
                default: return Operators.None;
            }
        }

        protected void FullRegisterScalarMembers(Type type)
        {
            if (!type.IsPrimitive)
                throw new ArgumentException("Can not treat type " + type.Name + " as scalar because is not primitive type.");
            
            var conversionsFrom =
            typeof(Convert).GetMethods(BindingFlags.Static | BindingFlags.Public).Where(m => m.Name.StartsWith("To") && m.GetParameters().Length == 1 &&
            m.GetParameters()[0].ParameterType == type && membersMap.ContainsKey(m.ReturnType));

            foreach (var m in conversionsFrom)
                if (!this.IsDefined(m))
                    RegisterFunction(m, Operators.Cast);

            var conversionsTo =
            typeof(Convert).GetMethods(BindingFlags.Static | BindingFlags.Public).Where(m => m.Name.StartsWith("To") && m.GetParameters().Length == 1 &&
            membersMap.ContainsKey(m.GetParameters()[0].ParameterType) && m.ReturnType == type);

            foreach (var m in conversionsTo)
                if (!this.IsDefined(m))
                    RegisterFunction(m, Operators.Cast);
        }

        protected void FullRegisterScalarMembers<T>()
        {
            FullRegisterScalarMembers(typeof(T));
        }

        protected void RegisterAvailableOverloads(Type target, string methodName, string shaderMethodName)
        {
            foreach (var method in target.GetMethods().Where(m => m.Name.Equals(methodName)))
            {
                bool allTypePresent = true;
                foreach (var p in method.GetParameters())
                {
                    ShaderType type = this.ResolveType(p.ParameterType);
                    if (type == null)
                        allTypePresent = false;

                    if (!allTypePresent)
                        break;
                }

                if (allTypePresent)
                    RegisterFunction(method, shaderMethodName);
            }
        }

        protected void RegisterFunction(System.Reflection.MethodInfo method, string name)
        {
            InternalRegisterFunction(method, name, Operators.None);
        }

        protected void RegisterFunction(System.Reflection.MethodInfo method, Operators op)
        {
            InternalRegisterFunction(method, "", op);
        }

        protected void RegisterConstructor(System.Reflection.ConstructorInfo constructor)
        {
            Register(constructor, new PrimitiveShaderConstructor(this, constructor));
        }

        protected void RegisterField(System.Reflection.FieldInfo field, string name)
        {
            ShaderType type = ResolveType (field.FieldType);

            if (type == null)
                throw new ArgumentException("Can not resolve type " + field.FieldType.Name);

            ShaderField fieldInfo = new PrimitiveShaderField(this, name, field);
            Register(field, fieldInfo);
        }

        protected void RegisterGlobal(System.Reflection.FieldInfo field, string name)
        {
            ShaderGlobal globalInfo = new PrimitiveShaderGlobal(this, name, field);
            Register(field, globalInfo);
        }

        protected void RegisterType(Type type, string name, ShaderFieldAccessResolver accessResolver)
        {
            if (type.IsValueType)
            {
                if (type.IsGenericTypeDefinition)
                {
                    throw new NotSupportedException("Can not register generic definitios nor Arguments.");
                }
                else
                {
                    ShaderType typeInfo = accessResolver == null?
                        new PrimitiveShaderType (this, name, type):
                        new PrimitiveShaderType(this, name, type, accessResolver);
                    Register(type, typeInfo);
                    return;
                }
            }
            throw new NotSupportedException();
        }

        protected void RegisterType(Type type, string name)
        {
            RegisterType(type, name, null);
        }

        public IEnumerable<ShaderType> GetTypes()
        {
            return membersMap.Values.OfType<ShaderType>();
        }

        public IEnumerable<ShaderFunction> GetFunctions()
        {
            return membersMap.Values.OfType<ShaderFunction>();
        }

        public bool ContainsType(string typeName)
        {
            return GetTypes().Any(t => t.Name.Equals(typeName));
        }

        public bool ContainsFunction(string functionName, params ShaderType[] argumentTypes)
        {
            return GetFunctions(functionName, argumentTypes).FirstOrDefault() != null;
        }

        public bool ContainsFunction(string functionName)
        {
            return GetFunctions(functionName).FirstOrDefault() != null;
        }

        public bool ContainsOperator(Operators op, params ShaderType[] argumentTypes)
        {
            throw new NotImplementedException();
        }

        void ImplicitConversions(ShaderType origin, out ShaderType[] conversions, out int[] distances)
        {
            Dictionary<ShaderType, int> types = new Dictionary<ShaderType, int>();
            Queue<ShaderType> q = new Queue<ShaderType>();

            q.Enqueue(origin);

            while (q.Count != 0)
            {
                ShaderType a = q.Dequeue();
                int dist = a.Equals(origin) ? 0 : types[a];

                foreach (ShaderFunction conversion in GetFunctions().Where(f => f.Operator == Operators.Cast && f.Parameters[0].Type.Equals(a)))
                    if (!types.ContainsKey(conversion.ReturnType) && !conversion.ReturnType.Equals(origin))
                    {
                        q.Enqueue(conversion.ReturnType);
                        types.Add(conversion.ReturnType, dist + 1);
                    }
            }
            conversions = types.Keys.ToArray();
            distances = conversions.Select(c => types[c]).ToArray();
        }

        int Distance(ShaderType[] staticParameters, ShaderType[] callingArguments)
        {
            int TotalDistance = 0;

            for (int i = 0; i < staticParameters.Length; i++)
            {
                ShaderType[] conversions;
                int[] distances;
                ImplicitConversions(callingArguments[i], out conversions, out distances);
                int index = conversions.ToList().IndexOf(staticParameters[i]);
                if (index < 0)
                    return int.MaxValue;
                TotalDistance += distances[index];
            }

            return TotalDistance;
        }

        public IEnumerable<ShaderConstructor> GetConstructor(ShaderType type, params ShaderType[] argumentTypes)
        {
            int bestChoice = int.MaxValue;
            List<ShaderConstructor> best = new List<ShaderConstructor> ();

            foreach (ShaderConstructor c in type.Constructors)
            {
                int distance = Distance(c.Parameters.Select(p => p.Type).ToArray(), argumentTypes);
                if (bestChoice > distance)
                {
                    bestChoice = distance;
                    best = new List<ShaderConstructor>() { c };
                }
                else
                    if (bestChoice == distance)
                    {
                        best.Add(c);
                    }
            }

            return best;
        }

        public IEnumerable<ShaderConstructor> GetConstructors()
        {
            return membersMap.Values.OfType<ShaderConstructor>();
        }

        public IEnumerable<ShaderFunction> GetFunctions(string functionName)
        {
            return GetFunctions().Where(f => f.Name.Equals(functionName));
        }

        public IEnumerable<ShaderFunction> GetOperatorOverloads(Operators op)
        {
            return GetFunctions().Where(f => f.Operator == op);
        }

        public IEnumerable<ShaderFunction> GetOperatorOverloads(Operators op, ShaderType[] argumentTypes)
        {
            int bestChoice = int.MaxValue;
            List<ShaderFunction> best = new List<ShaderFunction>();

            foreach (ShaderFunction function in GetOperatorOverloads(op))
            {
                int distance = Distance(function.Parameters.Select(p => p.Type).ToArray(), argumentTypes);
                if (bestChoice > distance)
                {
                    bestChoice = distance;
                    best = new List<ShaderFunction>() { function };
                }
                else
                    if (bestChoice == distance)
                    {
                        best.Add(function);
                    }
            }

            return best;
        }

        public IEnumerable<ShaderFunction> GetFunctions(string functionName, ShaderType[] argumentTypes)
        {
            int bestChoice = int.MaxValue;
            List<ShaderFunction> best = new List<ShaderFunction>();

            foreach (ShaderFunction function in GetFunctions(functionName))
            {
                int distance = Distance(function.Parameters.Select(p => p.Type).ToArray(), argumentTypes);
                if (bestChoice > distance)
                {
                    bestChoice = distance;
                    best = new List<ShaderFunction>() { function };
                }
                else
                    if (bestChoice == distance)
                    {
                        best.Add(function);
                    }
            }

            return best;
        }

        public IEnumerable<ShaderFunction> GetIndexer(ShaderType type, ShaderType[] argumentTypes)
        {
            return GetFunctions("[]", new ShaderType[] { type }.Union(argumentTypes).ToArray());
        }

        public ShaderType GetType(string typeName)
        {
            return GetTypes().FirstOrDefault(t => t.Name.Equals(typeName));
        }

        public bool CanImplicitConvert(ShaderType from, ShaderType to)
        {
            return GetOperatorOverloads(Operators.Cast, new ShaderType[] { from }).FirstOrDefault(op => op.ReturnType.Equals(to)) != null;
        }

        #region Type Definitions

        public ShaderType Int32
        {
            get { return ResolveType(typeof(int)); }
        }
        public ShaderType Float
        {
            get { return ResolveType(typeof(float)); }
        }
        public ShaderType Double
        {
            get { return ResolveType(typeof(double)); }
        }
        public ShaderType Boolean
        {
            get { return ResolveType(typeof(bool)); }
        }
        public ShaderType Void
        {
            get { return ResolveType(typeof(void)); }
        }
        public ShaderType Byte
        {
            get { return ResolveType(typeof(byte)); }
        }
        public ShaderType Int16
        {
            get { return ResolveType(typeof(short)); }
        }
        public ShaderType Int64
        {
            get
            {
                return ResolveType(typeof(long));
            }
        }

        public ShaderType Sampler2D
        {
            get
            {
                return ResolveType(typeof(Sampler2D));
            }
        }

        #endregion

        public virtual IEnumerable<string> Keywords
        {
            get { yield break; }
        }

        public ShaderGlobal GetGlobal(string name)
        {
            return membersMap.Values.OfType<ShaderGlobal>().FirstOrDefault(g => g.Name == name);
        }

        public ShaderType Vector(ShaderType elementType, int components)
        {
            if (!elementType.IsScalar)
                throw new NotSupportedException();

            Type netType = ((PrimitiveShaderType)elementType).NetType;

            switch (components)
            {
                case 1: return ResolveType(typeof(Vector1<>).MakeGenericType(netType));
                case 2: return ResolveType(typeof(Vector2<>).MakeGenericType(netType));
                case 3: return ResolveType(typeof(Vector3<>).MakeGenericType(netType));
                case 4: return ResolveType(typeof(Vector4<>).MakeGenericType(netType));
            }

            throw new ArgumentOutOfRangeException();
        }

        public string AssemblyName
        {
            get
            {
                string assemblyName = typeof(IRenderer).AssemblyQualifiedName;
                int comaPos = assemblyName.IndexOf(",");
                if (comaPos < 0)
                    return "";
                else
                    return assemblyName.Substring(comaPos + 1);
            }
        }

        public ShaderType Matrix(ShaderType elementType, int rows, int cols)
        {
            string assemblyInfo = AssemblyName;
            Type baseType = ((PrimitiveShaderType)elementType).NetType;
            Type genericMatrixType = Type.GetType(string.Format("System.Rendering.Matrix{0}x{1}`1, " + assemblyInfo, rows, cols));
            Type matrixType = genericMatrixType.MakeGenericType(baseType);
            return ResolveType(matrixType);
        }

        public object Invoke(ShaderType type, string fieldAccess, object target)
        {
            if (type.IsPrimitive)
                return type.AccessResolver.GetAccessValue(target, fieldAccess);

            throw new NotSupportedException();
        }
    }

    internal class EmptyBuiltins : Builtins
    {
    }

    public enum Operators
    {
        None,
        Addition,
        Subtraction,
        Multiply,
        Division,
        Modulus,
        Equality,
        Inequality,
        Cast,
        LessThan,
        LessThanOrEquals,
        GreaterThan,
        GreaterThanOrEquals,
        LogicOr,
        LogicAnd,
        LogicXor,
        ConditionalOr,
        ConditionalAnd,
        Not,
        UnaryPlus,
        UnaryNegation,
        TernaryDecision,
        PreIncrement,
        PreDecrement,
        PostIncrement,
        PostDecrement,
        Indexer
    }
}
