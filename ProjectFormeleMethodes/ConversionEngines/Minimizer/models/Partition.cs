using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.Minimizer.models
{
    public class Partition
    {
        private SortedSet<Block> blocks; // the container for the blocks
        private SortedSet<string> allStates; // all available states to optimize
        private Automata<string> toOptimizeDFA; // a link to the automata to 
        private char blockIndex = (char) 65; // 65, equals the character 'A'

        public Partition(SortedSet<string> allStates, Automata<string> dfa)
        {
            this.blocks = new SortedSet<Block>();
            this.allStates = allStates;
            this.toOptimizeDFA = dfa;
        }

        public SortedSet<string> GetAllStates()
        {
            return this.allStates;
        }

        public SortedSet<Block> GetAllBlocks()
        {
            return this.blocks;
        }

        public Automata<string> GetAutomata()
        {
            return this.toOptimizeDFA;
        }

        public void SetAvailableStates(SortedSet<string> states)
        {
            this.allStates = states;
        }

        public void AddTransitionsToBlock(Block block, string stateId, List<Transition<string>> transitions)
        {
            foreach (var transition in transitions)
            {
                block.AddBlockRowToPartition(new BlockRow(block.GetBlockId(), transition));
            }
        }

        public Block GetBlockById(string blockId)
        {
            foreach (var block in this.blocks)
            {
                if (block.GetBlockId().Equals(blockId))
                {
                    return block;
                }
            }
            Console.WriteLine("There is no block with id: {0}", blockId);
            return null;
        }

        public void AddBlockToPartition(SortedSet<string> stateIds, bool isEndStateType)
        {
            Block block = new Block(this.blockIndex.ToString(), stateIds, isEndStateType);

            foreach (var sId in stateIds)
            {
                AddTransitionsToBlock(block, sId, this.toOptimizeDFA.GetTransition(sId));
            }
            this.blocks.Add(block);

            this.blockIndex++; // add +1 to the char to change letter, e.g. A --> B
        }

        public string GetAllBlocksString()
        {
            string result = "";

            foreach (var block in this.blocks)
            {
                result += block.GetBlockId() + ",";
            }

            return result;
        }

        public SortedSet<Block> GetAllBlocksExcept(Block blockToXclude)
        {
            SortedSet<Block> listOfBlocks = new SortedSet<Block>(this.blocks);
            listOfBlocks.Remove(blockToXclude);
            return listOfBlocks;
        }

        public string GetAllStatesString()
        {
            string result = "";

            foreach (var state in this.allStates)
            {
                result += state + ",";
            }

            return result;
        }

        public override string ToString()
        {
            return "BlocksInPartition: {" + GetAllBlocksString() + "} ,StatesOfDFA: {" + GetAllStatesString() + "}";
        }
    }
}
