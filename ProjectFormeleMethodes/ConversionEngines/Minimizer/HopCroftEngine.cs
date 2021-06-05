using ProjectFormeleMethodes.ConversionEngines.Minimizer.models;
using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines
{
    // minimizer https://github.com/MaurodeLyon/Formele-methoden/blob/master/Formele%20methoden/HopcroftAlgorithm.cs <- helpfull link to the algorithm
    public class HopCroftEngine
    {
        public HopCroftEngine()
        {

        }

        public Automata<string> MinimizeDFA(Automata<string> dfaToOptimize)
        {
            var dfaOptimal = new Automata<string>();
            // First start with assigning the partition
            // Create a Partition

            // - Get all normal state ids (start + non-end states)
            SortedSet<string> normalStateIds = dfaToOptimize.GetNormalStates();
            // - Get all end-state ids
            SortedSet<string> endStateIds = dfaToOptimize.FinalStates;

            // Assign a new Block to the partition
            Partition partitionStart = new Partition(dfaToOptimize.States, dfaToOptimize); // add all the available states to the partition 
            partitionStart.AddBlockToPartition(normalStateIds, false); // Assign the normal states to a block 
            partitionStart.AddBlockToPartition(endStateIds, true); // Assign the normal states to a block 

            // the partition is now ready to be used and search for equivalent nodes     
            // create some variables to prepare optimization
            optimizePartition(partitionStart, true);

            return dfaOptimal;
        }

        private string getProperTransitionBlockId(string toState, Block blockToSearch, Partition partition)
        {
            foreach (var state in blockToSearch.GetStates())
            {
                if (toState.Equals(state))
                {
                    return blockToSearch.GetBlockId();
                }
            }
            foreach (var block in partition.GetAllBlocksExcept(blockToSearch))
            {
                foreach (var state in block.GetStates())
                {
                    if (toState.Equals(state))
                    {
                        return block.GetBlockId();
                    }
                }
            }
            return null; // if this gets returned something went wrong
        }

        private Partition optimizePartition(Partition inputPartition, bool firstIteration)
        {
            if (!firstIteration)
            {
                // check for optimized partition
                bool optimized = checkPartitionOptimized(inputPartition);
                if (optimized)
                {
                    return inputPartition;
                }
            }
            // setup the temporaryRows list
            List<Block> temporaryBlocks = new List<Block>();

            // assign the proper block letters to the rows
            foreach (var block in inputPartition.GetAllBlocks())
            {
                Block newBlock = new Block(block.GetBlockId(), block.GetStates(), block.IsBlockEndState());
                foreach (var row in block.GetBlockRows())
                {
                    string blockIdToAssign = getProperTransitionBlockId(row.Transition.ToState, block, inputPartition);
                    newBlock.AddBlockRowToPartition(new BlockRow(blockIdToAssign, row.Transition));
                }
                temporaryBlocks.Add(newBlock);
            }

            // after the rows are designated
            Partition newPartition = CreateNewPartition(inputPartition, temporaryBlocks);

            return newPartition; // a safety measure to  
            return optimizePartition(newPartition, false); // since this is not the first iteration, the partition gets optimized as normal, and a new rows model gets spit out
        }

        private Partition CreateNewPartition(Partition oldPartition, List<Block> temporaryBlocks)
        {
            // first create the new partition object
            Partition newPartition = new Partition(oldPartition.GetAllStates(), oldPartition.GetAutomata());

            List<Block> blocks = new List<Block>(); // these are the new blocks of the new partition

            foreach (var block in temporaryBlocks)
            {
                if (block.GetBlockRows().Count != 1) // if it does not contain a single entry, a block can be checked for equivalency 
                {
                    blocks.AddRange(replaceEquivalentBlocks(block)); // add new blocks which are created from the list
                }
            }

            return newPartition;
        }

        private List<Block> replaceEquivalentBlocks(Block block)
        {
            List<Block> newBlocks = new List<Block>();

            //foreach (var state in block.GetStates())
            //{
            //    foreach (var subRow in block.GetBlockRows())
            //    {

            //    }
            //}

            return null;
        }

        private bool checkPartitionOptimized(Partition inputPartition)
        {
            return false;
        }
    }
}
