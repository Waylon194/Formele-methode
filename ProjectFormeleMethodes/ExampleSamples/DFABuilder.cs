using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;

namespace ProjectFormeleMethodes.Examples
{
    public class DFABuilder
    {
        //Alfabets
        static char[] alphabetAB = { 'a', 'b' };
        static char[] alphabetXY = { 'x', 'y' };

        public static Automata<string> BuildDFASampleOne()
        {
            Automata<string> m = new Automata<string>(alphabetAB);

            m.AddTransition(new Transition<string>("q0", 'a', "q1"));
            m.AddTransition(new Transition<string>("q0", 'b', "q4"));

            m.AddTransition(new Transition<string>("q1", 'a', "q4"));
            m.AddTransition(new Transition<string>("q1", 'b', "q2"));

            m.AddTransition(new Transition<string>("q2", 'a', "q3"));
            m.AddTransition(new Transition<string>("q2", 'b', "q4"));

            m.AddTransition(new Transition<string>("q3", 'a', "q1"));
            m.AddTransition(new Transition<string>("q3", 'b', "q2"));

            //Error state, met fuik voor a en b
            m.AddTransition(new Transition<string>("q4", 'a'));
            m.AddTransition(new Transition<string>("q4", 'b'));

            //Start state
            m.DefineAsStartState("q0");

            //Final states
            m.DefineAsFinalState("q2");
            m.DefineAsFinalState("q3");

            return m;
        }

        public static Automata<string> BuildDFASampleTwo()
        {
            //Tweede alfabet
            Automata<string> n = new Automata<string>(alphabetAB);

            n.AddTransition(new Transition<string>("q0", 'a', "q2"));
            n.AddTransition(new Transition<string>("q0", 'b', "q3"));

            n.AddTransition(new Transition<string>("q3", 'a', "q2"));
            n.AddTransition(new Transition<string>("q1", 'b', "q2"));

            n.AddTransition(new Transition<string>("q2", 'a', "q4"));
            n.AddTransition(new Transition<string>("q2", 'b', "q3"));

            n.AddTransition(new Transition<string>("q2", 'a', "q1"));
            n.AddTransition(new Transition<string>("q4", 'b', "q2"));

            //Error state, met fuik voor a en b
            n.AddTransition(new Transition<string>("q4", 'a'));
            n.AddTransition(new Transition<string>("q4", 'b'));

            //Start state
            n.DefineAsStartState("q0");

            //Final states
            n.DefineAsFinalState("q4");
            n.DefineAsFinalState("q3");

            return n;
        }

        public static Automata<string> BuildDFASampleThree()
        {
            //Derde alfabet
            Automata<string> o = new Automata<string>(alphabetXY);

            o.AddTransition(new Transition<string>("q0", 'x', "q3"));
            o.AddTransition(new Transition<string>("q0", 'y', "q2"));

            o.AddTransition(new Transition<string>("q2", 'x', "q4"));
            o.AddTransition(new Transition<string>("q4", 'y', "q2"));

            o.AddTransition(new Transition<string>("q2", 'x', "q3"));
            o.AddTransition(new Transition<string>("q4", 'y', "q2"));

            o.AddTransition(new Transition<string>("q4", 'x', "q3"));
            o.AddTransition(new Transition<string>("q1", 'y', "q2"));

            //Error state, met fuik voor x en y
            o.AddTransition(new Transition<string>("q4", 'x'));
            o.AddTransition(new Transition<string>("q4", 'y'));

            //Start state
            o.DefineAsStartState("q0");

            //Final states
            o.DefineAsFinalState("q3");
            o.DefineAsFinalState("q2");

            return o;
        }
    }
}