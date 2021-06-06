using ProjectFormeleMethodes.ConversionEngines.Minimizer.Models;
using ProjectFormeleMethodes.NDFA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.Minimizer
{
    public class HopcroftEngine
    {
        public Automata<string> MinimizeDFA(Automata<string> dfaToOptimize)
        {
            // create a new dfa object, this is where the DFA ultimately will end up in
            var dfaOptimal = new Automata<string>();
            // - Get all normal state ids (non-end and start states)
            SortedSet<string> normalStateIds = dfaToOptimize.GetNormalStates();
            // - Get all end-state ids
            SortedSet<string> endStateIds = dfaToOptimize.FinalStates;
            // - Get all start-state ids
            SortedSet<string> startStateIds = dfaToOptimize.StartStates;

            // Create a Partition
            PartitionTable partitionStart = new PartitionTable(dfaToOptimize); // add all the available states to the partition 
            // Assign new pieces to the partition
            partitionStart.AddRowsToPartitionTable(startStateIds, StateType.Start); // Assign the normal states to a piece
            partitionStart.AddRowsToPartitionTable(normalStateIds, StateType.Normal); // Assign the normal states to a piece 
            partitionStart.AddRowsToPartitionTable(endStateIds, StateType.End); // Assign the normal states to a piece 

            partitionStart.SetCorrectDesignatedLetters(); // After filling in the rows, set the rows to their correct designated letter

            // the partition is now ready to be used and search for equivalent nodes     
            // create some variables to prepare optimization

            var newPTable = OptimizePartitionTable(partitionStart);

            // ** method to create optimal DFA machine **
            return dfaOptimal;
        }

        public PartitionTable OptimizePartitionTable(PartitionTable partitionTable)
        {
            StateEquivalencyModel equivalencyModel;
            List<Tuple<string, StateEquivalencyModel>> models = new List<Tuple<string, StateEquivalencyModel>>();

            // Count the amount of letter occurances
            foreach (var state in partitionTable.StateLetters)
            {
                equivalencyModel = new StateEquivalencyModel();
                foreach (var item in partitionTable.GetRowsByState(state.LetterAssigned, state.State))
                {
                    try
                    {
                        equivalencyModel.SymbolOccurence.Add(item.Item2.DesignatedLetter, 1);
                    }
                    catch (ArgumentException )
                    {
                        equivalencyModel.SymbolOccurence[item.Item2.DesignatedLetter]++;
                    }
                }
                models.Add(new Tuple<string, StateEquivalencyModel>(state.State,equivalencyModel));
            }

            PartitionTable newTable = createNewPartitionTable(models);

            return newTable;
        }

        // TODO check method
        private void assignEquivalency(List<Tuple<string, StateEquivalencyModel>> oldModels, ref List<List<string>> newPairs)
        {
            if (oldModels.Count() > 0)
            {
                List<string> states = new List<string>();
                Tuple<string, StateEquivalencyModel> checkerModel = oldModels[0];
                oldModels.Remove(checkerModel);
                
                // add the removed state to the states, this state will be used to check if states are equal
                states.Add(checkerModel.Item1);

                foreach (var item in oldModels)
                {
                    if (item.Equals(checkerModel))
                    {
                        states.Add(item.Item1);
                    }
                }
                newPairs.Add(states);

                if (oldModels.Count == 1)
                {
                    states = new List<string>();
                    states.Add(oldModels[0].Item1);
                }
                assignEquivalency(oldModels, ref newPairs);
            }            
        }

        private PartitionTable createNewPartitionTable(List<Tuple<string, StateEquivalencyModel>> models)
        {
            PartitionTable partitionTable = null;
            List<Tuple<string, StateEquivalencyModel>> oldModels = new List<Tuple<string, StateEquivalencyModel>>(models);
            List<List<string>> newPairs = new List<List<string>>();

            assignEquivalency(models, ref newPairs);

            return partitionTable;
        }
    }
}
