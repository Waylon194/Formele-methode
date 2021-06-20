using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;

namespace ProjectFormeleMethodes.Examples
{
    public class NDFABuilder
    {
        public static Automata<string> BuildNDFASampleOne()
        {
            char[] alphabet = { 'a', 'b' };
            Automata<string> m = new Automata<string>(alphabet);

            m.AddTransition(new Transition<string>("A", 'a', "C"));
            m.AddTransition(new Transition<string>("A", 'b', "B"));
            m.AddTransition(new Transition<string>("A", 'b', "C"));

            m.AddTransition(new Transition<string>("B", 'b', "C"));
            m.AddTransition(new Transition<string>("B", "C"));

            m.AddTransition(new Transition<string>("C", 'a', "D"));
            m.AddTransition(new Transition<string>("C", 'a', "E"));
            m.AddTransition(new Transition<string>("C", 'b', "D"));

            m.AddTransition(new Transition<string>("D", 'a', "B"));
            m.AddTransition(new Transition<string>("D", 'a', "C"));

            m.AddTransition(new Transition<string>("E", 'a'));
            m.AddTransition(new Transition<string>("E", "D"));

            // only on start state in a dfa:
            m.DefineAsStartState("A");

            // two final states:
            m.DefineAsFinalState("C");
            m.DefineAsFinalState("E");

            return m;
        }

        public static Automata<string> BuildNDFASampleTwo()
        {
            char[] alphabet = { 'b', 'd' };
            Automata<string> m = new Automata<string>(alphabet);

            m.AddTransition(new Transition<string>("A", 'd', "C"));
            m.AddTransition(new Transition<string>("A", 'b', "B"));
            m.AddTransition(new Transition<string>("A", 'b', "C"));

            m.AddTransition(new Transition<string>("B", 'b', "C"));
            m.AddTransition(new Transition<string>("B", 'd', "D"));
            m.AddTransition(new Transition<string>("B", "C"));

            m.AddTransition(new Transition<string>("C", 'd', "D"));
            m.AddTransition(new Transition<string>("C", 'd', "E"));
            m.AddTransition(new Transition<string>("C", 'b', "D"));

            m.AddTransition(new Transition<string>("D", 'b', "B"));
            m.AddTransition(new Transition<string>("D", 'd', "C"));

            m.AddTransition(new Transition<string>("E", 'b'));
            m.AddTransition(new Transition<string>("E", "D"));

            // only on start state in a dfa:
            m.DefineAsStartState("A");

            // two final states:
            m.DefineAsFinalState("C");
            m.DefineAsFinalState("E");

            return m;
        }

        public static Automata<string> BuildNDFASampleThree()
        {
            char[] alphabet = { 'a', 'b', 'c'};
            Automata<string> m = new Automata<string>(alphabet);

            m.AddTransition(new Transition<string>("A", 'a', "C"));
            m.AddTransition(new Transition<string>("A", 'b', "B"));
            m.AddTransition(new Transition<string>("A", 'c', "C"));

            m.AddTransition(new Transition<string>("B", 'b', "C"));
            m.AddTransition(new Transition<string>("B", "C"));

            m.AddTransition(new Transition<string>("C", 'c', "D"));
            m.AddTransition(new Transition<string>("C", 'c', "E"));
            m.AddTransition(new Transition<string>("C", 'b', "E"));
            m.AddTransition(new Transition<string>("C", 'a', "D"));
            m.AddTransition(new Transition<string>("C", 'c', "A"));

            m.AddTransition(new Transition<string>("D", 'a', "B"));
            m.AddTransition(new Transition<string>("D", 'a', "C"));

            m.AddTransition(new Transition<string>("E", 'a'));
            m.AddTransition(new Transition<string>("E", "D"));

            // only on start state in a dfa:
            m.DefineAsStartState("A");

            // two final states:
            m.DefineAsFinalState("C");
            m.DefineAsFinalState("E");

            return m;
        }

        public static Automata<string> BuildNDFASampleFour()
        {
            Automata<string> ndfa = new Automata<string>();
            ndfa.Symbols.Add('a');
            ndfa.Symbols.Add('b');

            ndfa.DefineAsStartState("q0");
            ndfa.DefineAsFinalState("qF");

            // transitions of first state
            ndfa.AddTransition(new Transition<string>("q0", 'a', "q1"));
            ndfa.AddTransition(new Transition<string>("q0", 'a', "q2"));
            ndfa.AddTransition(new Transition<string>("q0", 'b', "q3"));

            // transitions of second state
            ndfa.AddTransition(new Transition<string>("q1", 'a', "q2"));
            ndfa.AddTransition(new Transition<string>("q1", 'b', "q0"));
            ndfa.AddTransition(new Transition<string>("q1", 'ɛ', "q2"));

            // transitions of third state
            ndfa.AddTransition(new Transition<string>("q2", 'a', "q2"));
            ndfa.AddTransition(new Transition<string>("q2", 'ɛ', "q3"));
            ndfa.AddTransition(new Transition<string>("q2", 'a', "qF"));

            // transitions of fourth state
            ndfa.AddTransition(new Transition<string>("q3", 'a', "qF"));

            // transitions of final state
            ndfa.AddTransition(new Transition<string>("qF", 'a', "q3"));

            return ndfa;
        }
    }
}
