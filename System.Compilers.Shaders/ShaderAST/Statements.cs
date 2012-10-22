using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.Info;

namespace System.Compilers.Shaders.ShaderAST
{
    public abstract class ShaderStatementAST : ShaderNodeAST
    {
        internal ShaderStatementAST(ShaderProgramAST program)
            : base(program)
        {
        }
    }

    public class ShaderLocalDeclarationAST : ShaderStatementAST
    {
        public ShaderLocalDeclarationAST(ShaderProgramAST program, ShaderType localType, string name)
            : base(program)
        {
            this.Type = localType;
            this.Name = name;
            this.Local = new ShaderLocal(this);
        }

        public ShaderType Type { get; private set; }

        public string Name { get; private set; }

        public ShaderLocal Local { get; private set; }
    }

    public class ShaderExpressionStatementAST : ShaderStatementAST
    {
        internal ShaderExpressionStatementAST(ShaderProgramAST program, ShaderExpressionAST expression)
            : base(program)
        {
            this.Expression = expression;
        }

        public ShaderExpressionAST Expression { get; private set; }
    }

    public class ShaderBlockStatementAST : ShaderStatementAST
    {
        internal ShaderBlockStatementAST(ShaderProgramAST program)
            :base (program)
        {
        }

        List<ShaderStatementAST> statements = new List<ShaderStatementAST>();

        public IEnumerable<ShaderStatementAST> Statements { get { return statements; } }

        internal IList<ShaderStatementAST> StatementList { get { return statements; } }
    }

    public class ShaderWhileStatementAST : ShaderStatementAST
    {
        internal ShaderWhileStatementAST(ShaderProgramAST program, ShaderExpressionAST conditional, ShaderBlockStatementAST body)
            :base(program)
        {
            this.Conditional = conditional;
            this.Body = body;
        }

        public ShaderExpressionAST Conditional { get; private set; }

        public ShaderBlockStatementAST Body { get; private set; }
    }

    public class ShaderDoWhileStatementAST : ShaderStatementAST
    {
        internal ShaderDoWhileStatementAST(ShaderProgramAST program, ShaderExpressionAST conditional, ShaderBlockStatementAST body)
            : base(program)
        {
            this.Conditional = conditional;
            this.Body = body;
        }

        public ShaderExpressionAST Conditional { get; private set; }

        public ShaderBlockStatementAST Body { get; private set; }
    }

    public class ShaderForStatementAST : ShaderStatementAST
    {
        internal ShaderForStatementAST(ShaderProgramAST program, ShaderStatementAST initialization, ShaderExpressionAST conditional, ShaderStatementAST increment, ShaderBlockStatementAST body)
            :base (program)
        {
            this.Initialization = initialization;
            this.Conditional = conditional;
            this.Increment = increment;
            this.Body = body;
        }

        public ShaderStatementAST Initialization { get; private set; }

        public ShaderExpressionAST Conditional { get; private set; }

        public ShaderStatementAST Increment { get; private set; }

        public ShaderBlockStatementAST Body { get; private set; }
    }

    public class ShaderReturnStatementAST : ShaderStatementAST
    {
        internal ShaderReturnStatementAST(ShaderProgramAST program, ShaderExpressionAST expression)
            : base(program)
        {
            this.Expression = expression;
        }

        public ShaderExpressionAST Expression { get; private set; }
    }

    public class ShaderConditionalStatement : ShaderStatementAST
    {
        internal ShaderConditionalStatement(ShaderProgramAST program, ShaderExpressionAST conditional, ShaderBlockStatementAST trueBlock, ShaderBlockStatementAST falseBlock)
            : base(program)
        {
            this.Conditional = conditional;
            this.TrueBlock = trueBlock;
            this.FalseBlock = falseBlock;
        }

        public ShaderExpressionAST Conditional { get; private set; }

        public ShaderBlockStatementAST TrueBlock { get; private set; }

        public ShaderBlockStatementAST FalseBlock { get; private set; }
    }

}
