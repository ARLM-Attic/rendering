using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Antlr.Runtime;
using GLSLCompiler.AST;
using GLSLCompiler.Types;

namespace GLSLCompiler
{
  public class GLSLCompiler
  {
    public static IEnumerable<CompilerResult> Compile(string filename)
    {
      return Compile(new StreamReader(filename));
    }

    public static IEnumerable<CompilerResult> Compile(TextReader textReader)
    {
      List<CompilerResult> result = new List<CompilerResult>();
      GLSLLexer lexer = new GLSLLexer(new ANTLRReaderStream(textReader));
      CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
      GLSLParser parser = new GLSLParser(commonTokenStream);
      BaseAST ast = null;

      try
      {
        ast = parser.program();
      }
      catch (RecognitionException e)
      {
        result.Add(new SyntacticError(e.Message, e.Line, e.CharPositionInLine));
      }
      catch (Exception e)
      {
        //result.Add(new SyntacticError(e.Message, 0, 0));
      }
      if (parser.NumberOfSyntaxErrors == 0 && ast != null)
      {
        GLSLTypes.Reset();
        Scope baseScope = Scope.GetMainScope(ShaderType.VertexShader);
        SemanticContext cxt = new SemanticContext(baseScope);
        try
        {
          ast.CheckSemantic(cxt);
        }
        catch (Exception e)
        {
          result.Add(new CompilerError(e.Message, 0, 0));
        }
        finally
        {
          result.AddRange(cxt.Errors.Cast<CompilerResult>());
          result.AddRange(cxt.Warnings.Cast<CompilerResult>());
        }
      }
      return result;
    }
  }
}
