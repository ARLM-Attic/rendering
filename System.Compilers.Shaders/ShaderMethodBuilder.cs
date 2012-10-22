using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.ShaderAST;
using System.Compilers.Shaders.Info;
using System.ComponentModel;
using System.Maths;

namespace System.Compilers.Shaders
{
    public class ShaderMethodBuilder
    {
        public ShaderProgramAST Program { get; private set; }

        public Builtins Builtins { get; private set; }

        public IList<ShaderStatementAST> Statements { get; private set; }

        public ShaderMethodBaseDeclarationAST Method { get; private set; }

        private ShaderMethodBuilder(ShaderMethodBaseDeclarationAST method, IList<ShaderStatementAST> statements)
        {
            this.Program = method.Program;
            this.Builtins = method.Program.Builtins;
            this.Method = method;
            this.Statements = statements;
        }

        internal ShaderMethodBuilder(ShaderMethodBaseDeclarationAST method):this (method, method.Body.StatementsList)
        {
        }

        internal ShaderMethodBuilder(ShaderMethodBaseDeclarationAST method, ShaderBlockStatementAST block)
            :this (method, block.StatementList)
        {
        }

        public ShaderLocal DeclareLocal(ShaderType type, string name)
        {
            ShaderLocalDeclarationAST localDec = new ShaderLocalDeclarationAST(Program, type, name);

            Statements.Add (localDec);

            return localDec.Local;
        }

        #region Expressions

        public ShaderConstantExpressionAST Constant<T>(T value)
        {
            return Program.CreateConstant<T>(value);
        }

        public ShaderConstantExpressionAST Constant(ShaderType type, object value)
        {
            return Program.CreateConstant(type, value);
        }

        public ShaderLocalInvokeAST Local(ShaderLocal local)
        {
            return Program.CreateInvoke(local);
        }

        public ShaderParameterInvokeAST Parameter(ShaderParameter parameter)
        {
            return Program.CreateInvoke(parameter);
        }

        public ShaderFieldInvokeAST Field(ShaderExpressionAST target, ShaderField field)
        {
            return Program.CreateInvoke(field, target);
        }

        public ShaderMethodInvokeAST Call(ShaderExpressionAST target, ShaderMethod method, params ShaderExpressionAST[] arguments)
        {
            return Program.CreateInvoke(method, target, arguments);
        }

        public void AddCall(ShaderExpressionAST target, ShaderMethod method, params ShaderExpressionAST[] arguments)
        {
            Statements.Add(Program.CreateExpressionStatement(Program.CreateInvoke(method, target, arguments)));
        }

        public ShaderConstructorInvokeAST Create(ShaderConstructor constructor, params ShaderExpressionAST[] arguments)
        {
            return Program.CreateInvoke(constructor, arguments);
        }

        public ShaderOperationAST Operation(Operators op, params ShaderExpressionAST[] arguments)
        {
            return Program.CreateOperation(op, arguments);
        }

        #endregion

        #region Assignaments

        /// <summary>
        /// General statement for assignaments.
        /// exp1 = exp2;
        /// </summary>
        public void AddAssignament(ShaderExpressionAST leftValue, ShaderExpressionAST expression)
        {
            Statements.Add(Program.CreateExpressionStatement(Program.CreateAssignament(leftValue, expression)));
        }

        /// <summary>
        /// General statement for local assignaments.
        /// local = exp;
        /// </summary>
        public void AddAssignament(ShaderLocal local, ShaderExpressionAST expression)
        {
            Statements.Add(Program.CreateExpressionStatement(Program.CreateAssignament(Program.CreateInvoke(local), expression)));
        }

        /// <summary>
        /// Statement for assignaments to local fields.
        /// local.Field = exp
        /// </summary>
        public void AddAssignament(ShaderLocal local, ShaderField localField, ShaderExpressionAST expression)
        {
            Statements.Add(Program.CreateExpressionStatement(Program.CreateAssignament(Program.CreateInvoke(localField, Program.CreateInvoke(local)), expression)));
        }

        public void AddInitialization(ShaderLocal local)
        {
            AddAssignament(local, Program.CreateInitialization(local.Type));
        }

        /// <summary>
        /// This method should not be used to generate assignament, use Assignament overloads instead.
        /// </summary>
        [EditorBrowsable (EditorBrowsableState.Never)]
        public ShaderStatementAST CreateAssignament(ShaderExpressionAST leftValue, ShaderExpressionAST expression)
        {
            return Program.CreateExpressionStatement(Program.CreateAssignament(leftValue, expression));
        }

        #endregion

        #region Adapt

        static ShaderMethod CreatePromotionOf(ShaderProgramAST program, ShaderType type)
        {
            string methodName = "promoteVec";
            var method = program.Members.OfType<ShaderMethod>().FirstOrDefault(m => m.Name.StartsWith(methodName) && m.Parameters.First().ParameterType.Equals(type));
            if (method != null)
                return method;

            program.PushMode(ShaderProgramAST.AddingMode.Prepend);
            var methodDeclaration = program.DeclareNewMethod(methodName, 0);
            program.PopMode();

            var parameter = methodDeclaration.DeclareNewParameter(type, "v", null, ParameterModifier.In);

            var vec4Type = program.Builtins.Resolve(typeof(Vector4));
            methodDeclaration.ReturnType = vec4Type;

            ShaderExpressionAST[] arguments = new ShaderExpressionAST[4];

            int count = 0;
            foreach (var m in type.Members.OfType<ShaderField>())
            {
                arguments[count] = program.CreateInvoke(m, program.CreateInvoke(parameter));
                count++;
            }

            if (count == 0) // basic types
            {
                while (count < 4)
                {
                    arguments[count] = program.CreateInvoke(parameter);
                    count++;
                }
            }

            while (count < 4)
                arguments[count++] = program.CreateConstant<float>(0.0f);

            methodDeclaration.Body.StatementsList.Add(new ShaderReturnStatementAST(program,
                program.CreateInvoke(program.Builtins.GetBestOverload(vec4Type, new ShaderType[] { program.Builtins.Float, program.Builtins.Float, program.Builtins.Float, program.Builtins.Float }), arguments)));

            return methodDeclaration.Method;
        }

        /// <summary>
        /// Performs an assignament, demotion or promotion according to the type being assigned.
        /// </summary>
        public void AddAdapt(ShaderExpressionAST leftValue, ShaderExpressionAST expression)
        {
            if (leftValue.Type.Equals(expression.Type))
            {
                AddAssignament(leftValue, expression);
            }
            else
            {
                if (Builtins.GetConversion(expression.Type, leftValue.Type) != null)
                {
                    AddAssignament(leftValue,
                        Program.CreateConversion(leftValue.Type, expression));
                }
                else
                {
                    var promotionMethod = CreatePromotionOf(Program, expression.Type);

                    AddAssignament(leftValue,
                        Program.CreateConversion(leftValue.Type, Program.CreateInvoke(promotionMethod, null, expression)));
                }
            }
        }

        #endregion

        #region Return

        /// <summary>
        /// Adds a return statement to the 
        /// </summary>
        /// <param name="returnExpression"></param>
        public void AddReturn(ShaderExpressionAST returnExpression)
        {
            Statements.Add(Program.CreateReturn(returnExpression));
        }

        #endregion

        #region Loops

        /// <summary>
        /// Adds a new for statement to the code.
        /// </summary>
        public void AddFor(ShaderType iterationType, string name, ShaderExpressionAST initialValue, Func<ShaderLocal, ShaderExpressionAST> conditional, Func<ShaderLocal, ShaderStatementAST> increment,
            Action<ShaderLocal, ShaderMethodBuilder> body)
        {
            var i = DeclareLocal(iterationType, name);
            var cond = conditional(i);
            var inc = increment(i);

            ShaderBlockStatementAST block = new ShaderBlockStatementAST(Program);
            ShaderMethodBuilder builder = new ShaderMethodBuilder(Method, block);

            body(i, builder);

            Statements.Add(Program.CreateFor(Program.CreateExpressionStatement(Program.CreateAssignament(Program.CreateInvoke(i), initialValue)),
                cond, inc, block));
        }

        /// <summary>
        /// Adds a simple for statement that repeats certain number of time
        /// </summary>
        public void AddFor(int numberOfTimes, Action<ShaderLocal, ShaderMethodBuilder> body)
        {
            AddFor(Builtins.Int, "i", Constant(0), i => Operation(Operators.LessThan, Local(i), Constant(numberOfTimes)),
                i => CreateAssignament(Local(i), Operation(Operators.Addition, Local(i), Constant(1))),
                body);
        }

        /// <summary>
        /// Adds a while to the code.
        /// </summary>
        public void AddWhile(ShaderExpressionAST conditional, Action<ShaderMethodBuilder> body)
        {
            ShaderBlockStatementAST block = new ShaderBlockStatementAST(Program);
            ShaderMethodBuilder builder = new ShaderMethodBuilder(Method, block);

            body(builder);

            Statements.Add(Program.CreateWhile(conditional, block));
        }

        /// <summary>
        /// Adds a do while to the code.
        /// </summary>
        public void AddDoWhile(Action<ShaderMethodBuilder> body, ShaderExpressionAST conditional)
        {
            ShaderBlockStatementAST block = new ShaderBlockStatementAST(Program);
            ShaderMethodBuilder builder = new ShaderMethodBuilder(Method, block);

            body(builder);

            Statements.Add(Program.CreateDoWhile(conditional, block));
        }

        #endregion

    }
}
