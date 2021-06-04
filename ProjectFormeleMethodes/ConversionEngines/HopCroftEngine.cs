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
            Partition partitionStart = new Partition();

            // - Get all normal state ids (start + non-end states)
            SortedSet<string> normalStateIds = dfaToOptimize.GetNormalStates();
            // - Get all end-state ids
            SortedSet<string> endStatesIds = dfaToOptimize.FinalStates;

            // Assign a new Block to the partition
            partitionStart.AddBlockToPartition("B", endStatesIds); // Assign the normal states to block B
            partitionStart.AddBlockToPartition("A", normalStateIds); // Assign the normal states to block 
            partitionStart.SetAvailableStates(dfaToOptimize.States);

            // Gather all the Transitions available, starting with the non-end states
            foreach (var nFState in partitionStart.GetBlockById("A").GetStateIds())
            {
                partitionStart.AddTransitionsToBlock("A",dfaToOptimize.GetTransition(nFState));
            }
            // Next, get all the end/final Transitions available 
            foreach (var fState in partitionStart.GetBlockById("B").GetStateIds())
            {
                partitionStart.AddTransitionsToBlock("B", dfaToOptimize.GetTransition(fState));
            }

            // the partition is now ready to be used and search for equivalent nodes     
            // create some variables to prepare optimization
            int currentLetterIndex = 67; // start with the correct letter index;

            optimizePartition(partitionStart);

            return dfaOptimal;
        }  

        public string AssignBlockLetterToTransition(int letterToAssign, Transition<string> transition)
        {
            return "";
        }

        private Partition optimizePartition(Partition partition)
        {
            Partition newPartition = null;

            return newPartition;
        }

        internal class Partition
        {
            private SortedSet<Block> blocks; // the container for the blocks
            private SortedSet<string> allStates; // all available states to optimize

            public Partition()
            {
                this.blocks = new SortedSet<Block>();
            }

            public void SetAvailableStates(SortedSet<string> states)
            {
                this.allStates = states;
            }

            public void AddTransitionsToBlock(string blockId, List<Transition<string>> transitions)
            {
                Block block = GetBlockById(blockId);
                block.GetTransitions().AddRange(transitions);
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

            public void AddBlockToPartition(string blockId, SortedSet<string> stateIds)
            {
                this.blocks.Add(new Block(blockId, stateIds));
            }

            /// <summary>
            /// The class which keeps track of the individual blocks, which are added to a partition
            /// </summary>
            internal class Block : IComparable<Block>
            {
                private string blockId; // a Partition id, like A, B, etc.. 
                private SortedSet<string> stateIds; // the state Ids, like q0, q1 etc...
                private List<Transition<string>> transitions; // the transitions

                public Block(string partitionId, SortedSet<string> stateIds)
                {
                    this.transitions = new List<Transition<string>>();
                    this.blockId = partitionId;
                    this.stateIds = stateIds;
                }

                public Block(string partitionId, SortedSet<string> stateIds, List<Transition<string>> transitions)
                {
                    this.blockId = partitionId;
                    this.stateIds = stateIds;
                    this.transitions = transitions;
                }

                public int CompareTo(Block other)
                {
                    return this.blockId.CompareTo(other.blockId);
                }

                public string GetBlockId()
                {
                    return this.blockId;
                }

                public SortedSet<string> GetStateIds()
                {
                    return this.stateIds;
                }

                public List<Transition<string>> GetTransitions()
                {
                    return this.transitions;
                }
            }
        }        
    }
}
