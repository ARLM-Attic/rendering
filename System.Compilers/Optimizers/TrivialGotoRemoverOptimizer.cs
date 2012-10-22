using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.AST;

namespace System.Compilers.Optimizers
{
    public class TrivialGotoRemoverOptimizer : Optimizer
    {
        public override void Optimize(NetAstBlock toOptimize)
        {
            Dictionary<NetAstLabel, int> labelRefCount = new Dictionary<NetAstLabel, int>();
            foreach (var target in toOptimize.GetSelfAndChildrenRecursive<NetAstNode>().SelectMany(e => GetBranchTargets(e)))
            {
                labelRefCount[target] = labelRefCount.GetOrDefault(target) + 1;
            }

            foreach (NetAstBlock block in toOptimize.GetSelfAndChildrenRecursive<NetAstBlock>().ToList())
            {
                var body = block.Instructions;
                var newBody = new List<NetAstStatement>(body.Count);
                for (int i = 0; i < body.Count; i++)
                {
                    NetAstLabel target = body[i] is NetAstUnconditionalGoto ? (body[i] as NetAstUnconditionalGoto).Destination : null;
                    if (target != null && i + 1 < body.Count && body[i + 1].Equals(target))
                    {
                        // Ignore the branch  TODO: ILRanges
                        if (labelRefCount[target] == 1)
                            i++;  // Ignore the label as well
                    }
                    else
                    {
                        newBody.Add(body[i]);
                    }
                }
                block.Instructions = newBody;
            }
        }
    }
}
