using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.AST.Declarations;
using GLSLCompiler.AST;
using GLSLCompiler.Utils;

namespace GLSLCompiler
{
  public class Scope
  {
    private Dictionary<string, VariableInfo> variables;
    private Dictionary<string, TypeInfo> types;
    private Dictionary<string, List<FunctionInfo>> functions;

    private int countOfAnonymousTypes = 0;

    public Scope(ScopeType type) : this(type, null)
    {
    }

    public Scope(ScopeType type, Scope parentScope)
    {
      ScopeType = type;
      ParentScope = parentScope;
      variables = new Dictionary<string, VariableInfo>();
      types = new Dictionary<string, TypeInfo>();
      functions = new Dictionary<string, List<FunctionInfo>>();
    }

    #region [ GetMainScope Methods ]

    private static void AddBasicTypes(Scope mainscope)
    {
      mainscope.AddType(GLSLTypes.FloatType.Name, GLSLTypes.FloatType);
      mainscope.AddType(GLSLTypes.IntegerType.Name, GLSLTypes.IntegerType);
      mainscope.AddType(GLSLTypes.BoolType.Name, GLSLTypes.BoolType);
      mainscope.AddType(GLSLTypes.VoidType.Name, GLSLTypes.VoidType);
      
      mainscope.AddType(GLSLTypes.FloatVec2Type.Name, GLSLTypes.FloatVec2Type);
      mainscope.AddType(GLSLTypes.FloatVec3Type.Name, GLSLTypes.FloatVec3Type);
      mainscope.AddType(GLSLTypes.FloatVec4Type.Name, GLSLTypes.FloatVec4Type);
      mainscope.AddType(GLSLTypes.IntegerVec2Type.Name, GLSLTypes.IntegerVec2Type);
      mainscope.AddType(GLSLTypes.IntegerVec3Type.Name, GLSLTypes.IntegerVec3Type);
      mainscope.AddType(GLSLTypes.IntegerVec4Type.Name, GLSLTypes.IntegerVec4Type);
      mainscope.AddType(GLSLTypes.BoolVec2Type.Name, GLSLTypes.BoolVec2Type);
      mainscope.AddType(GLSLTypes.BoolVec3Type.Name, GLSLTypes.BoolVec3Type);
      mainscope.AddType(GLSLTypes.BoolVec4Type.Name, GLSLTypes.BoolVec4Type);
      
      mainscope.AddType(GLSLTypes.Mat2x2Type.Name, GLSLTypes.Mat2x2Type);
      mainscope.AddType(GLSLTypes.Mat2x3Type.Name, GLSLTypes.Mat2x3Type);
      mainscope.AddType(GLSLTypes.Mat2x4Type.Name, GLSLTypes.Mat2x4Type);
      mainscope.AddType(GLSLTypes.Mat3x2Type.Name, GLSLTypes.Mat3x2Type);
      mainscope.AddType(GLSLTypes.Mat3x3Type.Name, GLSLTypes.Mat3x3Type);
      mainscope.AddType(GLSLTypes.Mat3x4Type.Name, GLSLTypes.Mat3x4Type);
      mainscope.AddType(GLSLTypes.Mat4x2Type.Name, GLSLTypes.Mat4x2Type);
      mainscope.AddType(GLSLTypes.Mat4x3Type.Name, GLSLTypes.Mat4x3Type);
      mainscope.AddType(GLSLTypes.Mat4x4Type.Name, GLSLTypes.Mat4x4Type);

      mainscope.AddType("mat2", GLSLTypes.Mat2x2Type);
      mainscope.AddType("mat3", GLSLTypes.Mat3x3Type);
      mainscope.AddType("mat4", GLSLTypes.Mat4x4Type);

      mainscope.AddType(GLSLTypes.Sampler1DType.Name, GLSLTypes.Sampler1DType);
      mainscope.AddType(GLSLTypes.Sampler2DType.Name, GLSLTypes.Sampler2DType);
      mainscope.AddType(GLSLTypes.Sampler3DType.Name, GLSLTypes.Sampler3DType);
      mainscope.AddType(GLSLTypes.Sampler1DShadowType.Name, GLSLTypes.Sampler1DShadowType);
      mainscope.AddType(GLSLTypes.Sampler2DShadowType.Name, GLSLTypes.Sampler2DShadowType);
    }

    private static void AddFunctions(Scope mainscope)
    {
      //mainscope.AddFunction("vec2", GLSLTypes.FloatVec2Type, new[] { GLSLTypes.FloatVec2Type });
      //mainscope.AddFunction("vec3", GLSLTypes.FloatVec3Type, new[] { GLSLTypes.FloatVec3Type });
      //mainscope.AddFunction("vec4", GLSLTypes.FloatVec4Type, new[] { GLSLTypes.FloatVec4Type });
      //mainscope.AddFunction("ivec2", GLSLTypes.IntegerVec2Type, new[] { GLSLTypes.IntegerVec2Type });
      //mainscope.AddFunction("ivec3", GLSLTypes.IntegerVec3Type, new[] { GLSLTypes.IntegerVec3Type });
      //mainscope.AddFunction("ivec4", GLSLTypes.IntegerVec4Type, new[] { GLSLTypes.IntegerVec4Type });
      //mainscope.AddFunction("bvec2", GLSLTypes.BoolVec2Type, new[] { GLSLTypes.BoolVec2Type });
      //mainscope.AddFunction("bvec3", GLSLTypes.BoolVec3Type, new[] { GLSLTypes.BoolVec3Type });
      //mainscope.AddFunction("bvec4", GLSLTypes.BoolVec4Type, new[] { GLSLTypes.BoolVec4Type });
      
      
      //mainscope.AddFunction("ftransform", GLSLTypes.FloatVec4Type, new GLSLType[0]);
      //mainscope.AddFunction("radians", GLSLTypes.FloatType, new[] { GLSLTypes.FloatType });
      //mainscope.AddFunction("degrees", GLSLTypes.FloatType, new[] { GLSLTypes.FloatType });
      //mainscope.AddFunction("sin", GLSLTypes.FloatType, new[] { GLSLTypes.FloatType });
      //mainscope.AddFunction("cos", GLSLTypes.FloatType, new[] { GLSLTypes.FloatType });
      //mainscope.AddFunction("tan", GLSLTypes.FloatType, new[] { GLSLTypes.FloatType });
      //mainscope.AddFunction("asin", GLSLTypes.FloatType, new[] { GLSLTypes.FloatType });
      //mainscope.AddFunction("acos", GLSLTypes.FloatType, new[] { GLSLTypes.FloatType });
      //mainscope.AddFunction("atan", GLSLTypes.FloatType, new[] { GLSLTypes.FloatType });
    }

    private static void AddConstantVariables(Scope mainscope)
    {
      //mainscope.AddVariable("gl_MaxLights", GLSLTypes.IntegerType, TypeQualifier.Const);
      //mainscope.AddVariable("gl_MaxClipPlanes", GLSLTypes.IntegerType, TypeQualifier.Const);
      //mainscope.AddVariable("gl_MaxTextureUnits", GLSLTypes.IntegerType, TypeQualifier.Const);
      //mainscope.AddVariable("gl_MaxTextureCoords", GLSLTypes.IntegerType, TypeQualifier.Const);
      //mainscope.AddVariable("gl_MaxVertexAttribs", GLSLTypes.IntegerType, TypeQualifier.Const);
      //mainscope.AddVariable("gl_MaxVertexUniformComponents", GLSLTypes.IntegerType, TypeQualifier.Const);
      //mainscope.AddVariable("gl_MaxVaryingFloats", GLSLTypes.IntegerType, TypeQualifier.Const);
      //mainscope.AddVariable("gl_MaxVertexTextureImageUnits", GLSLTypes.IntegerType, TypeQualifier.Const);
      //mainscope.AddVariable("gl_MaxCombinedTextureImageUnits", GLSLTypes.IntegerType, TypeQualifier.Const);
      //mainscope.AddVariable("gl_MaxTextureImageUnits", GLSLTypes.IntegerType, TypeQualifier.Const);
      //mainscope.AddVariable("gl_MaxFragmentUniformComponents", GLSLTypes.IntegerType, TypeQualifier.Const);
      //mainscope.AddVariable("gl_MaxDrawBuffers", GLSLTypes.IntegerType, TypeQualifier.Const);
    }

    private static void AddUniformVariables(Scope mainscope)
    {
      ////
      //// Matrix state
      ////
      //mainscope.AddVariable("gl_ModelViewMatrix", GLSLTypes.Mat4Type, TypeQualifier.Uniform);
      //mainscope.AddVariable("gl_ProjectionMatrix", GLSLTypes.Mat4Type, TypeQualifier.Uniform);
      //mainscope.AddVariable("gl_ModelViewProjectionMatrix", GLSLTypes.Mat4Type, TypeQualifier.Uniform);
      //mainscope.AddVariable("gl_TextureMatrix", new ArrayType("gl_TextureMatrix", GLSLTypes.Mat4Type, 1, 8), TypeQualifier.Uniform);

      ////
      //// Derived matrix state that provides inverse and transposed versions
      //// of the matrices above. Poorly conditioned matrices may result
      //// in unpredictable values in their inverse forms.
      ////
      //mainscope.AddVariable("gl_NormalMatrix", GLSLTypes.Mat3Type, TypeQualifier.Uniform); // transpose of the inverse of the upper leftmost 3x3 of gl_ModelViewMatrix

      //mainscope.AddVariable("gl_ModelViewMatrixInverse", GLSLTypes.Mat4Type, TypeQualifier.Uniform); 
      //mainscope.AddVariable("gl_ProjectionMatrixInverse", GLSLTypes.Mat4Type, TypeQualifier.Uniform); 
      //mainscope.AddVariable("gl_ModelViewProjectionMatrixInverse", GLSLTypes.Mat4Type, TypeQualifier.Uniform); 
      //mainscope.AddVariable("gl_ModelViewMatrixTranspose", GLSLTypes.Mat4Type, TypeQualifier.Uniform); 
      //mainscope.AddVariable("gl_ProjectionMatrixTranspose", GLSLTypes.Mat4Type, TypeQualifier.Uniform); 
      //mainscope.AddVariable("gl_ModelViewProjectionMatrixTranspose", GLSLTypes.Mat4Type, TypeQualifier.Uniform); 
      //mainscope.AddVariable("gl_ModelViewMatrixInverseTranspose", GLSLTypes.Mat4Type, TypeQualifier.Uniform); 
      //mainscope.AddVariable("gl_ProjectionMatrixInverseTranspose", GLSLTypes.Mat4Type, TypeQualifier.Uniform); 
      //mainscope.AddVariable("gl_ModelViewProjectionMatrixInverseTranspose", GLSLTypes.Mat4Type, TypeQualifier.Uniform); 
      //mainscope.AddVariable("gl_TextureMatrixInverse", new ArrayType("gl_TextureMatrixInverse", GLSLTypes.Mat4Type, 1, 8), TypeQualifier.Uniform); 
      //mainscope.AddVariable("gl_TextureMatrixTranspose", new ArrayType("gl_TextureMatrixTranspose", GLSLTypes.Mat4Type, 1, 8), TypeQualifier.Uniform); 
      //mainscope.AddVariable("gl_TextureMatrixInverseTranspose", new ArrayType("gl_TextureMatrixInverseTranspose", GLSLTypes.Mat4Type, 1, 8), TypeQualifier.Uniform); 
      
      ////
      //// Normal scaling
      ////
      //mainscope.AddVariable("gl_NormalScale", GLSLTypes.FloatType, TypeQualifier.Uniform);

      ////
      //// Depth range in window coordinates, p. 33
      ////
      //StructDeclarationAST structDecl = new StructDeclarationAST("gl_DepthRangeParameters", 
      //  new [] {
      //    new MultipleFieldDeclarationAST(
      //      new [] {
      //        new FieldDeclarationAST("near", 0, 0),
      //        new FieldDeclarationAST("far", 0, 0),
      //        new FieldDeclarationAST("diff", 0, 0) 
      //      }, 
      //      new NamedTypeSpecifier("float", 0, 0), 0, 0) 
      //  }, 0, 0);
      //StructType gl_DepthRangeParametersType = new StructType("gl_DepthRangeParameters", structDecl,
      //  new[] {
      //    new StructType.FieldInfo("near", "float"),
      //    new StructType.FieldInfo("far", "float"),
      //    new StructType.FieldInfo("diff", "float")
      //  });
      //mainscope.AddType("gl_DepthRangeParameters", gl_DepthRangeParametersType);
      //mainscope.AddVariable("gl_DepthRange", gl_DepthRangeParametersType, TypeQualifier.Uniform);
    }

    private static void AddVertexShaderVariables(Scope mainscope)
    {
      //mainscope.AddVariable("gl_Position", GLSLTypes.FloatVec4Type);   // must be written
      //mainscope.AddVariable("gl_PointSize", GLSLTypes.FloatType);      // may be written
      //mainscope.AddVariable("gl_ClipVertex", GLSLTypes.FloatVec4Type); // may be written

      //mainscope.AddVariable("gl_Color", GLSLTypes.FloatVec4Type, TypeQualifier.Attribute);
      //mainscope.AddVariable("gl_SecondaryColor", GLSLTypes.FloatVec4Type, TypeQualifier.Attribute);
      //mainscope.AddVariable("gl_Normal", GLSLTypes.FloatVec4Type, TypeQualifier.Attribute);
      //mainscope.AddVariable("gl_Vertex", GLSLTypes.FloatVec4Type, TypeQualifier.Attribute);
      //mainscope.AddVariable("gl_MultiTexCoord0", GLSLTypes.FloatVec4Type, TypeQualifier.Attribute);
      //mainscope.AddVariable("gl_MultiTexCoord1", GLSLTypes.FloatVec4Type, TypeQualifier.Attribute);
      //mainscope.AddVariable("gl_MultiTexCoord2", GLSLTypes.FloatVec4Type, TypeQualifier.Attribute);
      //mainscope.AddVariable("gl_MultiTexCoord3", GLSLTypes.FloatVec4Type, TypeQualifier.Attribute);
      //mainscope.AddVariable("gl_MultiTexCoord4", GLSLTypes.FloatVec4Type, TypeQualifier.Attribute);
      //mainscope.AddVariable("gl_MultiTexCoord5", GLSLTypes.FloatVec4Type, TypeQualifier.Attribute);
      //mainscope.AddVariable("gl_MultiTexCoord6", GLSLTypes.FloatVec4Type, TypeQualifier.Attribute);
      //mainscope.AddVariable("gl_MultiTexCoord8", GLSLTypes.FloatVec4Type, TypeQualifier.Attribute);
      //mainscope.AddVariable("gl_FogCoord", GLSLTypes.FloatVec4Type, TypeQualifier.Attribute);
    
      //mainscope.AddVariable("gl_FrontColor", GLSLTypes.FloatVec4Type, TypeQualifier.Varying);
      //mainscope.AddVariable("gl_BackColor", GLSLTypes.FloatVec4Type, TypeQualifier.Varying);
      //mainscope.AddVariable("gl_FrontSecondaryColor", GLSLTypes.FloatVec4Type, TypeQualifier.Varying);
      //mainscope.AddVariable("gl_BackSecondaryColor", GLSLTypes.FloatVec4Type, TypeQualifier.Varying);
      //mainscope.AddVariable("gl_TexCoord", new ArrayType("gl_TexCoord", GLSLTypes.FloatVec4Type, 1, 0)); // at most will be gl_MaxTextureCoords
      //mainscope.AddVariable("gl_FogFragCoord", GLSLTypes.FloatType);
    }

    private static void AddFragmentShaderVariables(Scope mainscope)
    {
      //mainscope.AddVariable("gl_FragCoord", GLSLTypes.FloatVec4Type);
      //mainscope.AddVariable("gl_FrontFacing", GLSLTypes.BoolType);
      //mainscope.AddVariable("gl_FragColor", GLSLTypes.FloatVec4Type);
      //mainscope.AddVariable("gl_FragDepth", GLSLTypes.FloatType);
      //mainscope.AddVariable("gl_FragData", new ArrayType("gl_FragData", GLSLTypes.FloatType, 1, 0));

      //mainscope.AddVariable("gl_Color", GLSLTypes.FloatVec4Type);
      //mainscope.AddVariable("gl_SecondaryColor", GLSLTypes.FloatVec4Type);
      //mainscope.AddVariable("gl_TexCoord", new ArrayType("gl_TexCoord", GLSLTypes.FloatVec4Type, 1, 0)); // at most will be gl_MaxTextureCoords
      //mainscope.AddVariable("gl_FogFragCoord", GLSLTypes.FloatType);
    }

    public static Scope GetMainScope(ShaderType shaderType)
    {
      Scope mainscope = new Scope(ScopeType.GlobalScope);
      AddBasicTypes(mainscope);
      AddConstantVariables(mainscope);
      switch (shaderType)
      {
        case ShaderType.VertexShader:
          AddVertexShaderVariables(mainscope);
          break;
        case ShaderType.FramentShader:
          AddFragmentShaderVariables(mainscope);
          break;
        case ShaderType.GeometryShader:
          break;
        default:
          break;
      }
      return mainscope;
    }

    #endregion

    #region [ Contains Methods ]

    public bool ContainsVariable(string varName)
    {
      return variables.ContainsKey(varName);
    }

    public bool DeepContainsVariable(string varName)
    {
      return ContainsVariable(varName) || ((ParentScope != null) ? ParentScope.DeepContainsVariable(varName) : false);
    }

    public bool ContainsType(string typeName)
    {
      return types.ContainsKey(typeName);
    }

    public bool DeepContainsType(string typeName)
    {
      return ContainsType(typeName) || ((ParentScope != null) ? ParentScope.DeepContainsType(typeName) : false);
    }

    public bool ContainsFunction(string functionName)
    {
      return functions.ContainsKey(functionName);
    }

    public bool ContainsFunction(string functionName, params GLSLType[] paramTypes)
    {
      List<FunctionInfo> fInfos;
      if(functions.TryGetValue(functionName, out fInfos))
        return fInfos.Exists(info => info.ParamInfos.Select(pInfo => pInfo.Type).SequenceEqual(paramTypes));
      return false;
    }

    public bool DeepContainsFunction(string functionName)
    {
      return ContainsFunction(functionName) || ((ParentScope != null) ? ParentScope.DeepContainsFunction(functionName) : false);
    }

    #endregion

    #region [ Add Methods ]

    public void AddVariable(VariableInfo varinfo)
    {
      variables.Add(varinfo.Name, varinfo);
    }

    public void AddType(string name, GLSLType type)
    {
      AddType(new TypeInfo() { Name = name, Type = type });
    }

    public void AddType(TypeInfo typeinfo)
    {
      types.Add(typeinfo.Name, typeinfo);
    }

    public void AddFunction(FunctionInfo funcinfo)
    {
      if (!functions.ContainsKey(funcinfo.Name))
        functions[funcinfo.Name] = new List<FunctionInfo>();
      functions[funcinfo.Name].Add(funcinfo);
    }

    #endregion

    #region [ Get Methods ]

    public VariableInfo GetVariableInfo(string name)
    {
      VariableInfo vInfo;
      return TryGetVariableInfo(name, out vInfo) ? vInfo : null;
    }

    public T GetVariableInfo<T>(string name) where T : VariableInfo
    {
      return GetVariableInfo(name).Cast<T>();
    }

    public bool TryGetVariableInfo(string name, out VariableInfo varInfo)
    {
      if (variables.TryGetValue(name, out varInfo))
        return true;
      return ParentScope != null ? ParentScope.TryGetVariableInfo(name, out varInfo) : false;
    }

    public TypeInfo GetTypeInfo(string name)
    {
      TypeInfo tInfo;
      return TryGetTypeInfo(name, out tInfo) ? tInfo : null;
    }

    public bool TryGetTypeInfo(string name, out TypeInfo typeInfo)
    {
      if (types.TryGetValue(name, out typeInfo))
        return true;
      return ParentScope != null ? ParentScope.TryGetTypeInfo(name, out typeInfo) : false;
    }

    public IEnumerable<FunctionInfo> GetFunctionInfo(string name)
    {
      List<FunctionInfo> result;
      return TryGetFunctionInfo(name, out result) ? result : null;
    }

    public bool TryGetFunctionInfo(string name, out List<FunctionInfo> funcInfo)
    {
      if (functions.TryGetValue(name, out funcInfo))
        return true;
      return ParentScope != null ? ParentScope.TryGetFunctionInfo(name, out funcInfo) : false;
    }

    public bool TryGetFunctionInfo(string name, IEnumerable<GLSLType> paramTypes, out FunctionInfo funcInfo)
    {
      funcInfo = null;
      List<FunctionInfo> fInfos;
      if (functions.TryGetValue(name, out fInfos))
        funcInfo = fInfos.FirstOrDefault(info => info.ParamInfos.Select(pInfo => pInfo.Type).SequenceEqual(paramTypes));
      return funcInfo != null;
    }

    public string GetAnonymousTypeName()
    {
      return "<>anonymous_type_{0}".Fmt(countOfAnonymousTypes++);
    }

    #endregion

    public Scope ParentScope { get; set; }

    public ScopeType ScopeType { get; set; }

    public bool IsGlobalScope
    {
      get { return ScopeType == ScopeType.GlobalScope; }
    }

    public bool AllowContinue
    {
      get
      {
        return (ScopeType == ScopeType.BreakableScope) ||
          (ParentScope != null ? ParentScope.AllowContinue : false);
      }
    }

    public bool AllowBreak
    {
      get
      {
        return (ScopeType == ScopeType.BreakableScope) ||
          (ParentScope != null ? ParentScope.AllowBreak : false);
      }
    }

    public bool AllowReturn
    {
      get
      {
        return (ScopeType == ScopeType.FunctionDeclarationScope) ||
          (ParentScope != null ? ParentScope.AllowReturn : false);
      }
    }
  }

  public enum ScopeType
  {
    GlobalScope,
    FunctionDeclarationScope,
    StructDeclarationScope,
    BreakableScope,
    NonBreakableScope
  }
}
