using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.AST;

namespace System.Compilers.Optimizers
{
    public class FlattenBlocksOptimizer:Optimizer
    {
        public override void Optimize(NetAstBlock toOptimize)
        {
            FlattenBasicBlocks(toOptimize);
        }

        void FlattenBasicBlocks(NetAstStatement node)
        {
            NetAstBlock block = node as NetAstBlock;
            if (block != null)
            {
                List<NetAstStatement> flatBody = new List<NetAstStatement>();
                foreach (var child in block.GetChildren().Cast<NetAstStatement>())
                {
                    FlattenBasicBlocks(child);
                    if (child is OptBlock)
                    {
                        flatBody.AddRange(child.GetChildren().Cast<NetAstStatement>());
                    }
                    else
                    {
                        flatBody.Add(child);
                    }
                }
                block.EntryGoto = null;
                block.Instructions = flatBody;
            }
            else if (node != null)
            {
                // Recursively find all ILBlocks
                foreach (var child in node.GetChildren().OfType<NetAstStatement>())
                {
                    FlattenBasicBlocks(child);
                }
            }
        }
    }
}
