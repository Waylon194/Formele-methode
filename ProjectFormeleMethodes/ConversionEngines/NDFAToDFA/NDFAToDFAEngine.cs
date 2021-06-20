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

            return dfa;
        }

        private void GetReachableStatesByEpsilons(Automata<string> ndfa, Transition<string> transition, ref SortedSet<string> reachableStates)
        {
            var transitionsToTraverse = ndfa.GetTransition(transition.ToState).Where(item => item.FromState == transition.ToState).ToList();

            foreach (var transitionToTraverse in transitionsToTraverse)
            {
                if (transitionToTraverse.Symbol.Equals(EPSILON))
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
