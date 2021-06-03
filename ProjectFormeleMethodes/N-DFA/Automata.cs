using System;
using System.Collections.Generic;
using ProjectFormeleMethodes.NDFA.Transitions;
using System.Linq;

/// <summary>
/// The class Automata represents both DFA and NDFA: some NDFA's are also DFA
/// Using the method isDFA we can check this
/// 
/// We use '$' to denote the empty symbol epsilon
/// 
/// @author Paul de Mast
/// @version 1.0

// Updates* With edits from Waylon Lodder & Vincent de Rooij to fix formatting for Java to C#

/// </summary>

namespace ProjectFormeleMethodes.NDFA
{
    public class Automata<T> where T : IComparable
    {
        public ISet<Transition<T>> Transitions { get; }
        public SortedSet<T> States { get; }
        public SortedSet<T> StartStates { get; set; }
        public SortedSet<T> FinalStates { get; set; }
        public SortedSet<char> Symbols { get; set; }

        public Automata() : this(new SortedSet<char>())
        {
        }

        public Automata(char[] s) : this(new SortedSet<char>(s))
        {
        }

        public Automata(SortedSet<char> symbols)
        {
            Transitions = new SortedSet<Transition<T>>();
            States = new SortedSet<T>();
            StartStates = new SortedSet<T>();
            FinalStates = new SortedSet<T>();
            Symbols = symbols;
        }

        public void AddTransition(Transition<T> t)
        {
            Transitions.Add(t);
            States.Add(t.FromState);
            States.Add(t.ToState);
        }

        public void DefineAsStartState(T t)
        {
            if (!States.Contains(t))
            {
                States.Add(t);
            }
            StartStates.Add(t);
        }

        public void DefineAsFinalState(T t)
        {
            if (!States.Contains(t))
            {
                States.Add(t);
            }
            FinalStates.Add(t);
        }

        public List<Transition<T>> GetToStates(T state, char symbol)
        {
            return Transitions.Where(e => e.Symbol == symbol).Where(e => e.FromState.Equals(state)).ToList();
        }

        public bool IsDfa()
        {
            bool isDfa = !(Transitions.Where(e => e.Symbol.Equals('$')).ToList().Count > 0);
            foreach (T state in States)
            {
                foreach (char symbol in Symbols)
                {
                    isDfa = isDfa && GetToStates(state, symbol).Count <= 1;
                }
            }
            return isDfa;
        }

        public List<Transition<T>> GetTransition(T state)
        {
            List<Transition<T>> transitions = Transitions.Where(e => e.FromState.Equals(state)).ToList();
            List<T> epsilonStates = transitions.Where(e => e.Symbol == 'ɛ').Select(e => e.ToState).ToList();
            foreach (T epsilonState in epsilonStates)
            {
                transitions.AddRange(GetTransition(epsilonState));
            }
            return transitions;
        }
    }
}