﻿using System;
using System.Collections.Generic;
using ProjectFormeleMethodes.NDFA.Transitions;
using System.Linq;

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

        private static void CreateBeginsWithAutomataByReference(DfaGenerateValue param, ref Automata<string> dfa)
        {
            char[] chars = param.Parameter.ToCharArray();
            int stateCounter = 0;
            dfa.DefineAsStartState(stateCounter.ToString());
            foreach (char c in chars)
            {
                dfa.AddTransition(new Transition<string>(stateCounter.ToString(), c,
                    (stateCounter + 1).ToString()));

                stateCounter = dfa.States.Count - 1;
            }

            dfa.DefineAsFinalState(stateCounter.ToString());
            foreach (char c in dfa.Symbols)
            {
                dfa.AddTransition(new Transition<string>(stateCounter.ToString(), c, stateCounter.ToString()));
            }

            //Hardcopy states
            SortedSet<string> ogStates = new SortedSet<string>(dfa.States);

            foreach (string state in ogStates)
            {
                List<Transition<string>> trans = dfa.GetTransition(state);
                SortedSet<char> routesPresent = new SortedSet<char>();
                foreach (Transition<string> t in trans)
                {
                    routesPresent.Add(t.Symbol);
                }

                foreach (char letter in dfa.Symbols)
                {
                    if (!routesPresent.Contains(letter))
                    {
                        dfa.AddTransition(new Transition<string>(state, letter, "F"));
                    }
                }
            }

            if (dfa.States.Contains("F"))
            {
                foreach (char letter in dfa.Symbols)
                {
                    dfa.AddTransition(new Transition<string>("F", letter, "F"));
                }
            }
        }

        private static void CreateContainsAutomataByReference(DfaGenerateValue param, ref Automata<string> dfa)
        {
            char[] chars = param.Parameter.ToCharArray();
            int stateCounter = 0;
            dfa.DefineAsStartState(stateCounter.ToString());
            foreach (char c in chars)
            {
                dfa.AddTransition(new Transition<string>(stateCounter.ToString(), c,
                    (stateCounter + 1).ToString()));
                stateCounter = dfa.States.Count - 1;
            }

            dfa.DefineAsFinalState(stateCounter.ToString());

            //Hardcopy states
            List<string> ogStates = new List<string>(dfa.States);

            foreach (var state in ogStates)
            {
                List<Transition<string>> trans = dfa.GetTransition(state);
                SortedSet<char> routesPresent = new SortedSet<char>();
                foreach (Transition<string> t in trans)
                {
                    routesPresent.Add(t.Symbol);
                }

                foreach (char letter in dfa.Symbols)
                {
                    if (!routesPresent.Contains(letter) && !dfa.FinalStates.Contains(state))
                    {
                        int stateToReturnTo = BackTrackForWorkingRoute(chars, letter);
                        dfa.AddTransition(new Transition<string>(state, letter, ogStates[stateToReturnTo]));
                    }
                }
            }

            foreach (char c in dfa.Symbols)
            {
                foreach (string finalstate in dfa.FinalStates)
                    dfa.AddTransition(new Transition<string>(stateCounter.ToString(), c, stateCounter.ToString()));
            }
        }

        private static void CreateEndsWithAutomataByReference(DfaGenerateValue param, ref Automata<string> dfa)
        {
            char[] chars = param.Parameter.ToCharArray();
            int stateCounter = 0;
            dfa.DefineAsStartState(stateCounter.ToString());
            foreach (char c in chars)
            {
                dfa.AddTransition(new Transition<string>(stateCounter.ToString(), c,
                    (stateCounter + 1).ToString()));

                stateCounter = dfa.States.Count - 1;
            }

            dfa.DefineAsFinalState(stateCounter.ToString());
            //Hardcopy states
            List<string> ogStates = new List<string>(dfa.States);

            foreach (var state in ogStates)
            {
                List<Transition<string>> transition = dfa.GetTransition(state);
                SortedSet<char> routesPresent = new SortedSet<char>();
                foreach (Transition<string> t in transition)
                {
                    routesPresent.Add(t.Symbol);
                }

                foreach (char letter in dfa.Symbols)
                {
                    if (!routesPresent.Contains(letter))
                    {
                        int stateToReturnTo = BackTrackForWorkingRoute(chars, letter);
                        dfa.AddTransition(new Transition<string>(state, letter, ogStates[stateToReturnTo]));
                    }
                }
            }
        }

        private static int BackTrackForWorkingRoute(char[] route, char toUse)
        {
            string completeRoute = new string(route);

            for (int i = route.Length - 1; i >= 0; i--)
            {
                string tobeAdded = "";
                for (int j = i; j < route.Length; j++)
                {
                    tobeAdded += route[j];
                }

                string routeTocheck = (toUse.ToString() + tobeAdded);
                if (routeTocheck.Contains(completeRoute))
                {
                    return i;
                }
            }
            return -1;
        }

        // Flips the whole NDFA/DFA to create the opposite, of NOT version
        public static Automata<string> CreateNotVariant(Automata<string> automataToFlip)
        {
            Automata<string> flippedAutomata = new Automata<string>(automataToFlip.Symbols);

            // create a copy of the data
            flippedAutomata.StartStates = automataToFlip.StartStates;
            flippedAutomata.Transitions.UnionWith(automataToFlip.Transitions); 

            foreach (string state in flippedAutomata.States)
            {
                if (!automataToFlip.FinalStates.Contains(state))
                {
                    flippedAutomata.DefineAsFinalState(state);
                }
            }
            return flippedAutomata;
        }

        public enum GeneratorType
        {
            BeginsWith,
            Contains,
            EndsWith
        }

        public struct DfaGenerateValue
        {
            public bool IsNot;
            public string Parameter;
            public GeneratorType Type;
        }
    }
}