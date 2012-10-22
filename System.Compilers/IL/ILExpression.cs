using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.AST;
using System.Reflection.Emit;
using System.IO;

namespace System.Compilers.IL
{
    internal class ILNode
    {
        public virtual bool Match<T>(OpCodeCodes code, out T operand, out List<ILExpression> args)
        {
            operand = default(T);
            args = null;
            return false;
        }

        public virtual bool Match<T>(OpCodeCodes code, out T operand, out ILExpression arg)
        {
            List<ILExpression> args;
            if (Match(code, out operand, out args) && args.Count == 1)
            {
                arg = args[0];
                return true;
            }
            arg = null;
            return false;
        }

        public virtual IEnumerable<ILNode> GetChildren()
        {
            yield break;
        }
    }

    internal class ILLabel : ILNode
    {
        public string Name;

        public static explicit operator NetAstLabel(ILLabel element)
        {
            return new NetAstLabel() { Name = element.Name };
        }

        public override string ToString()
        {
            return Name + ":";
        }

    }

    internal class ILExpression : ILNode
    {
        public OpCodeCodes Code { get; set; }
        public object Operand { get; set; }
        public List<ILExpression> Arguments { get; set; }
        public ILInstruction[] Prefixes { get; set; }
        
        public ILLabel Label { get; set; }
        
        public Type ExpectedType { get; set; }
        public Type InferredType { get; set; }

        public static readonly object AnyOperand = new object();

        public ILExpression(OpCodeCodes code, object operand, List<ILExpression> args)
        {
            this.Code = code;
            this.Operand = operand;
            this.Arguments = new List<ILExpression>(args);
        }

        public ILExpression(OpCodeCodes code, object operand, params ILExpression[] args)
        {
            this.Code = code;
            this.Operand = operand;
            this.Arguments = new List<ILExpression>(args);
        }

        public ILInstruction GetPrefix(OpCode code)
        {
            var prefixes = this.Prefixes;
            if (prefixes != null)
            {
                foreach (ILInstruction i in prefixes)
                {
                    if (i.OpCode == code)
                        return i;
                }
            }
            return null;
        }

        public bool IsBranch()
        {
            return this.Operand is NetAstLabel || this.Operand is NetAstLabel[];
        }

        public IEnumerable<NetAstLabel> GetBranchTargets()
        {
            if (this.Operand is NetAstLabel)
            {
                return new NetAstLabel[] { (NetAstLabel)this.Operand };
            }
            else if (this.Operand is NetAstLabel[])
            {
                return (NetAstLabel[])this.Operand;
            }
            else
            {
                return new NetAstLabel[] { };
            }
        }

        public override bool Match<T>(OpCodeCodes code, out T operand, out List<ILExpression> args)
        {
            if (Prefixes == null && Code == code)
            {
                operand = (T)Operand;
                args = Arguments;
                return true;
            }
            operand = default(T);
            args = null;
            return false;
        }

        public override bool Match<T>(OpCodeCodes code, out T operand, out ILExpression arg)
        {
            List<ILExpression> args;
            if (Match(code, out operand, out args) && args.Count == 1)
            {
                arg = args[0];
                return true;
            }
            arg = null;
            return false;
        }

        public override IEnumerable<ILNode> GetChildren()
        {
            foreach (var item in Arguments)
                yield return item;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Code.ToString().ToLower());
            
            if (Operand != null)
                sb.Append(" " + Operand.ToString());

            if (Arguments != null && Arguments.Count > 0)
            {
                sb.Append(String.Format("({0}", Arguments[0]));
                foreach (var item in Arguments.Skip(1))
                {
                    sb.Append(String.Format(", {0}", item));
                }
                sb.Append(")");
            }
            return sb.ToString();
        }
    }

    static class Extensions
    {
        public static IEnumerable<T> GetSelfAndChildrenRecursive<T>(this List<T> elements) where T:ILNode
        {
            List<T> result = new List<T>(16);
            for (int i = 0; i < elements.Count; i++)
            {
                result.AddRange(GetSelfAndChildrenRecursive(elements[i]));
            }

            return result;
        }

        static public IEnumerable<T> GetSelfAndChildrenRecursive<T>(T self) where T:ILNode
        {
            List<T> result = new List<T>(16);
            AccumulateSelfAndChildrenRecursive(self, result);
            return result;
        }

        static void AccumulateSelfAndChildrenRecursive<T>(T self, List<T> list) where T : ILNode 
        {
            if(self is ILExpression)
                list.Add(self);
            foreach (T node in self.GetChildren())
            {
                if (node != null)
                    AccumulateSelfAndChildrenRecursive(node, list);
            }
        }
		
    }
}
