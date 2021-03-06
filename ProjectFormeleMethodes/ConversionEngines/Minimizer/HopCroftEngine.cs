using ProjectFormeleMethodes.ConversionEngines.Minimizer.Models;
using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectFormeleMethodes.ConversionEngines.Minimizer
{
    public class HopcroftEngine
    {
        bool equivalencyOccured = false;
        int maxTries = 30;
        int currentTries = 0;

        public Automata<string> MinimizeDFA(Automata<string> dfaToOptimize)
        {
            // safety check for when epsilon closures are detected, this cannot occur in a DFA, and so optimization cannot and should not start
            if (dfaToOptimize.Symbols.Contains(Transition<string>.EPSILON))
            {
                return null;
            }

            // - Get all start-state ids
            Dictionary<string, StateSuperType> startStates = new Dictionary<string, StateSuperType>();
            foreach (var item in dfaToOptimize.StartStates)
            {
                startStates.Add(item, StateSuperType.Start);
            }

            // - Get all normal state ids (non-end and start states)
            Dictionary<string, StateSuperType> normalStates = new Dictionary<string, StateSuperType>();
            foreach (var item in dfaToOptimize.GetNormalStates())
            {
                normalStates.Add(item, StateSuperType.Normal);
            }

            // - Get all end-state ids
            Dictionary<string, StateSuperType> endStates = new Dictionary<string, StateSuperType>();
            foreach (var item in dfaToOptimize.FinalStates)
            {
                endStates.Add(item, StateSuperType.End);
            }

            // Create a Partition
            PartitionTable partitionStart = new PartitionTable(dfaToOptimize); // add all the available states to the partition 
            // Assign new pieces to the partition
            partitionStart.AddRowsToPartitionTable(startStates, StateSubType.NonEnd ,true, true); // Assign the normal states to a piece, toggle true for changing the letter
            partitionStart.AddRowsToPartitionTable(normalStates, StateSubType.NonEnd, false, false); // Assign the normal states to a piece, toggle false for keeping the letter
            partitionStart.AddRowsToPartitionTable(endStates, StateSubType.End, false, true); // Assign the normal states to a piece, toggle true for changing the letter

            partitionStart.SetCorrectDesignatedLetters(); // After filling in the rows, set the rows to their correct designated letter

            // the partition is now ready to be used and search for equivalent nodes     
            var newPTable = optimizePartitionTable(partitionStart);

            // the partition table is optimized and ready to be used and placed as a new DFA automata model
            return createAutomataFromPartitionTable(newPTable);
        }

        private Automata<string> createAutomataFromPartitionTable(PartitionTable partitionTable)
        {
            // create a new automata object where the new transitions can be placed in
            Automata<string> optimizedAutomata = new Automata<string>(partitionTable.DFA.Symbols);

            foreach (var item in partitionTable.Rows)
            {
                switch (item.Item2.SuperType)
                {
                    case StateSuperType.Start:
                        optimizedAutomata.DefineAsStartState(item.Item2.RowLetter.ToString());
                        break;
                    case StateSuperType.End:
                        optimizedAutomata.DefineAsFinalState(item.Item2.RowLetter.ToString());
                        break;
                    default:
                        break;
                }
                optimizedAutomata.AddTransition(new Transition<string>(item.Item2.RowLetter.ToString(), item.Item2.Symbol, partitionTable.GetCorrectLetterByState(item.Item2.ToState)));
            }
            return optimizedAutomata;
        }

        private PartitionTable optimizePartitionTable(PartitionTable partitionTable)
        {
            this.equivalencyOccured = false;
            var models = createEquivalencyModels(partitionTable);
            PartitionTable newTable = createNewPartitionTable(models, partitionTable.DFA);
            return newTable;
        }

        private List<Tuple<string, StateEquivalencyModel>> createEquivalencyModels(PartitionTable partitionTable)
        {
            List<Tuple<string, StateEquivalencyModel>> models = new List<Tuple<string, StateEquivalencyModel>>();
            StateEquivalencyModel equivalencyModel;
            // Count the amount of letter occurances
            foreach (var state in partitionTable.StateLetters)
            {
                equivalencyModel = new StateEquivalencyModel();
                foreach (var item in partitionTable.GetRowsByState(state.LetterAssigned, state.State))
                {
                    try
                    {
                        equivalencyModel.IsStartState = item.Item2.isStartState;
                        equivalencyModel.SubType = item.Item2.SubType;
                        equivalencyModel.SymbolOccurence.Add(item.Item2.DesignatedLetter, 1);
                        equivalencyModel.SuperType = item.Item2.SuperType;
                    }
                    catch (ArgumentException)
                    {
                        equivalencyModel.SymbolOccurence[item.Item2.DesignatedLetter]++;
                    }
                }
                models.Add(new Tuple<string, StateEquivalencyModel>(state.State, equivalencyModel));
            }
            return models;
        }

        private PartitionTable createNewPartitionTable(List<Tuple<string, StateEquivalencyModel>> models, Automata<string> dfa)
        {
            PartitionTable partitionTable = new PartitionTable(dfa);
            List<Tuple<string, StateEquivalencyModel>> oldModels = new List<Tuple<string, StateEquivalencyModel>>(models);
            List<Tuple<Dictionary<string, StateSuperType>, StateEquivalencyModel>> newPairs = new List<Tuple<Dictionary<string, StateSuperType>, StateEquivalencyModel>>();

            createNewPartitionPairs(oldModels, ref newPairs);

            foreach (var pairs in newPairs)
            {
                // add new rows to new partitionTable
                partitionTable.AddRowsToPartitionTable(pairs.Item1, pairs.Item2.SubType, pairs.Item2.IsStartState, true);
            }

            if (!equivalencyOccured || currentTries == maxTries) // this boolean value changes, if no changes have occured, signaling the partition is optimized
            {
                return partitionTable;
            }
            currentTries++;

            return optimizePartitionTable(partitionTable);
        }

        private int createNewPartitionPairs(List<Tuple<string, StateEquivalencyModel>> eqModels, ref List<Tuple<Dictionary<string, StateSuperType>, StateEquivalencyModel>> newPairs)
        {
            if (eqModels.Count() > 0)
            {
                // this list keeps track of which items are identical and should be a new state togethers
                Dictionary<string,StateSuperType> newStates = new Dictionary<string, StateSuperType>();

                // gets a single item which is used to check for duplicate states
                Tuple<string, StateEquivalencyModel> currentEqModel = eqModels[0];
                
                // remove the currentEqModel from the list, to remove duplicate values
                eqModels.Remove(currentEqModel);

                // the models which are marked for removal, after they have been added to the new pairs list
                List<Tuple<string, StateEquivalencyModel>> modelsToRemove = new List<Tuple<string, StateEquivalencyModel>>(); 

                newStates.Add(currentEqModel.Item1, currentEqModel.Item2.SuperType); // add a new state 

                foreach (var item in eqModels)
                {
                    if (item.Item2.SymbolOccurence.SequenceEqual(currentEqModel.Item2.SymbolOccurence) && item.Item2.SubType.Equals(currentEqModel.Item2.SubType))
                    {
                        newStates.Add(item.Item1, item.Item2.SuperType);
                        modelsToRemove.Add(item);
                        this.equivalencyOccured = true; // change the boolean to true, to signal an optimization occured
                    }
                }
                if (modelsToRemove.Count() > 0) // if there are entries inside the list, remove all the entries 
                {
                    foreach (var itemToRemove in modelsToRemove)
                    {
                        eqModels.Remove(itemToRemove);
                    }
                }
                newPairs.Add(new Tuple<Dictionary<string, StateSuperType>, StateEquivalencyModel>(newStates, currentEqModel.Item2));
                createNewPartitionPairs(eqModels, ref newPairs);
                return 1;
            }
            return 0;
        }        
    }
}