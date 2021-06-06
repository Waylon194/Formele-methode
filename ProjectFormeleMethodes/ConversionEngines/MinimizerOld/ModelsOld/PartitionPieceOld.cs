using ProjectFormeleMethodes.NDFA.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.Minimizer.models
{
    /// <summary>
    /// The class which keeps track of the individual piece, which is added to a partition
    /// </summary>
    public class PartitionPieceOld : IComparable<PartitionPieceOld>
    {
        private bool pieceContainsEndStates; // shows if block is endstate or not

        private string pieceId; // a Partition id, like A, B, etc.. 

        private List<Tuple<string, RowPieceOld>> rows; // the transitions
        private SortedSet<string> states; // contains all the states which are inside of the block
        private List<ProcessedRowPieceOld> processedRowPieces; // here data gets saved in the format of STATE, PieceID, PieceID (e.g. q1, A, A)

        public PartitionPieceOld(string blockId, SortedSet<string> states, bool blockIsEndState)
        {
            this.rows = new List<Tuple<string, RowPieceOld>>();
            this.pieceId = blockId;
            this.states = states;
            this.pieceContainsEndStates = blockIsEndState;
        }

        public List<ProcessedRowPieceOld> GetProcessedRows()
        {
            return this.processedRowPieces;
        }

        public void SetProcessedRowPieces(List<ProcessedRowPieceOld> pRowPieces)
        {
            this.processedRowPieces = pRowPieces;
        }

        public bool PieceContainsEndStates()
        {
            return this.pieceContainsEndStates;
        }

        public int CompareTo(PartitionPieceOld other)
        {
            return this.pieceId.CompareTo(other.pieceId);
        }

        public string GetPieceId()
        {
            return this.pieceId;
        }

        public void AddRowToPartitionPiece(string state, RowPieceOld row)
        {
            this.rows.Add(new Tuple<string, RowPieceOld>(state, row));
        }

        public List<Tuple<string, RowPieceOld>> GetRows()
        {
            return this.rows;
        }

        public SortedSet<string> GetStates()
        {
            return this.states;
        }

        public string GetRowString()
        {
            string result = "";
            foreach (var item in this.rows)
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
            return "BlockID: " + this.pieceId + ", States: " + GetStatesString();
        }
    }
}
