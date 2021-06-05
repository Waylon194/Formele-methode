using ProjectFormeleMethodes.NDFA.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.Minimizer.models
{
    public class BlockRow
    {
        public string BlockId { get; private set; } // the id of the block, so an A, B, etc..
        public Transition<string> Transition { get; private set; } // the transitions of the state 

        public BlockRow(string blockId, Transition<string> transitions)
        {
            this.BlockId = blockId;
            this.Transition = transitions;
        }

        public override string ToString()
        {
            return "BlockID: " + this.BlockId  + ", " + Transition.ToString();
        }
    }
}
