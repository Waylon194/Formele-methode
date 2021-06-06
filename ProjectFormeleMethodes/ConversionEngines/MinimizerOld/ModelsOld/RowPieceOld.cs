using ProjectFormeleMethodes.NDFA.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.Minimizer.models
{
    public class RowPieceOld
    {
        public string PieceId { get; private set; } // the id of the RowPiece, so an A, B, etc..
        public Transition<string> Transition { get; private set; } // the transitions of the state 

        public RowPieceOld(string pieceId, Transition<string> transitions)
        {
            this.PieceId = pieceId;
            this.Transition = transitions;
        }

        public override string ToString()
        {
            return "BlockID: " + this.PieceId  + ", " + Transition.ToString();
        }
    }
}
