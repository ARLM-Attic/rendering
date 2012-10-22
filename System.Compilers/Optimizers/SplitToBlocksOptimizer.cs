using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.AST;
using System.Diagnostics;

namespace System.Compilers.Optimizers
{
    public class SplitToBlocksOptimizer : Optimizer
    {
        int nextLabelIndex;

        public override void Optimize(NetAstBlock toOptimize)
        {
            SplitToBasicBlocks(toOptimize);
        }
        void SplitToBasicBlocks(NetAstBlock block)
        {
            List<NetAstStatement> basicBlocks = new List<NetAstStatement>();

            OptBlock basicBlock = new OptBlock()
            {
                EntryLabel = new NetAstLabel() { Name = "Block_" + (nextLabelIndex++) }
            };
            basicBlocks.Add(basicBlock);
            block.EntryGoto = new NetAstUnconditionalGoto() { Destination = basicBlock.EntryLabel };

            if (block.Instructions.Count > 0)
            {
                basicBlock.Instructions.Add(block.Instructions[0]);

                for (int i = 1; i < block.Instructions.Count; i++)
                {
                    var lastNode = block.Instructions[i - 1];
                    var currNode = block.Instructions[i];

                    // Insert split
                    if (currNode is NetAstLabel || IsBranch(lastNode) || currNode is NetAstConditionalGoto && basicBlock.Instructions.Count > 0)
                    {
                        var lastBlock = basicBlock;
                        basicBlock = new OptBlock();
                        basicBlocks.Add(basicBlock);

                        if (currNode is NetAstLabel)
                        {
                            // Insert as entry label
                            basicBlock.EntryLabel = (NetAstLabel)currNode;
                        }
                        else
                        {
                            basicBlock.EntryLabel = new NetAstLabel() { Name = "Block_" + (nextLabelIndex++) };
                            basicBlock.Instructions.Add(currNode);
                        }

                        // Explicit branch from one block to other
                        // (unless the last expression was unconditional branch)
                        if (!(lastNode is NetAstUnconditionalGoto))
                        {
                            lastBlock.FallthoughGoto = new NetAstUnconditionalGoto(){ Destination = basicBlock.EntryLabel};
                        }
                    }
                    else
                    {
                        basicBlock.Instructions.Add(currNode);
                    }
                }
            }

            foreach (OptBlock bb in basicBlocks)
            {
                if (bb.Instructions.Count > 0 &&
                    bb.Instructions.Last() is NetAstUnconditionalGoto)
                {
                    Debug.Assert(bb.FallthoughGoto == null);
                    bb.FallthoughGoto = bb.Instructions.Last() as NetAstUnconditionalGoto;
                    bb.Instructions.RemoveAt(bb.Instructions.Count - 1);
                }
            }

            var jumps = block.GetSelfAndChildrenRecursive<NetAstGoto>();

            for (int i = 0; i < basicBlocks.Count; i++)
            {
                var currentBlock = basicBlocks[i] as OptBlock;
                if (currentBlock != null && currentBlock.Instructions.Count == 0)
                {
                    foreach (var jump in jumps)
                    {
                        if (jump.Destination.Equals(currentBlock.EntryLabel))
                            jump.Destination = currentBlock.FallthoughGoto.Destination;
                    }
                    
                    foreach (var bb in basicBlocks)
                    {

                        if (bb is OptBlock)
                        {
                            var optBlock = bb as OptBlock;
                            if (optBlock.FallthoughGoto != null && optBlock.FallthoughGoto.Destination.Equals(currentBlock.EntryLabel))
                                optBlock.FallthoughGoto.Destination = currentBlock.FallthoughGoto.Destination;
                        }
                    }

                    basicBlocks.RemoveAt(i--);
                }
            }

            block.Instructions = basicBlocks;
            return;
        }
    }
}
