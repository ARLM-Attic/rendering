using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.AST;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;

namespace System.Compilers.IL
{
    internal class TypeAnalysis
    {
        List<ILExpression> storedToGeneratedVariables = new List<ILExpression>();
        HashSet<NetLocalVariable> inferredVariables = new HashSet<NetLocalVariable>();
        List<ILExpression> methodBody;
        MethodBase methodInfo;
        List<ILExpression> allExpresions;

        public static void Run(MethodBase methodInfo, List<ILNode> methodBody)
        {
            var exprBody = methodBody.Where((e) => e is ILExpression).Cast<ILExpression>().ToList();
            TypeAnalysis ta = new TypeAnalysis(methodInfo, exprBody);
            foreach (var item in exprBody)
            {
                ta.InferTypes(item);   
            }
            ta.InferRemainingStores();
        }

        private TypeAnalysis(MethodBase methodInfo, List<ILExpression> methodBody)
        {
            this.methodInfo = methodInfo;
            this.methodBody = methodBody;
        }

        void InferTypes(ILExpression expr)
        {

            NetLocalVariable v = expr.Operand as NetLocalVariable;
            if (v != null && v.IsGenerated && v.Type == null && expr.Code == OpCodeCodes.Stloc && !inferredVariables.Contains(v) && HasSingleLoad(v))
            {
                // Don't deal with this node or its children yet,
                // wait for the expected type to be inferred first.
                // This happens with the arg_... variables introduced by the ILAst - we skip inferring the whole statement,
                // and first infer the statement that reads from the arg_... variable.
                // The ldloc inference will write the expected type to the variable, and the next InferRemainingStores() pass
                // will then infer this statement with the correct expected type.
                storedToGeneratedVariables.Add(expr);
                return;
            }
            bool anyArgumentIsMissingType = expr.Arguments.Any(a => a.InferredType == null);
            if (expr.InferredType == null || anyArgumentIsMissingType)
                expr.InferredType = InferTypeForExpression(expr, expr.ExpectedType, forceInferChildren: anyArgumentIsMissingType);
            
            foreach (var child in expr.Arguments)
                InferTypes(child);
        }

        bool HasSingleLoad(NetLocalVariable v)
        {
            int loads = 0;
            foreach (ILExpression expr in GetAllILExpressions(methodBody).ToList())
            {
                if (expr.Operand == v)
                {
                    if (expr.Code == OpCodeCodes.Ldloc)
                        loads++;
                    else if (expr.Code != OpCodeCodes.Stloc)
                        return false;
                }
            }
            return loads == 1;
        }

        void InferRemainingStores()
        {
            while (storedToGeneratedVariables.Count > 0)
            {
                List<ILExpression> stored = storedToGeneratedVariables;
                storedToGeneratedVariables = new List<ILExpression>();
                foreach (ILExpression expr in stored)
                    InferTypes(expr);
                if (!(storedToGeneratedVariables.Count < stored.Count))
                    throw new InvalidOperationException("Infinite loop in type analysis detected.");
            }
        }

        private IEnumerable<ILExpression> GetAllILExpressions(List<ILExpression> method)
        {
            if (allExpresions == null)
            {
                allExpresions = new List<ILExpression>();
                for (int i = 0; i < method.Count; i++)
                    foreach (var item in GetSelfAndChildren(method[i]))
                        allExpresions.Add(item);
            }
            return allExpresions;
        }

        private IEnumerable<ILExpression> GetSelfAndChildren(ILExpression expr)
        {
            yield return expr;
            foreach (var item in expr.Arguments)
                foreach (var desc in GetSelfAndChildren(item))
                    yield return desc;
        }

        Type InferTypeForExpression(ILExpression expr, Type expectedType, bool forceInferChildren = false)
        {
            expr.ExpectedType = expectedType;
            if (forceInferChildren || expr.InferredType == null)
                expr.InferredType = DoInferTypeForExpression(expr, expectedType, forceInferChildren);
            return expr.InferredType;
        }

        private Type DoInferTypeForExpression(ILExpression expr, Type expectedType, bool forceInferChildren)
        {
            switch (expr.Code)
            {
                #region Variable load/store
                case OpCodeCodes.Stloc:
                    {
                        NetLocalVariable v = (NetLocalVariable)expr.Operand;
                        if (forceInferChildren || v.Type == null)
                        {
                            Type t = InferTypeForExpression(expr.Arguments.Single(), ((NetLocalVariable)expr.Operand).Type);
                            if (v.Type == null)
                                v.Type = t;
                        }
                        return v.Type;
                    }
                case OpCodeCodes.Ldloc:
                    {
                        NetLocalVariable v = (NetLocalVariable)expr.Operand;
                        if (v.Type == null)
                        {
                            v.Type = expectedType;
                            // Mark the variable as inferred. This is necessary because expectedType might be null
                            // (e.g. the only use of an arg_*-Variable is a pop statement),
                            // so we can't tell from v.Type whether it was already inferred.
                            inferredVariables.Add(v);
                        }
                        return v.Type;
                    }
                case OpCodeCodes.Starg:
                    if (forceInferChildren)
                        InferTypeForExpression(expr.Arguments.Single(), ((ParameterInfo)expr.Operand).ParameterType);
                    return null;
                case OpCodeCodes.Ldarg:
                    return ((ParameterInfo)expr.Operand).ParameterType;
                case OpCodeCodes.Ldloca:
                    return ((NetLocalVariable)expr.Operand).Type.MakeByRefType();
                case OpCodeCodes.Ldarga:
                    return ((ParameterInfo)expr.Operand).ParameterType.MakeByRefType();;
                #endregion
                #region Call / NewObj
                case OpCodeCodes.Call:
                case OpCodeCodes.Callvirt:
                    {
                        MethodBase method = (MethodBase)expr.Operand;
                        if (forceInferChildren)
                        {
                            for (int i = 0; i < expr.Arguments.Count; i++)
                            {
                                if (i == 0 && !method.IsStatic)
                                {
                                    ILInstruction constraint = expr.GetPrefix(OpCodes.Constrained);
                                    if (constraint != null)
                                        InferTypeForExpression(expr.Arguments[i], ((Type)constraint.Operand).MakeByRefType());
                                    else
                                        InferTypeForExpression(expr.Arguments[i], method.DeclaringType);
                                }
                                else
                                {
                                    InferTypeForExpression(expr.Arguments[i], SubstituteTypeArgs(method.GetParameters()[!method.IsStatic ? i - 1 : i].ParameterType, method));
                                }
                            }
                        }
                        return SubstituteTypeArgs(method.IsConstructor ? method.DeclaringType : ((MethodInfo)method).ReturnType, method);
                    }
                case OpCodeCodes.Newobj:
                    {
                        ConstructorInfo ctor = (ConstructorInfo)expr.Operand;
                        if (forceInferChildren)
                        {
                            for (int i = 0; i < ctor.GetParameters().Length; i++)
                            {
                                InferTypeForExpression(expr.Arguments[i], SubstituteTypeArgs(ctor.GetParameters()[i].ParameterType, ctor));
                            }
                        }
                        return ctor.DeclaringType;
                    }
                #endregion
                #region Load/Store Fields
                case OpCodeCodes.Ldfld:
                    if (forceInferChildren)
                        InferTypeForExpression(expr.Arguments[0], ((FieldInfo)expr.Operand).DeclaringType);
                    return GetFieldType((FieldInfo)expr.Operand);
                case OpCodeCodes.Ldsfld:
                    return GetFieldType((FieldInfo)expr.Operand);
                case OpCodeCodes.Ldflda:
                case OpCodeCodes.Ldsflda:
                    return (GetFieldType((FieldInfo)expr.Operand)).MakeByRefType();
                case OpCodeCodes.Stfld:
                    if (forceInferChildren)
                    {
                        InferTypeForExpression(expr.Arguments[0], ((FieldInfo)expr.Operand).DeclaringType);
                        InferTypeForExpression(expr.Arguments[1], GetFieldType((FieldInfo)expr.Operand));
                    }
                    return null;
                case OpCodeCodes.Stsfld:
                    if (forceInferChildren)
                        InferTypeForExpression(expr.Arguments[0], GetFieldType((FieldInfo)expr.Operand));
                    return null;
                #endregion
                #region Reference/Pointer instructions
                case OpCodeCodes.Ldind_I:
                case OpCodeCodes.Ldind_I1:
                case OpCodeCodes.Ldind_I2:
                case OpCodeCodes.Ldind_I4:
                case OpCodeCodes.Ldind_I8:
                case OpCodeCodes.Ldind_U1:
                case OpCodeCodes.Ldind_U2:
                case OpCodeCodes.Ldind_U4:
                case OpCodeCodes.Ldind_R4:
                case OpCodeCodes.Ldind_R8:
                case OpCodeCodes.Ldind_Ref:
                    return UnpackPointer(InferTypeForExpression(expr.Arguments[0], null));
                case OpCodeCodes.Stind_I1:
                case OpCodeCodes.Stind_I2:
                case OpCodeCodes.Stind_I4:
                case OpCodeCodes.Stind_I8:
                case OpCodeCodes.Stind_R4:
                case OpCodeCodes.Stind_R8:
                case OpCodeCodes.Stind_I:
                case OpCodeCodes.Stind_Ref:
                    if (forceInferChildren)
                    {
                        Type elementType = UnpackPointer(InferTypeForExpression(expr.Arguments[0], null));
                        InferTypeForExpression(expr.Arguments[1], elementType);
                    }
                    return null;
                case OpCodeCodes.Ldobj:
                    return (Type)expr.Operand;
                case OpCodeCodes.Stobj:
                    if (forceInferChildren)
                    {
                        InferTypeForExpression(expr.Arguments[1], (Type)expr.Operand);
                    }
                    return null;
                case OpCodeCodes.Initobj:
                    return null;
                case OpCodeCodes.Localloc:
                    return typeof(IntPtr);
                #endregion
                #region Arithmetic instructions
                case OpCodeCodes.Not: // bitwise complement
                case OpCodeCodes.Neg:
                    return InferTypeForExpression(expr.Arguments.Single(), expectedType);
                case OpCodeCodes.Add:
                case OpCodeCodes.Sub:
                case OpCodeCodes.Mul:
                case OpCodeCodes.Or:
                case OpCodeCodes.And:
                case OpCodeCodes.Xor:
                    return InferArgumentsInBinaryOperator(expr, null);
                case OpCodeCodes.Add_Ovf:
                case OpCodeCodes.Sub_Ovf:
                case OpCodeCodes.Mul_Ovf:
                case OpCodeCodes.Div:
                case OpCodeCodes.Rem:
                    return InferArgumentsInBinaryOperator(expr, true);
                case OpCodeCodes.Add_Ovf_Un:
                case OpCodeCodes.Sub_Ovf_Un:
                case OpCodeCodes.Mul_Ovf_Un:
                case OpCodeCodes.Div_Un:
                case OpCodeCodes.Rem_Un:
                    return InferArgumentsInBinaryOperator(expr, false);
                case OpCodeCodes.Shl:
                case OpCodeCodes.Shr:
                    if (forceInferChildren)
                        InferTypeForExpression(expr.Arguments[1], typeof(int));
                    return InferTypeForExpression(expr.Arguments[0], typeof(int));
                case OpCodeCodes.Shr_Un:
                    if (forceInferChildren)
                        InferTypeForExpression(expr.Arguments[1], typeof(int));
                    return InferTypeForExpression(expr.Arguments[0], typeof(int));
                #endregion
                #region Constant loading instructions
                case OpCodeCodes.Ldnull:
                    return typeof(object);
                case OpCodeCodes.Ldstr:
                    return typeof(string);
                case OpCodeCodes.Ldftn:
                case OpCodeCodes.Ldvirtftn:
                    return typeof(IntPtr);
                case OpCodeCodes.Ldc_I4:
                    if (IsBoolean(expectedType) && ((int)expr.Operand == 0 || (int)expr.Operand == 1))
                        return typeof(bool);
                    return IsIntegerOrEnum(expectedType) ? expectedType : typeof(Int32);
                case OpCodeCodes.Ldc_I8:
                    return (IsIntegerOrEnum(expectedType)) ? expectedType : typeof(Int64);
                case OpCodeCodes.Ldc_R4:
                    return typeof(float);
                case OpCodeCodes.Ldc_R8:
                    return typeof(double);
                //case OpCodeCodes.Ldc_Decimal:
                //    Debug.Assert(expr.InferredType != null && expr.InferredType.FullName == "System.Decimal");
                //    return expr.InferredType;
                //case OpCodeCodes.Ldtoken:
                //    if (expr.Operand is Type)
                //        return new TypeReference("System", "RuntimeTypeHandle", module, module, true);
                //    else if (expr.Operand is FieldReference)
                //        return new TypeReference("System", "RuntimeFieldHandle", module, module, true);
                //    else
                //        return new TypeReference("System", "RuntimeMethodHandle", module, module, true);
                //case OpCodeCodes.Arglist:
                //    return new TypeReference("System", "RuntimeArgumentHandle", module, module, true);
                #endregion
                #region Array instructions
                case OpCodeCodes.Newarr:
                    if (forceInferChildren)
                        InferTypeForExpression(expr.Arguments.Single(), typeof(Int32));
                    return ((Type)expr.Operand).MakeArrayType();
                case OpCodeCodes.Ldlen:
                    return typeof(Int32);
                case OpCodeCodes.Ldelem_U1:
                case OpCodeCodes.Ldelem_U2:
                case OpCodeCodes.Ldelem_U4:
                case OpCodeCodes.Ldelem_I1:
                case OpCodeCodes.Ldelem_I2:
                case OpCodeCodes.Ldelem_I4:
                case OpCodeCodes.Ldelem_I8:
                case OpCodeCodes.Ldelem_I:
                case OpCodeCodes.Ldelem_Ref:
                    {
                        Type arrayType = InferTypeForExpression(expr.Arguments[0], null) ;
                        if (forceInferChildren)
                        {
                            InferTypeForExpression(expr.Arguments[0], typeof(Byte[]));
                            InferTypeForExpression(expr.Arguments[1], typeof(Int32));
                        }
                        return arrayType != null ? arrayType.GetElementType() : null;
                    }
                case OpCodeCodes.Ldelema:
                    {
                        Type arrayType = InferTypeForExpression(expr.Arguments[0], null);
                        if (forceInferChildren)
                            InferTypeForExpression(expr.Arguments[1], typeof(Int32));
                        return arrayType != null ? arrayType.GetElementType().MakeByRefType() : null;
                    }
                case OpCodeCodes.Stelem_I:
                case OpCodeCodes.Stelem_I1:
                case OpCodeCodes.Stelem_I2:
                case OpCodeCodes.Stelem_I4:
                case OpCodeCodes.Stelem_I8:
                case OpCodeCodes.Stelem_R4:
                case OpCodeCodes.Stelem_R8:
                case OpCodeCodes.Stelem_Ref:
                #endregion
                #region Conversion instructions
                case OpCodeCodes.Conv_I1:
                case OpCodeCodes.Conv_Ovf_I1:
                    return (GetInformationAmount(expectedType) == 8 && IsSigned(expectedType) == true) ? expectedType : typeof(SByte);
                case OpCodeCodes.Conv_I2:
                case OpCodeCodes.Conv_Ovf_I2:
                    return (GetInformationAmount(expectedType) == 16 && IsSigned(expectedType) == true) ? expectedType : typeof(Int16);
                case OpCodeCodes.Conv_I4:
                case OpCodeCodes.Conv_Ovf_I4:
                    return (GetInformationAmount(expectedType) == 32 && IsSigned(expectedType) == true) ? expectedType : typeof(Int32);
                case OpCodeCodes.Conv_I8:
                case OpCodeCodes.Conv_Ovf_I8:
                    return (GetInformationAmount(expectedType) == 64 && IsSigned(expectedType) == true) ? expectedType : typeof(Int64);
                case OpCodeCodes.Conv_U1:
                case OpCodeCodes.Conv_Ovf_U1:
                    return (GetInformationAmount(expectedType) == 8 && IsSigned(expectedType) == false) ? expectedType : typeof(Byte);
                case OpCodeCodes.Conv_U2:
                case OpCodeCodes.Conv_Ovf_U2:
                    return (GetInformationAmount(expectedType) == 16 && IsSigned(expectedType) == false) ? expectedType : typeof(UInt16);
                case OpCodeCodes.Conv_U4:
                case OpCodeCodes.Conv_Ovf_U4:
                    return (GetInformationAmount(expectedType) == 32 && IsSigned(expectedType) == false) ? expectedType : typeof(UInt32);
                case OpCodeCodes.Conv_U8:
                case OpCodeCodes.Conv_Ovf_U8:
                    return (GetInformationAmount(expectedType) == 64 && IsSigned(expectedType) == false) ? expectedType : typeof(UInt64);
                case OpCodeCodes.Conv_I:
                case OpCodeCodes.Conv_Ovf_I:
                    return (GetInformationAmount(expectedType) == nativeInt && IsSigned(expectedType) == true) ? expectedType : typeof(IntPtr);
                case OpCodeCodes.Conv_U:
                case OpCodeCodes.Conv_Ovf_U:
                    return (GetInformationAmount(expectedType) == nativeInt && IsSigned(expectedType) == false) ? expectedType : typeof(UIntPtr);
                case OpCodeCodes.Conv_R4:
                    return typeof(Single);
                case OpCodeCodes.Conv_R8:
                    return typeof(Double);
                case OpCodeCodes.Conv_R_Un:
                    return (expectedType != null && Type.GetTypeCode(expectedType) == TypeCode.Single) ? typeof(Single) : typeof(Double);
                case OpCodeCodes.Castclass:
                case OpCodeCodes.Isinst:
                case OpCodeCodes.Unbox_Any:
                    return (Type)expr.Operand;
                case OpCodeCodes.Box:
                    if (forceInferChildren)
                        InferTypeForExpression(expr.Arguments.Single(), (Type)expr.Operand);
                    return (Type)expr.Operand;
                #endregion
                #region Comparison instructions
                case OpCodeCodes.Ceq:
                    if (forceInferChildren)
                        InferArgumentsInBinaryOperator(expr, null);
                    return typeof(bool);
                case OpCodeCodes.Clt:
                case OpCodeCodes.Cgt:
                    if (forceInferChildren)
                        InferArgumentsInBinaryOperator(expr, true);
                    return typeof(bool);
                case OpCodeCodes.Clt_Un:
                case OpCodeCodes.Cgt_Un:
                    if (forceInferChildren)
                        InferArgumentsInBinaryOperator(expr, false);
                    return typeof(bool);
                #endregion
                #region Branch instructions
                case OpCodeCodes.Brtrue:
                    if (forceInferChildren)
                        InferTypeForExpression(expr.Arguments.Single(), typeof(bool));
                    return null;
                case OpCodeCodes.Br:
                case OpCodeCodes.Leave:
                case OpCodeCodes.Endfinally:
                case OpCodeCodes.Switch:
                case OpCodeCodes.Throw:
                case OpCodeCodes.Rethrow:
                    return null;
                case OpCodeCodes.Ret:
                    if (forceInferChildren && expr.Arguments.Count == 1)
                        InferTypeForExpression(expr.Arguments[0], ((MethodInfo)methodInfo).ReturnType);
                    return null;
                #endregion
                case OpCodeCodes.Pop:
                    return null;
                case OpCodeCodes.Dup:
                    return InferTypeForExpression(expr.Arguments.Single(), expectedType);
                default:
                    Debug.WriteLine("Type Inference: Can't handle " + expr.Code.ToString().ToLower());
                    return null;
            }
        }

        static Type GetFieldType(FieldInfo fieldReference)
        {
            return SubstituteTypeArgs(UnpackModifiers(fieldReference.FieldType), fieldReference);
        }

        static Type SubstituteTypeArgs(Type type, MemberInfo member)
        {
            
            if (type.IsArray)
            {
                Type elementType = SubstituteTypeArgs(type.GetElementType(), member);
                if (elementType != type.GetElementType())
                {
                    var newArrayType = elementType.MakeArrayType(type.GetArrayRank());
                    return newArrayType;
                }
                else
                {
                    return type;
                }
            }
            if (type.IsByRef)
            {
                Type elementType = SubstituteTypeArgs(type.GetElementType(), member);
                return elementType != type.GetElementType() ? elementType.MakeByRefType() : type;
            }
            if (type.IsGenericTypeDefinition)
            {
                Type[] genericArgument = type.GetGenericArguments();
                Type[] newGenericArguments = new Type[genericArgument.Length];
                bool isChanged = false;
                for (int i = 0; i < genericArgument.Length; i++)
                {
                    newGenericArguments[i] = (SubstituteTypeArgs(genericArgument[i], member));
                    isChanged |= newGenericArguments[i] != newGenericArguments[i];
                }
                Type newType = type.MakeGenericType(newGenericArguments);
                return isChanged ? newType : type;
            }
            if (type.IsPointer)
            {
                Type elementType = SubstituteTypeArgs(type.GetElementType(), member);
                return elementType != type.GetElementType() ? elementType.MakePointerType() : type;
            }
            
            if (type.IsGenericParameter)
            {
                
                if (type.DeclaringMethod  != null)
                {
                    return ((MethodInfo)member).GetGenericArguments()[type.GenericParameterPosition];
                }
                else
                {
                    if (member.DeclaringType.IsArray)
                    {
                        return (member.DeclaringType).GetElementType();
                    }
                    else
                    {
                        return (member.DeclaringType).GetGenericArguments()[type.GenericParameterPosition];
                    }
                }
            }
            return type;
        }

        static Type UnpackPointer(Type pointerOrManagedReference)
        {
            if (pointerOrManagedReference.IsByRef || pointerOrManagedReference.IsPointer)
                return pointerOrManagedReference.GetElementType();
            return null;
        }

        static Type UnpackModifiers(Type type)
        {
            while (type.Namespace.Equals("System.Runtime.CompilerServices"))/*is OptionalModifier  || is RequiredModifier*/
                type = type.GetElementType();
            return type;
        }

        Type InferArgumentsInBinaryOperator(ILExpression expr, bool? isSigned)
        {
            ILExpression left = expr.Arguments[0];
            ILExpression right = expr.Arguments[1];
            Type leftPreferred = DoInferTypeForExpression(left, null, false);
            Type rightPreferred = DoInferTypeForExpression(right, null, false);
            if (leftPreferred == rightPreferred)
            {
                return left.InferredType = right.InferredType = left.ExpectedType = right.ExpectedType = leftPreferred;
            }
            else if (rightPreferred == DoInferTypeForExpression(left, rightPreferred, false))
            {
                return left.InferredType = right.InferredType = left.ExpectedType = right.ExpectedType = rightPreferred;
            }
            else if (leftPreferred == DoInferTypeForExpression(right, leftPreferred, false))
            {
                return left.InferredType = right.InferredType = left.ExpectedType = right.ExpectedType = leftPreferred;
            }
            else
            {
                left.ExpectedType = right.ExpectedType = TypeWithMoreInformation(leftPreferred, rightPreferred);
                left.InferredType = DoInferTypeForExpression(left, left.ExpectedType, false);
                right.InferredType = DoInferTypeForExpression(right, right.ExpectedType, false);
                return left.ExpectedType;
            }
        }

        Type TypeWithMoreInformation(Type leftPreferred, Type rightPreferred)
        {
            int left = GetInformationAmount(leftPreferred);
            int right = GetInformationAmount(rightPreferred);
            if (left < right)
                return rightPreferred;
            else
                return leftPreferred;
        }

        const int nativeInt = 33; // treat native int as between int32 and int64

        static int GetInformationAmount(Type type)
        {
            if (type == null)
                return 0;
            if (type.IsValueType)
            {
                // value type might be an enum
                if (type.IsEnum)
                {
                    Type underlyingType = type.GetFields().Single(f => f.IsSpecialName && !f.IsStatic).FieldType;
                    return GetInformationAmount(underlyingType);
                }
            }
            if (type == typeof(UIntPtr) || type == typeof(IntPtr))
                return nativeInt;

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Empty:
                    return 0;
                case TypeCode.Boolean:
                    return 1;
                case TypeCode.SByte:
                case TypeCode.Byte:
                    return 8;
                case TypeCode.Char:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    return 16;
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Single:
                    return 32;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Double:
                    return 64;
                default:
                    return 100; // we consider structs/objects to have more information than any primitives
            }
        }

        public static bool IsBoolean(Type type)
        {
            return type != null && Type.GetTypeCode(type) == TypeCode.Boolean;
        }

        public static bool IsIntegerOrEnum(Type type)
        {
            return IsSigned(type) != null;
        }

        public static bool IsEnum(Type type)
        {
            if (type == null)
                return false;
            return type.IsEnum;
        }

        static bool? IsSigned(Type type)
        {
            if (type == null)
                return null;
            // unfortunately we cannot rely on type.IsValueType here - it's not set when the instruction operand is a typeref (as opposed to a typespec)
            if (type != null && type.IsEnum)
            {
                Type underlyingType = type.GetFields().Single(f => f.IsSpecialName && !f.IsStatic).FieldType;
                return IsSigned(underlyingType);
            }
            if (type == typeof(IntPtr))
                return true;
            if (type == typeof(UIntPtr))
                return false;

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return true;
                case TypeCode.Byte:
                case TypeCode.Char:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return false;
                default:
                    return null;
            }
        }
    }
}
