using ProjectFormeleMethodes.ConversionEngines.NDFAToDFA.Models;
using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.NDFAToDFA
{
    public class NDFAToDFAEngine
    {
        private readonly char EPSILON = 'ɛ';
        private readonly string EMPTY_STATE_ID = "";
        public NDFAToDFAEngine()
        {

        }

        public Automata<string> Convert(Automata<string> ndfa)
        {
            Automata<string> dfa = new Automata<string>(ndfa.Symbols);

            var helperTable = createEmptyTable(ndfa);
            createHelperTable(ndfa, ref helperTable);

            var stateTable = createStateTable(ndfa, helperTable);

            finalizeConversion(stateTable, ref dfa, ndfa);

            return dfa;
        }

        private void finalizeConversion(Table stateTable, ref Automata<string> dfa, Automata<string> ndfa)
        {
            foreach (var state in stateTable.AvailableStates)
            {
                foreach (var column in stateTable.Columns)
                {
                    foreach (var startStateDefiner in ndfa.StartStates)
                    {
                        if (state.Contains(startStateDefiner))
                        {
                            // start state found
                            dfa.DefineAsStartState(state);
                        }
                    }
                    foreach (var finalStateDefiner in ndfa.FinalStates)
                    {
                        if (state.Contains(finalStateDefiner))
                        {
                            // start state found
                            dfa.DefineAsFinalState(state);
                        }
                    }
                    string combinedStates = "";
                    foreach (var item in column.ReachableStates[state])
                    {
                        combinedStates += item;
                    }
                    dfa.AddTransition(new Transition<string>(state, column.Symbol, combinedStates));
                }
            }
            Console.WriteLine();
        }

        private SortedSet<string> getAllTotalStates(Table stateTable)
        {
            SortedSet<string> states = new SortedSet<string>();
            states.Add(stateTable.Columns.FirstOrDefault().ReachableStates.FirstOrDefault().Key);

            foreach (var rows in stateTable.Columns)
            {
                foreach (var singleRow in rows.ReachableStates)
                {
                    var rowStates = singleRow.Value;
                    string rowState = "";
                    foreach (var item in rowStates)
                    {
                        rowState += item;
                    }
                    states.Add(rowState);
                }
            }
            return states;
        }

        private Table createStateTable(Automata<string> ndfa, Table helperTable)
        {
            Table stateTable = createEmptyTable(ndfa);
            stateTable.AvailableStates = getAllTotalStates(helperTable);
            int symbolIndex = 0;

            foreach (var combinedState in stateTable.AvailableStates)
            {
                foreach (var state in helperTable.AvailableStates)
                {
                    foreach (var column in helperTable.Columns)
                    {
                        foreach (var stateData in column.ReachableStates)
                        {
                            if (combinedState.Contains(stateData.Key))
                            {
                                if (!stateTable.Columns[symbolIndex].ReachableStates.ContainsKey(combinedState))
                                {
                                    stateTable.Columns[symbolIndex].AddReachableState(combinedState, column.ReachableStates[stateData.Key]);
                                }
                                stateTable.Columns[symbolIndex].ReachableStates[combinedState].UnionWith(column.ReachableStates[stateData.Key]);
                            }                            
                        }
                        symbolIndex++;
                    }
                    symbolIndex = 0;
                }                
            }
            return stateTable;
        }

        private void GetReachableStatesByEpsilons(Automata<string> ndfa, Transition<string> transition, ref SortedSet<string> reachableStates)
        {
            var transitionsToTraverse = ndfa.GetTransition(transition.ToState).Where(item => item.FromState == transition.ToState).ToList();

            foreach (var transitionToTraverse in transitionsToTraverse)
            {
                if (transitionToTraverse.Symbol.Equals(EPSILON) && transition.Symbol != EPSILON)
                {
                    reachableStates.Add(transitionToTraverse.FromState);
                    GetReachableStatesByEpsilons(ndfa, transitionToTraverse, ref reachableStates);
                }
            }
            if (!transitionsToTraverse.Select(item => item.Symbol).Contains(EPSILON))
            {
                reachableStates.Add(transition.ToState);
            }
            else if (transitionsToTraverse.Count() == 0)
            {
                reachableStates.Add(EMPTY_STATE_ID);
            }
            Console.WriteLine();
        }

        private void createHelperTable(Automata<string> ndfa, ref Table helperTable)
        {
            SortedSet<string> reachableStates = new SortedSet<string>();

            foreach (var state in helperTable.AvailableStates)
            {
                foreach (var column in helperTable.Columns)
                {
                    // search for epsilon closure states
                    var transitionsSymbol = ndfa.GetTransition(state).Where(item => item.Symbol == column.Symbol).ToList();
                    var transitionsEpsilon = ndfa.GetTransition(state).Where(item => item.Symbol == EPSILON && item.FromState == state).ToList();

                    var transitions = transitionsSymbol;
                    transitions.AddRange(transitionsEpsilon);

                    if (transitions != null)
                    {
                        foreach (var transition in transitions)
                        {
                            GetReachableStatesByEpsilons(ndfa, transition, ref reachableStates);

                            if (!column.ReachableStates.ContainsKey(state))
                            {
                                column.AddReachableState(state, reachableStates);
                            }
                            else
                            {
                                column.ReachableStates[state].UnionWith(reachableStates);
                            }
                            reachableStates = new SortedSet<string>();
                        }
                    }
                }
            }
            Console.WriteLine();
        }

        

        private Table createEmptyTable(Automata<string> ndfa)
        {
            Table helpTable = new Table();
            helpTable.AvailableStates = ndfa.States;

            var symbols = ndfa.Symbols;
            symbols.Remove('ɛ'); // remove epsilon from the list of symbols

            foreach (var symbol in ndfa.Symbols)
            {
                helpTable.AddColumn(new TableColumn(symbol));
            }
            return helpTable;
        }
    }
}
