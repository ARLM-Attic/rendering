using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Node = System.Compilers.Decompiler.ExecutionState;
using System.Compilers.AST;
using System.Compilers.ILAST;
using System.Reflection;

namespace System.Compilers
{
    public class ILFlow
    {
        Node startState;
        
        public ILFlow(Node start)
        {
            this.startState = start;

            Compact();
           
        }

        private IEnumerable<Node> ReachFrom(Node start)
        {
            Queue<Node> q = new Queue<Node>();
            q.Enqueue(start);

            HashSet<Node> visited = new HashSet<Node>();

            while (q.Count > 0)
            {
                Node n = q.Dequeue();
                if (!visited.Contains(n))
                {
                    yield return n;
                    visited.Add(n);
                    foreach (var node in n.Next)
                        q.Enqueue(node);
                }
            }
        }

        private bool PotentialLoopStart(Node start, out Node[] loop)
        {
            if (start.Prev.Length != 2 && !(start == this.Start && start.Prev.Length == 1))
            {
                loop = null;
                return false;
            }

            List<Node> reached = new List<Node>(ReachFrom(start));

            if (start == this.Start)
            {
                loop = reached.ToArray();
                return true;
            }

            if (reached.Contains(start.Prev[0]) != reached.Contains(start.Prev[1]))
            {
                loop = reached.ToArray();
                return true;
            }

            loop = null;
            return false;
        }

        private bool PotencialIfStart(Node start, out Node[] trueBody, out Node[] falseBody)
        {
            if (start.Statement is ILAST.ILAstConditionalGoto)
            {
                var reachTrue = ReachFrom(start.Next[0]);
                var reachFalse = ReachFrom(start.Next[1]);

                trueBody = reachTrue.Except(reachFalse).ToArray();
                falseBody = reachFalse.Except(reachTrue).ToArray();
                return true;
            }
            else
            {
                trueBody = null;
                falseBody = null;
                return false;
            }
        }

        #region Expressions Convert

        private NetAstConstantExpression Convert(ILAstConstantExpression exp)
        {
            return new NetAstConstantExpression { Value = exp.Value };
        }

        private NetAstLocalExpression Convert(ILAstLocalExpression exp)
        {
            return new NetAstLocalExpression { LocalInfo = exp.LocalInfo };
        }

        private NetAstArgumentExpression Convert(ILAstArgumentExpression exp)
        {
            return new NetAstArgumentExpression
            {
                ParameterInfo = exp.ParameterInfo
            };
        }

        private NetAstFieldExpression Convert(ILAstFieldExpression exp)
        {
            return new NetAstFieldExpression
            {
                FieldInfo = exp.FieldInfo,
                LeftSide = Convert(exp.LeftSide)
            };
        }

        private NetAstBinaryOperatorExpression Convert(ILAstBinaryOperatorExpression exp)
        {
            return new NetAstBinaryOperatorExpression
            {
                Operator = exp.Operator,
                LeftOperand = Convert (exp.LeftOperand),
                RightOperand = Convert (exp.RightOperand)
            };
        }

        private NetAstUnaryOperatorExpression Convert(ILAstUnaryOperatorExpression exp)
        {
            return new NetAstUnaryOperatorExpression
            {
                Operand = Convert(exp.Operand),
                Operator = exp.Operator
            };
        }

        private NetAstConvertOperatorExpression Convert(ILAstConvertOperatorExpression exp)
        {
            return new NetAstConvertOperatorExpression
            {
                TargetType = exp.TargetType,
                Operand = Convert (exp.Operand)
            };
        }

        private NetAstMethodCallExpression Convert(ILAstMethodCallExpression exp)
        {
            return new NetAstMethodCallExpression
            {
                MethodBase = exp.MethodBase,
                Arguments = exp.Arguments.Select(a => Convert (a)).ToArray ()
            };
        }

        private NetAstConstructorCallExpression Convert(ILAstConstructorCallExpression exp)
        {
            return new NetAstConstructorCallExpression
            {
                MethodBase = exp.MethodBase,
                Arguments = exp.Arguments.Select(a => Convert(a)).ToArray()
            };
        }

        private NetAstInitObjectExpression Convert(ILAstInitObjectExpression exp)
        {
            return new NetAstInitObjectExpression { TargetType = exp.TargetType };
        }

        private NetAstExpression Convert(ILAstExpression node)
        {
            return Convert<NetAstExpression>(node);
        }

        #endregion

        #region Return Convert

        private NetAstReturn Convert(ILAstReturn node)
        {
            return new NetAstReturn() { Expression = Convert(node.Expression) };
        }

        #endregion

        #region Assignaments

        private NetAstFieldAssignament Convert(ILAstFieldAssignament assignament)
        {
            return new NetAstFieldAssignament
            {
                FieldInfo = assignament.FieldInfo,
                LeftSide = Convert(assignament.LeftSide),
                Value = Convert(assignament.Value)
            };
        }

        private NetAstLocalAssignament Convert(ILAstLocalAssignament assignament)
        {
            return new NetAstLocalAssignament
            {
                LocalInfo = assignament.LocalInfo,
                Value = Convert (assignament.Value)
            };
        }

        private NetAstArgumentAssignament Convert(ILAstArgumentAssignament assignament)
        {
            return new NetAstArgumentAssignament
            {
                ParameterInfo = assignament.ParameterInfo,
                Value = Convert(assignament.Value)
            };
        }

        private NetAstAssignament Convert(ILAstAssignament node)
        {
            return Convert<NetAstAssignament>(node);
        }

        #endregion

        private T Convert<T>(ILAstNode node) where T:NetAstNode
        {
            MethodInfo converter = GetType().GetMethod("Convert", BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder, new Type[] {
                node.GetType ()
            }, null);

            return (T)converter.Invoke(this, new object[] { node });
        }

        #region Block Of Instructions

        private NetAstIf ConvertIf(ILAstConditionalGoto conditional, Node[] trueBody, Node[] falseBody)
        {
            return new NetAstIf
            {
                Condition = Convert(conditional.Condition),
                WhenTrue = ConvertBlock(trueBody),
                WhenFalse = ConvertBlock(falseBody)
            };
        }

        IEnumerable<Node> Exits(Node[] il)
        {
            HashSet<Node> exits = new HashSet<Node>();

            foreach (var n in il)
                foreach (var node in n.Next)
                    if (!il.Contains(node))
                        if (!(node.Statement is ILAstReturn))
                            if (!exits.Contains(node))
                                exits.Add(node);
            
            return exits;
        }

        private NetAstBlock ConvertBlock(Node[] il)
        {
            List<NetAstNode> instructions = new List<NetAstNode>();

            Node start = il[0];

            return new NetAstBlock() { Instructions = instructions };
        }

        #endregion

        public Node Start { get { return startState; } }

        private void Remove(Node state)
        {
            if (state.Next.Length > 1)
                throw new InvalidOperationException();

            if (state == startState)
                this.startState = state.Next[0];

            foreach (var prev in state.Prev)
                prev.Next = prev.Next.Except(new Node[] { state }).Union(state.Next).ToArray();
            foreach (var next in state.Next)
                next.Prev = next.Prev.Except(new Node[] { state }).Union(state.Prev).ToArray();
        }

        public IEnumerable<Node> AllNodes
        {
            get
            {
                HashSet<Node> nodes = new HashSet<Node>();

                Queue<Node> q = new Queue<Node>();
                q.Enqueue(startState);

                while (q.Count > 0)
                {
                    Node n = q.Dequeue();
                    if (!nodes.Contains(n))
                    {
                        yield return n;

                        nodes.Add(n);

                        foreach (var next in n.Next)
                            q.Enqueue(next);
                    }
                }
            }
        }

        private void Compact()
        {
            List<Node> toRemove = new List<Node>();

            foreach (var n in AllNodes)
                if (n.Statement == null || (n.Statement is ILAstGoto && !(n.Statement is ILAstConditionalGoto)))
                    toRemove.Add(n);

            foreach (var n in toRemove)
                Remove(n);
        }
    }
}
