using System;
using System.Collections.Generic;
using ProjectFormeleMethodes.NDFA.Transitions;
using System.Linq;
using ProjectFormeleMethodes.ConversionEngines;

namespace ProjectFormeleMethodes.NDFA
{
    public class Automata<T> where T : IComparable
    {
        public ISet<Transition<T>> Transitions { get; }
        public SortedSet<T> States { get; }
        public SortedSet<T> StartStates { get; set; }
        public SortedSet<T> FinalStates { get; set; }
        public SortedSet<T> NormalStates { get; set; }
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
            NormalStates = new SortedSet<T>();
            Symbols = symbols;
        }

        public void AddTransition(Transition<T> t)
        {
            Transitions.Add(t);
            States.Add(t.FromState);
            States.Add(t.ToState);
            NormalStates.Add(t.FromState);
            NormalStates.Add(t.ToState);
        }

        public SortedSet<T> GetNormalStates()
        {
            foreach (var endState in FinalStates)
            {
                if (NormalStates.Contains(endState))
                {
                    NormalStates.Remove(endState);
                }
            }
            foreach (var startState in StartStates)
            {
                if (NormalStates.Contains(startState))
                {
                    NormalStates.Remove(startState);
                }
            }
            return this.NormalStates;
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
            bool isDfa = !(Transitions.Where(e => e.Symbol.Equals('ɛ')).ToList().Count > 0);
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

        public Automata<string> MinimizeDFAReversedAlgorithm(Automata<string> dfa)
        {
            NDFAToDFAEngine engine = new NDFAToDFAEngine();

            Automata<string> one = CreateReversedVariant(dfa);
            Automata<string> two = engine.Convert(one);
            Automata<string> three = CreateReversedVariant(two);
            Automata<string> four = engine.Convert(three);

            //return reversed;
            return engine.Convert(
                CreateReversedVariant(
                    engine.Convert(
                        CreateReversedVariant(four))));
        }

        // Flips the whole NDFA/DFA to create the opposite, of NOT version
        public static Automata<string> CreateReversedVariant(Automata<string> automataToFlip)
        {
            Automata<string> reverseAutomata = new Automata<string>(automataToFlip.Symbols);
            foreach (Transition<string> transition in automataToFlip.Transitions)
            {
                reverseAutomata.AddTransition(new Transition<string>(transition.ToState, transition.Symbol, transition.FromState));
            }
            reverseAutomata.StartStates = automataToFlip.FinalStates;
            reverseAutomata.FinalStates = automataToFlip.StartStates;
            return reverseAutomata;
        }
    }
}