using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;
using System.Collections.Generic;
using System.Linq;

namespace ProjectFormeleMethodes.ConversionEngines
{
    public class NDFAToDFAEngine
    {
        public Automata<string> Convert(Automata<string> ndfa)
        {
            Automata<string> dfa = new Automata<string>(ndfa.Symbols);
            string combinedStartState = "";
            SortedSet<string> completeStartState = new SortedSet<string>();

            bool isFinalState = false;
            // Loop through all the available start states from the ndfa and create a list with them + their epsilon-linked states
            foreach (string startState in ndfa.StartStates)
            {
                RetrieveEpsilonIncludedState(startState, ndfa, ref completeStartState);
            }

            //Turn sortedset into a string with all its states
            foreach (string s in completeStartState)
            {
                combinedStartState += s + "_";
                if (ndfa.FinalStates.Contains(s))
                    isFinalState = true;
            }

            //trim last "_" off of string
            combinedStartState = combinedStartState.TrimEnd('_');
            //Start conversion
            ConvertState(combinedStartState, ref dfa, ref ndfa);
            // Define combinedStartState as one and only start state in dfa
            dfa.DefineAsStartState(combinedStartState);
            if (isFinalState)
            {
                dfa.DefineAsFinalState(combinedStartState);
            }

            // Add a symbol loop to the failstate if one is created during conversion.
            if (dfa.States.Contains("F"))
            {
                foreach (char route in dfa.Symbols)
                {
                    dfa.AddTransition(new Transition<string>("F", route, "F"));
                }
            }
            return finaliseConversion(dfa);
        }

        private Automata<string> finaliseConversion(Automata<string> merged)
        {
            Automata<string> finalisedMerge = new Automata<string>(merged.Symbols);

            foreach (Transition<string> t in merged.Transitions)
            {
                finalisedMerge.AddTransition(new Transition<string>(t.FromState.Replace("_", string.Empty), t.Symbol, t.ToState.Replace("_", string.Empty)));
            }

            foreach (string startState in merged.StartStates)
            {
                finalisedMerge.DefineAsStartState(startState.Replace("_", string.Empty));
            }

            foreach (string finalState in merged.FinalStates)
            {
                finalisedMerge.DefineAsFinalState(finalState.Replace("_", string.Empty));
            }
            return finalisedMerge;
        }

        private void RetrieveEpsilonIncludedState(string state, Automata<string> auto, ref SortedSet<string> subStateList)
        {
            //Add given state to the given substatelist
            subStateList.Add(state);

            //retrieve the list of transitions from the given state
            List<Transition<string>> trans = auto.GetTransition(state);

            //Loop through all the transitions in search of epsilon routes. If a epsilon route is found that is not yet included in the list the route and its subsequent epsilon routes-
            //Will be added to substatelist through recursion.
            foreach (Transition<string> t in trans)
            {
                if (t.Symbol == 'ɛ' && !subStateList.Contains(t.ToState))
                {
                    RetrieveEpsilonIncludedState(t.ToState, auto, ref subStateList);
                }
            }
            /////Handy should we ever need to remove duplicates from an array without the use of sortedset<>
            //string[] individualSubStates = (completeState.Split('_')).Distinct().ToArray();
        }

        private bool CheckExistingRouteForChar(string currentState, char symbol, Automata<string> dfa)
        {
            List<Transition<string>> currentTrans = dfa.GetTransition(currentState);
            foreach (Transition<string> t in currentTrans)
            {
                if (t.Symbol == symbol)
                {
                    return true;
                }
            }
            return false;
        }

        private int CheckAvailableRoutes(string[] states, char symbol, Automata<string> ndfa)
        {
            //array which shows how many possible routes there are for each sub-state
            int[] possibleRoutesPerState = new int[states.Length];
            //// value that shows the amount of routes the ndfa has for all the substates combined.
            int correctAmountOfRoutes = 0;

            //reads ndfa for possible routes, saves maximum amount of accessible routes to correctAmountOfRoutes
            foreach (string state in states)
            {
                if (ndfa.GetTransition(state).Count(transition => transition.Symbol == symbol) > correctAmountOfRoutes)
                {
                    correctAmountOfRoutes = ndfa.GetTransition(state).Count(transition => transition.Symbol == symbol);
                }
            }
            return correctAmountOfRoutes;
        }

        //Fills toState string with correct TOSTATE, returns true or false whether or not this new TOSTATE should be a final state
        private bool GenerateToState(ref string toState, string[] states, char symbol, Automata<string> ndfa)
        {
            //boolean that will save whether this new TOSTATE needs to be a finalstate
            bool isFinalState = false;
            //Set of all the substates that need to be combined. this set does also include all states reached through epsilon routes
            SortedSet<string> newStates = new SortedSet<string>();

            //Loop through all the substates 
            foreach (string state in states)
            {
                //ndfa transitions for state
                List<Transition<string>> trans = ndfa.GetTransition(state);

                //This loop goes through all the aforementioned transitions
                //to see if there are routes with the correct symbol that need to be added to the new TOSTATE
                foreach (Transition<string> t in trans)
                {
                    if (t.Symbol == symbol)
                    {
                        RetrieveEpsilonIncludedState(t.ToState, ndfa, ref newStates);

                        //DEPRECATED, does not work if finalstate is reached through epsilon routes
                        //Check if this state is final, if one of the substates for the new TOSTATE is final, TOSTATE becomes final as a whole.
                        //if (ndfa.FinalStates.Contains(t.ToState))
                        //{
                        //    isFinalState = true;
                        //}
                    }
                }
            }

            //combines substates into one string (TOSTATE)
            foreach (string subState in newStates)
            {
                toState += subState + "_";
                if (ndfa.FinalStates.Contains(subState))
                    isFinalState = true;
            }
            toState = toState.TrimEnd('_');
            return isFinalState;
        }

        private void ConvertState(string currentState, ref Automata<string> dfa, ref Automata<string> ndfa)
        {
            //If this state is already completely processed, return to avoid stackoverflow exception
            if (dfa.GetTransition(currentState).Count == ndfa.Symbols.Count)
                return;

            //split given state for comparison
            string[] states = currentState.Split('_');

            //Loop through all symbols aka all the necessary routes
            foreach (char symbol in ndfa.Symbols)
            {
                //checks if this symbol already has a route in the new DFA
                if (CheckExistingRouteForChar(currentState, symbol, dfa))
                    return;

                int correctAmountOfRoutes = CheckAvailableRoutes(states, symbol, ndfa);

                //the TOSTATE of the to be added implementation
                string toState = "";
                if (correctAmountOfRoutes == 0)
                {
                    dfa.AddTransition(new Transition<string>(currentState, symbol, "F"));
                }
                else
                {
                    bool isFinalState = GenerateToState(ref toState, states, symbol, ndfa);

                    dfa.AddTransition(new Transition<string>(currentState, symbol, toState));

                    //Checks if currentState is should be final aswell (could be done better)
                    if (ndfa.FinalStates.Contains(currentState))
                    {
                        dfa.DefineAsFinalState(currentState);
                    }

                    if (isFinalState)
                        dfa.DefineAsFinalState(toState);

                    //checks if its not a loop to itself
                    if (currentState != toState)
                        ConvertState(toState, ref dfa, ref ndfa);
                }
            }
        }

        public Automata<string> Reverse(Automata<string> automaat)
        {
            Automata<string> reverseAutomaat = new Automata<string>(automaat.Symbols);
            foreach (Transition<string> transition in automaat.Transitions)
            {
                reverseAutomaat.AddTransition(
                    new Transition<string>(transition.ToState, transition.Symbol, transition.FromState));
            }
            reverseAutomaat.StartStates = automaat.FinalStates;
            reverseAutomaat.FinalStates = automaat.StartStates;
            return reverseAutomaat;
        }

        // The minimize method of the DFA
        public Automata<string> OptimizeDfa(Automata<string> dfa)
        {
            Automata<string> one = Reverse(dfa);
            Automata<string> two = Convert(one);
            Automata<string> three = Reverse(two);
            Automata<string> four = Convert(three);

            //return four;
            return Convert(Reverse(Convert(Reverse(four))));
        }
    }
}