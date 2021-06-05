using ProjectFormeleMethodes.NDFA.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.Minimizer.models
{
    /// <summary>
    /// The class which keeps track of the individual blocks, which are added to a partition
    /// </summary>
    public class Block : IComparable<Block>
    {
        private string blockId; // a Partition id, like A, B, etc.. 
        private List<BlockRow> rows; // the transitions
        private SortedSet<string> states; // contains all the states which are inside of the block
        private bool blockIsEndState; // shows if block is endstate or not

        public Block(string blockId, SortedSet<string> states, bool blockIsEndState)
        {
            this.rows = new List<BlockRow>();
            this.blockId = blockId;
            this.states = states;
            this.blockIsEndState = blockIsEndState;
        }

        public bool IsBlockEndState()
        {
            return this.blockIsEndState;
        }

        public int CompareTo(Block other)
        {
            return this.blockId.CompareTo(other.blockId);
        }

        public string GetBlockId()
        {
            return this.blockId;
        }

        public void AddBlockRowToPartition(BlockRow row)
        {
            this.rows.Add(row);
        }

        public List<BlockRow> GetBlockRows()
        {
            return this.rows;
        }

        public SortedSet<string> GetStates()
        {
            return this.states;
        }

        public string GetBlockRowString()
        {
            string result = "";
            foreach (var item in rows)
            {
                result += item.ToString() + ": ";
            }
            return result;
        }

        public string GetStatesString()
        {
            string result = "";

            foreach (var state in this.states)
            {
                result += state + ",";
            }

            return result;
        }

        public override string ToString()
        {
            return "BlockID: " + this.blockId + ", States: " + GetStatesString();
        }
    }
}
