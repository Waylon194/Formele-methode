using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.Minimizer.models
{
    public class PartitionOld
    {
        private SortedSet<PartitionPieceOld> pieces; // the container for the blocks
        private SortedSet<string> allStates; // all available states to optimize
        private Automata<string> dfaToOptimize; // a link to the automata to 
        private char pieceIndex = (char) 65; // 65, equals the character 'A'

        public PartitionOld(SortedSet<string> allStates, Automata<string> dfa)
        {
            this.pieces = new SortedSet<PartitionPieceOld>();
            this.allStates = allStates;
            this.dfaToOptimize = dfa;
        }

        public SortedSet<string> GetAllStates()
        {
            return this.allStates;
        }

        public SortedSet<PartitionPieceOld> GetAllPieces()
        {
            return this.pieces;
        }

        public Automata<string> GetAutomata()
        {
            return this.dfaToOptimize;
        }

        public void SetAvailableStates(SortedSet<string> states)
        {
            this.allStates = states;
        }

        public void AddTransitionsToPartitionPiece(PartitionPieceOld piece, string stateId, List<Transition<string>> transitions)
        {
            foreach (var transition in transitions)
            {
                piece.AddRowToPartitionPiece(stateId, new RowPieceOld(piece.GetPieceId(), transition));
            }
        }

        public PartitionPieceOld GetPartitionPieceById(string piece)
        {
            foreach (var block in this.pieces)
            {
                if (block.GetPieceId().Equals(piece))
                {
                    return block;
                }
            }
            Console.WriteLine("There is no block with id: {0}", piece);
            return null;
        }

        public void AddPieceToPartition(SortedSet<string> stateIds, bool isEndStateType)
        {
            PartitionPieceOld piece = new PartitionPieceOld(this.pieceIndex.ToString(), stateIds, isEndStateType);

            foreach (var sId in stateIds)
            {
                AddTransitionsToPartitionPiece(piece, sId, this.dfaToOptimize.GetTransition(sId));
            }
            this.pieces.Add(piece);

            this.pieceIndex++; // add +1 to the char to change letter, e.g. A --> B
        }

        public string GetAllPiecesString()
        {
            string result = "";

            foreach (var piece in this.pieces)
            {
                result += piece.GetPieceId() + ",";
            }

            return result;
        }

        public SortedSet<PartitionPieceOld> GetAllPiecesExcept(PartitionPieceOld pieceToXclude)
        {
            SortedSet<PartitionPieceOld> nonRemovedPieces = new SortedSet<PartitionPieceOld>(this.pieces);
            nonRemovedPieces.Remove(pieceToXclude);
            return nonRemovedPieces;
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
            return "BlocksInPartition: {" + GetAllPiecesString() + "} ,StatesOfDFA: {" + GetAllStatesString() + "}";
        }
    }
}
