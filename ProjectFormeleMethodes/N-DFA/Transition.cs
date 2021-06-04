using System;

/// <summary>
/// The class Automata represents both DFA and NDFA: some NDFA's are also DFA
/// Using the method isDFA we can check this
/// 
/// We use '$' to denote the empty symbol epsilon
/// 
/// @author Paul de Mast, 
/// @version 1.0

// Updates* With edits from Waylon Lodder & Vincent de Rooij to fix formatting for Java to C#

/// </summary>s

namespace ProjectFormeleMethodes.NDFA.Transitions
{
    public class Transition<T> : IComparable<Transition<T>> where T : IComparable
    {
        public const char EPSILON = 'ɛ';
        private T fromState;
        private char symbol;
        private T toState;

        public T FromState
        {
            get
            {
                return fromState;
            }
        }

        public T ToState
        {
            get
            {
                return toState;
            }
        }

        public char Symbol
        {
            get
            {
                return symbol;
            }
        }

        // this constructor can be used to define loops:
        public Transition(T fromOrTo, char symbol) : this(fromOrTo, symbol, fromOrTo)
        {

        }

        public Transition(T from, T to) : this(from, EPSILON, to)
        {

        }

        public Transition(T from, char symbol, T to)
        {
            this.fromState = from;
            this.symbol = symbol;
            this.toState = to;
        }

        // overriding equals
        public override bool Equals(object other)
        {
            // define the object type to type of transition, with type T comparable
            Transition<T> transition = other as Transition<T>;

            // if the transition is not null, check if states and symbols overlap
            if (transition != null)
            {
                return this.fromState.Equals(transition.FromState) && this.toState.Equals(transition.ToState) && this.symbol == (transition.Symbol);
            }
            return false;
        }

        public int CompareTo(Transition<T> t2)
        {
            // compares the individual items between each other
            int fromCmp = this.fromState.CompareTo(t2.fromState);
            int symbolCmp = this.symbol.CompareTo(t2.symbol);
            int toCmp = this.toState.CompareTo(t2.toState);

            // if from compare != 0, return value of "fromCmp", else check the symbolCmp 
            return (fromCmp != 0 ? fromCmp : (symbolCmp != 0 ? symbolCmp : toCmp));
        }

        public override string ToString()
        {
            return this.FromState + "--[ " + this.Symbol + " ]-->" + this.ToState;
        }
    }
}