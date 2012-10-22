using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Compilers.Shaders.Info;
using System.Compilers.Shaders.ShaderAST;
using System.Maths;

namespace System.Compilers.Shaders
{
    /// <summary>
    /// Allows to performe evaluation within a primitive scope of certain Language.
    /// Represents the basic field, methods, operators and types.
    /// </summary>
    public abstract class Builtins
    {
        #region Static Methods and Properties

        public static Builtins Empty { get; private set; }

        static Builtins()
        {
            Empty = new EmptyBuiltins();
        }

        #region Primitive types operations

        #region Operations

        #region None
        #endregion
        #region Addition
        protected static byte op_Addition(byte x, byte y) { return (byte)(x + y); }
        protected static int op_Addition(int x, int y) { return (int)(x + y); }
        protected static float op_Addition(float x, float y) { return (float)(x + y); }
        protected static short op_Addition(short x, short y) { return (short)(x + y); }
        #endregion
        #region Subtraction
        protected static byte op_Subtraction(byte x, byte y) { return (byte)(x - y); }
        protected static int op_Subtraction(int x, int y) { return (int)(x - y); }
        protected static float op_Subtraction(float x, float y) { return (float)(x - y); }
        protected static short op_Subtraction(short x, short y) { return (short)(x - y); }
        #endregion
        #region Multiply
        protected static byte op_Multiply(byte x, byte y) { return (byte)(x * y); }
        protected static int op_Multiply(int x, int y) { return (int)(x * y); }
        protected static float op_Multiply(float x, float y) { return (float)(x * y); }
        protected static short op_Multiply(short x, short y) { return (short)(x * y); }
        #endregion
        #region Division
        protected static byte op_Division(byte x, byte y) { return (byte)(x / y); }
        protected static int op_Division(int x, int y) { return (int)(x / y); }
        protected static float op_Division(float x, float y) { return (float)(x / y); }
        protected static short op_Division(short x, short y) { return (short)(x / y); }
        #endregion
        #region Modulus
        protected static byte op_Modulus(byte x, byte y) { return (byte)(x % y); }
        protected static int op_Modulus(int x, int y) { return (int)(x % y); }
        protected static float op_Modulus(float x, float y) { return (float)(x % y); }
        protected static short op_Modulus(short x, short y) { return (short)(x % y); }
        #endregion
        #region Equality
        protected static bool op_Equality(byte x, byte y) { return (bool)(x == y); }
        protected static bool op_Equality(int x, int y) { return (bool)(x == y); }
        protected static bool op_Equality(float x, float y) { return (bool)(x == y); }
        protected static bool op_Equality(short x, short y) { return (bool)(x == y); }
        protected static bool op_Equality(bool x, bool y) { return (bool)(x == y); }
        #endregion
        #region Inequality
        protected static bool op_Inequality(byte x, byte y) { return (bool)(x != y); }
        protected static bool op_Inequality(int x, int y) { return (bool)(x != y); }
        protected static bool op_Inequality(float x, float y) { return (bool)(x != y); }
        protected static bool op_Inequality(short x, short y) { return (bool)(x != y); }
        protected static bool op_Inequality(bool x, bool y) { return (bool)(x != y); }
        #endregion
        #region Cast
        #endregion
        #region LessThan
        protected static bool op_LessThan(byte x, byte y) { return (bool)(x < y); }
        protected static bool op_LessThan(int x, int y) { return (bool)(x < y); }
        protected static bool op_LessThan(float x, float y) { return (bool)(x < y); }
        protected static bool op_LessThan(short x, short y) { return (bool)(x < y); }
        #endregion
        #region LessThanOrEquals
        protected static bool op_LessThanOrEquals(byte x, byte y) { return (bool)(x <= y); }
        protected static bool op_LessThanOrEquals(int x, int y) { return (bool)(x <= y); }
        protected static bool op_LessThanOrEquals(float x, float y) { return (bool)(x <= y); }
        protected static bool op_LessThanOrEquals(short x, short y) { return (bool)(x <= y); }
        #endregion
        #region GreaterThan
        protected static bool op_GreaterThan(byte x, byte y) { return (bool)(x > y); }
        protected static bool op_GreaterThan(int x, int y) { return (bool)(x > y); }
        protected static bool op_GreaterThan(float x, float y) { return (bool)(x > y); }
        protected static bool op_GreaterThan(short x, short y) { return (bool)(x > y); }
        #endregion
        #region GreaterThanOrEquals
        protected static bool op_GreaterThanOrEquals(byte x, byte y) { return (bool)(x >= y); }
        protected static bool op_GreaterThanOrEquals(int x, int y) { return (bool)(x >= y); }
        protected static bool op_GreaterThanOrEquals(float x, float y) { return (bool)(x >= y); }
        protected static bool op_GreaterThanOrEquals(short x, short y) { return (bool)(x >= y); }
        #endregion
        #region LogicOr
        protected static byte op_LogicOr(byte x, byte y) { return (byte)(x | y); }
        protected static int op_LogicOr(int x, int y) { return (int)(x | y); }
        protected static short op_LogicOr(short x, short y) { return (short)(x | y); }
        #endregion
        #region LogicAnd
        protected static byte op_LogicAnd(byte x, byte y) { return (byte)(x & y); }
        protected static int op_LogicAnd(int x, int y) { return (int)(x & y); }
        protected static short op_LogicAnd(short x, short y) { return (short)(x & y); }
        #endregion
        #region LogicXor
        protected static byte op_LogicXor(byte x, byte y) { return (byte)(x ^ y); }
        protected static int op_LogicXor(int x, int y) { return (int)(x ^ y); }
        protected static short op_LogicXor(short x, short y) { return (short)(x ^ y); }
        #endregion
        #region ConditionalOr
        protected static bool op_ConditionalOr(bool x, bool y) { return (bool)(x || y); }
        #endregion
        #region ConditionalAnd
        protected static bool op_ConditionalAnd(bool x, bool y) { return (bool)(x && y); }
        #endregion
        #region Not
        protected static bool op_Not(bool x) { return (bool)!(x); }
        #endregion
        #region UnaryPlus
        #endregion
        #region UnaryNegation
        #endregion
        #region TernaryDecision
        #endregion
        #region PreIncrement
        #endregion
        #region PreDecrement
        #endregion
        #region PostIncrement
        #endregion
        #region PostDecrement
        #endregion
        #region Indexer
        #endregion

        #endregion


        #endregion

        #endregion

        #region Instances

        internal Dictionary<MemberInfo, ShaderMember> membersMap = new Dictionary<MemberInfo, ShaderMember>();
        internal Dictionary<ShaderMember, MemberInfo> shadersMap = new Dictionary<ShaderMember, MemberInfo>();
        private IBinder binder;

        #endregion

        #region Constructors
        
        protected Builtins()
        {
            this.binder = new DefaultBinder(shadersMap.Keys.OfType<ShaderMethodBase>());
        }

        protected void EndInitialization()
        {
            if (this.binder is DefaultBinder)
                ((DefaultBinder)binder).UpdateConversions();
        }

        #endregion

        /// <summary>
        /// Determines if some member of a shader is supported by this builtins.
        /// </summary>
        public bool Contains(ShaderMember member)
        {
            if (member == null)
                throw new ArgumentNullException("member");

            return shadersMap.ContainsKey(member);
        }

        /// <summary>
        /// Determines if some member of a .net program is supported by this builtins.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public bool Contains(MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException("member");

            return membersMap.ContainsKey (member);
        }

        /// <summary>
        /// Get access to all shader members supported natively.
        /// </summary>
        public IEnumerable<ShaderMember> Globals
        {
            get { return shadersMap.Keys.Where(m => m.DeclaringType == null); }
        }

        #region Registers

        /// <summary>
        /// Allows to map certain .net member with a shader member and viceversa.
        /// </summary>
        internal void Register(System.Reflection.MemberInfo netMember, ShaderMember shaderMember)
        {
            if (!membersMap.ContainsKey(netMember))
                membersMap[netMember] = shaderMember;
            if (!shadersMap.ContainsKey(shaderMember))
                shadersMap[shaderMember] = netMember;
        }

        /// <summary>
        /// Allows to take decisitions during adding the members of a type.
        /// </summary>
        public class MemberRegistration
        {
            /// <summary>
            /// Gets the current member is being mapped.
            /// </summary>
            public MemberInfo Member { get; internal set; }

            public bool IsConstructor { get { return Member is ConstructorInfo; } }

            public bool IsMethod { get { return Member is MethodInfo && !Member.Name.Contains("op_"); } }

            public bool IsField { get { return Member is FieldInfo; } }

            public bool IsOperator { get { return Member is MethodInfo && Member.Name.Contains("op_"); } }

            /// <summary>
            /// Gets or sets the final name this member should have.
            /// </summary>
            public string Name { get; set; }

            public MemberRegistration(MemberInfo member)
            {
                this.Member = member;
                this.Name = this.Member.Name;
            }
        }

        internal protected void RegisterMembers(Type type, Func<MemberRegistration, bool> membering)
        {
            var t = Resolve(type);
            foreach (var member in type.GetMembers())
            {
                MemberRegistration registration = new MemberRegistration(member);
                if (membering != null && membering(registration))
                {
                    if (registration.IsConstructor)
                        Register(registration.Member as ConstructorInfo, t);

                    if (registration.IsField)
                        Register(registration.Member as FieldInfo, registration.Name, t);

                    if (registration.IsMethod)
                        Register(registration.Member as MethodInfo, registration.Name, t);

                    if (registration.IsOperator)
                        Register(registration.Member as MethodInfo, registration.Name, t);
                }
            }
        }

        /// <summary>
        /// Register a type in this builtins under a specific name and determines wich members should be mapped as well.
        /// </summary>
        /// <param name="membering">A function to determine wich member of the type should be added. It should return true if the member needs to be mapped.</param>
        /// <returns></returns>
        internal protected ShaderType Register(Type type, string name, ShaderType declaringType)
        {
            BuiltinShaderType t = new BuiltinShaderType(this, type, name, declaringType);

            Register(type, t);

            return t;
        }

        /// <summary>
        /// Registers a method as a primitive function of the builtins.
        /// </summary>
        internal protected ShaderMethod Register(MethodInfo method, string name, ShaderType declaringType)
        {
            BuiltinShaderMethod m = new BuiltinShaderMethod(this, method, name, declaringType);

            Register(method, m);

            return m;
        }
        
        /// <summary>
        /// Registers a field as a primitive fied of the builtins.
        /// </summary>
        internal protected ShaderField Register(FieldInfo field, string name, ShaderType declaringType)
        {
            BuiltinShaderField f = new BuiltinShaderField(this, field, name, declaringType);

            Register(field, f);

            return f;
        }

        /// <summary>
        /// Registers a constructor as a primitive constructor of the builtins.
        /// </summary>
        internal protected ShaderConstructor Register(ConstructorInfo constructor, ShaderType declaringType)
        {
            BuiltinShaderConstructor c = new BuiltinShaderConstructor(this, constructor, declaringType);

            Register(constructor, c);

            return c;
        }

        #endregion

        #region Resolves

        #region Members

        /// <summary>
        /// Resolves any shader member present as builtin
        /// </summary>
        public ShaderMember Resolve(MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException("member");

            if (membersMap.ContainsKey(member))
                return membersMap[member];
            
            throw new ArgumentOutOfRangeException("Member " + member.Name + " is not a builtin member.");
        }

        /// <summary>
        /// Resolves any net member present as builtin
        /// </summary>
        public MemberInfo Resolve(ShaderMember member)
        {
            if (member == null)
                throw new ArgumentNullException("member");

            if (shadersMap.ContainsKey(member))
                return shadersMap[member];
            
            throw new ArgumentOutOfRangeException("Member " + member.Name + " is not a builtin member.");
        }

        #endregion

        #region Types

        /// <summary>
        /// Resolves a Type as a ShaderType defined as builtin..
        /// </summary>
        public ShaderType Resolve(Type type)
        {
            return (ShaderType)Resolve((MemberInfo)type);
        }

        /// <summary>
        /// Resolves a buitin ShaderType as a .net type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Type Resolve(ShaderType type)
        {
            return (Type)Resolve((ShaderMember)type);
        }

        /// <summary>
        /// Gets the boolean ShaderType of this builtins.
        /// </summary>
        public ShaderType Boolean { get { return Resolve(typeof(bool)); } }

        /// <summary>
        /// Gets the void ShaderType of this builtins.
        /// </summary>
        public ShaderType Void { get { return Resolve(typeof(void)); } }

        /// <summary>
        /// Gets the object ShaderType of this builtins.
        /// </summary>
        public ShaderType Object { get { return Resolve(typeof(object)); } }

        /// <summary>
        /// Gets the int ShaderType of this builtins.
        /// </summary>
        public ShaderType Int { get { return Resolve(typeof(int)); } }

        /// <summary>
        /// Gets the short ShaderType of this builtins.
        /// </summary>
        public ShaderType Short { get { return Resolve(typeof(short)); } }
        
        /// <summary>
        /// Gets the byte ShaderType of this builtins.
        /// </summary>
        public ShaderType Byte { get { return Resolve(typeof(byte)); } }

        /// <summary>
        /// Gets the float ShaderType of this builtins.
        /// </summary>
        public ShaderType Float { get { return Resolve(typeof(float)); } }

        /// <summary>
        /// Gets a ShaderType by its name.
        /// </summary>
        public virtual ShaderType GetType(string name)
        {
            var t = shadersMap.Keys.OfType<ShaderType>().FirstOrDefault(s => s.Name == name) as ShaderType;

            if (t == null)
                throw new ArgumentOutOfRangeException("Type " + name + " is not present as builtins");

            return t;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resolves a .net field as a ShaderField builtin.
        /// </summary>
        public ShaderField Resolve(FieldInfo field)
        {
            return (ShaderField)Resolve((MemberInfo)field);
        }

        /// <summary>
        /// Resolves the .net field corresponding to a ShaderField at this builtins.
        /// </summary>
        public FieldInfo Resolve(ShaderField field)
        {
            return (FieldInfo)Resolve((ShaderMember)field);
        }

        /// <summary>
        /// Resolves a .net method as a ShaderMethod builtin.
        /// </summary>
        public ShaderMethod Resolve(MethodInfo method)
        {
            return (ShaderMethod)Resolve((MemberInfo)method);
        }

        public ShaderConstructor Resolve(ConstructorInfo constructor)
        {
            return (ShaderConstructor)Resolve((MemberInfo)constructor);
        }

        /// <summary>
        /// Resolves the .net method corresponding to a ShaderMethod at this builtins.
        /// </summary>
        public MethodInfo Resolve(ShaderMethod method)
        {
            return (MethodInfo)Resolve((ShaderMember)method);
        }

        public ConstructorInfo Resolve(ShaderConstructor constructor)
        {
            return (ConstructorInfo)Resolve((ShaderMember)constructor);
        }

        /// <summary>
        /// Gets the conversion method that allows to covert from one type to another within this builtins.
        /// </summary>
        public ShaderMethod GetConversion(ShaderType fromType, ShaderType toType)
        {
            return binder.Match(Operators.Cast, fromType).FirstOrDefault(m => m.ReturnType == toType || m.ReturnType.IsSubclass(toType));
        }

        /// <summary>
        /// Gets the method that best fits for one operation and several arguments.
        /// </summary>
        public ShaderMethod GetBestOverload(Operators Operator, ShaderType[] arguments)
        {
            var overloads = binder.BestOverloads(binder.Match(Operator, arguments).Cast<ShaderMethodBase>(), arguments);

            if (overloads.Length == 0)
                throw new InvalidOperationException("No overloads found");

            if (overloads.Length != 1)
                throw new InvalidOperationException("Ambiguous call");

            return overloads[0] as ShaderMethod;
        }

        /// <summary>
        /// Gets the method that best fits for a method invocation and several arguments.
        /// </summary>
        public ShaderMethod GetBestOverload(ShaderType type, string name, ShaderType[] arguments)
        {
            var overloads = binder.BestOverloads(binder.Match(type, name, arguments).Cast<ShaderMethodBase>(), arguments);

            if (overloads.Length == 0)
                throw new InvalidOperationException("No overloads found");

            if (overloads.Length != 1)
                throw new InvalidOperationException("Ambiguous call");

            return overloads[0] as ShaderMethod;
        }

        /// <summary>
        /// Gets the constructor better fits the creation of certain type with several arguments.
        /// </summary>
        public ShaderConstructor GetBestOverload(ShaderType type, ShaderType[] arguments)
        {
            var overloads = binder.BestOverloads(binder.Match(type, arguments).Cast<ShaderMethodBase>(), arguments);

            if (overloads.Length == 0)
                throw new InvalidOperationException("No overloads found");

            if (overloads.Length != 1)
                throw new InvalidOperationException("Ambiguous call");

            return overloads[0] as ShaderConstructor;
        }

        #endregion

        /// <summary>
        /// Gets the name of one member of the builtins.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public virtual string GetName(ShaderMember member)
        {
            if (Contains(member))
                return membersMap[shadersMap[member]].Name;

            throw new ArgumentOutOfRangeException();
        }

        #endregion

        /// <summary>
        /// When overriden Gets a secuence of keywords of the language.
        /// </summary>
        public virtual IEnumerable<string> Keywords
        {
            get
            {
                yield break;
            }
        }

        public object Eval(Operators op, params object[] arguments)
        {
            return Resolve(GetBestOverload(op, arguments.Select(a => Resolve(a.GetType())).ToArray())).Invoke(null, arguments);
        }

        public object Eval(string methodName, object target, params object[] arguments)
        {
            if (target == null)
                return Resolve(GetBestOverload(null, methodName, arguments.Select(a => Resolve(a.GetType())).ToArray())).Invoke(null, arguments);
            else
                return Resolve(GetBestOverload(Resolve(target.GetType()), methodName, arguments.Select(a => Resolve(a.GetType())).ToArray())).Invoke(target, arguments);
        }

        public object CreateInstance(ShaderType shaderType, params object[] arguments)
        {
            return Resolve(GetBestOverload(shaderType, arguments.Select(a => Resolve(a.GetType())).ToArray())).Invoke(arguments);
        }
    }

    internal class EmptyBuiltins : Builtins
    {
    }

    public static class BuiltinsExtensors
    {
        public static void FullRegisterScalarMembers(this Builtins builtins, Type type)
        {
            if (!type.IsPrimitive)
                throw new ArgumentException("Can not treat type " + type.Name + " as scalar because is not primitive type.");

            List<Type> typeConversionOrder = new List<Type> {
                typeof (bool),
                typeof (byte),
                typeof (short),
                typeof (int),
                typeof (long),
                typeof (float),
                typeof (double)
            };

            var conversionsFrom =
            typeof(Convert).GetMethods(BindingFlags.Static | BindingFlags.Public).Where(m => m.Name.StartsWith("To") && m.GetParameters().Length == 1 &&
            m.GetParameters()[0].ParameterType == type && builtins.membersMap.ContainsKey(m.ReturnType));

            foreach (var m in conversionsFrom)
            {
                int fromIndex = typeConversionOrder.IndexOf(m.GetParameters()[0].ParameterType);
                int toIndex = typeConversionOrder.IndexOf(m.ReturnType);
                builtins.Register(m, new BuiltinShaderMethod(builtins, m, fromIndex > toIndex ? "op_Implicit" : "op_Cast", null));
            }

            var conversionsTo =
            typeof(Convert).GetMethods(BindingFlags.Static | BindingFlags.Public).Where(m => m.Name.StartsWith("To") && m.GetParameters().Length == 1 &&
            builtins.membersMap.ContainsKey(m.GetParameters()[0].ParameterType) && m.ReturnType == type);

            foreach (var m in conversionsTo)
            {
                int fromIndex = typeConversionOrder.IndexOf(m.GetParameters()[0].ParameterType);
                int toIndex = typeConversionOrder.IndexOf(m.ReturnType);
                builtins.Register(m, new BuiltinShaderMethod(builtins, m, fromIndex > toIndex ? "op_Implicit" : "op_Cast", null));
            }

            // full operations support

            Type builtinType = builtins.GetType();

            foreach (var op in Enum.GetValues(typeof(Operators)).Cast<Operators>())
            {
                var method = 
                    builtinType.GetMethod("op_" + op, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Type.DefaultBinder, new Type[] { type }, null) ??
                    builtinType.GetMethod("op_" + op, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Type.DefaultBinder, new Type[] { type, type }, null);

                if (method != null)
                    builtins.Register(method, "op_" + op, null);
            }
        }

        public static void FullRegisterScalarMembers<T>(this Builtins builtins)
        {
            FullRegisterScalarMembers(builtins, typeof(T));
        }

        public static void RegisterAvailableOverloads(this Builtins builtins, Type target, string methodName, string shaderMethodName)
        {
            foreach (var method in target.GetMethods().Where(m => m.Name.Equals(methodName)))
            {
                bool allTypePresent = true;
                foreach (var p in method.GetParameters())
                {
                    if (builtins.Contains(p.ParameterType))
                    {
                        ShaderType type = builtins.Resolve(p.ParameterType);
                    }
                    else
                    {
                        allTypePresent = false;
                        break;
                    }
                }

                if (allTypePresent)
                    builtins.Register(method, shaderMethodName, null);
            }
        }
    }

    
}
