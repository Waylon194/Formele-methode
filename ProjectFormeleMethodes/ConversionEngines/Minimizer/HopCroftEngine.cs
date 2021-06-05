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

            // - Get all normal state ids (start + non-end states)
            SortedSet<string> normalStateIds = dfaToOptimize.GetNormalStates();
            // - Get all end-state ids
            SortedSet<string> endStateIds = dfaToOptimize.FinalStates;

            // Create a Partition
            Partition partitionStart = new Partition(dfaToOptimize.States, dfaToOptimize); // add all the available states to the partition 
            // Assign new pieces to a partition
            partitionStart.AddPieceToPartition(normalStateIds, false); // Assign the normal states to a piece 
            partitionStart.AddPieceToPartition(endStateIds, true); // Assign the normal states to a piece 

            // the partition is now ready to be used and search for equivalent nodes     
            // create some variables to prepare optimization
            optimizePartition(partitionStart, true);

            // ** method to create optimal DFA machine **
            return dfaOptimal;
        }

        private Partition optimizePartition(Partition inputPartition, bool firstIteration)
        {
            if (!firstIteration)
            {
                // check for optimized partition
                bool optimized = checkPartitionOptimized(inputPartition);
                if (optimized) // if fully optimized return the same partition
                {
                    return inputPartition;
                }
            }
            // setup the temporaryRows list, this list will also contain the processedRowPieces
            List<PartitionPiece> temporaryPieces = new List<PartitionPiece>();

            // assign the proper block letters to the rows
            foreach (var piece in inputPartition.GetAllPieces())
            {
                List<ProcessedRowPiece> pRowPieces = new List<ProcessedRowPiece>();
                PartitionPiece newPiece = new PartitionPiece(piece.GetPieceId(), piece.GetStates(), piece.PieceContainsEndStates());
                foreach (var row in piece.GetRows())
                {
                    string pieceIdToAssign = getProperTransitionPieceId(row.Item2.Transition.ToState, piece, inputPartition);
                    newPiece.AddRowToPartitionPiece(row.Item2.Transition.FromState, new RowPiece(pieceIdToAssign, row.Item2.Transition));
                }
                temporaryPieces.Add(newPiece);

                // start the processing of the created rows, to sort the data neatly inside a class package
                foreach (var state in piece.GetStates()) // now with the new information the rows can be processed
                {
                    ProcessedRowPiece pRowPiece = new ProcessedRowPiece(state);
                    foreach (var item in newPiece.GetRows())
                    {
                        if (state.Equals(item.Item2.Transition.FromState))
                        {
                            pRowPiece.AddPieceId(item.Item2.PieceId);
                        }
                    }
                    pRowPieces.Add(pRowPiece);
                }
                // set and add the processedRowPieces to the list
                newPiece.SetProcessedRowPieces(pRowPieces);
            }

            // after the rows are designated
            Partition newPartition = CreateNewPartition(inputPartition, temporaryPieces);

            return newPartition; // a safety measure to  
            return optimizePartition(newPartition, false); // since this is not the first iteration, the partition gets optimized as normal, and a new rows model gets spit out
        }

        // Sets the correct pieceId 
        private string getProperTransitionPieceId(string toState, PartitionPiece pieceToSearch, Partition partition)
        {
            foreach (var state in pieceToSearch.GetStates())
            {
                if (toState.Equals(state))
                {
                    return pieceToSearch.GetPieceId();
                }
            }
            foreach (var piece in partition.GetAllPiecesExcept(pieceToSearch))
            {
                foreach (var state in piece.GetStates())
                {
                    if (toState.Equals(state))
                    {
                        return piece.GetPieceId();
                    }
                }
            }
            return null; // if this gets returned something went wrong
        }

        private Partition CreateNewPartition(Partition oldPartition, List<PartitionPiece> temporaryBlocks)
        {
            // first create the new partition object
            Partition newPartition = new Partition(oldPartition.GetAllStates(), oldPartition.GetAutomata());

            List<PartitionPiece> pieces = new List<PartitionPiece>(); // these are the new blocks of the new partition

            foreach (var piece in temporaryBlocks)
            {
                if (piece.GetRows().Count != 1) // if it does not contain a single entry, a block can be checked for equivalency 
                {
                    pieces.AddRange(replaceEquivalentPieces(piece)); // add new blocks which are created from the list
                }
            }

            return newPartition;
        }

        private List<PartitionPiece> replaceEquivalentPieces(PartitionPiece piece)
        {
            List<PartitionPiece> newPieces = new List<PartitionPiece>();

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
