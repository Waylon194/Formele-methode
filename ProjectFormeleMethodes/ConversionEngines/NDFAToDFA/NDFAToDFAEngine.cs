using ProjectFormeleMethodes.ConversionEngines.NDFAToDFA.models;
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

        public NDFAToDFAEngine()
        {

        }

        public Automata<string> Convert(Automata<string> ndfaToConvert)
        {
            // create our automata object (DFA)
            Automata<string> automata = new Automata<string>(ndfaToConvert.Symbols);

            // SortedSet of the reachable states 
            SortedSet<Transition<string>> reachableStates = new SortedSet<Transition<string>>();

            // begin search for Epsilon closures
            foreach (var state in ndfaToConvert.States)
            {                
                determineEpsilonClosures(state, ndfaToConvert, ref reachableStates);
            }
            return automata;
        }

        private void generateHelperTable() 
        {
            
        }

        private void checkForEpsilon(Transition<string> transition)
        {
            if (transition.Symbol == this.EPSILON)
            {
                //checkForEpsilon();
            }
            else
            {
                // a symbol other than EPSILON occured
                //checkForEpsilon();
            }
        }

        private void determineEpsilonClosures(string state, Automata<string> auto, ref SortedSet<Transition<string>> validStates)
        {
            List<Transition<string>> transitions = auto.GetTransition(state).Where(item => item.FromState.Equals(state)).ToList();

            if (transitions.Count() == 1)
            {
                var t = transitions.First(); // gets the only element of the transitions
                    // if the symbol doesn't equal epsilon, start transition search, because appropriate symbol was found
                if ( !(t.Symbol.Equals(EPSILON)) )
                {
                    Console.WriteLine("Called!");
                }
                determineEpsilonClosures(t.ToState, auto, ref validStates);
            }
            else
            {
                while (transitions.Count() == 0)
                {
                    // loop through all the possible options
                }
            }
        }
    }
}
