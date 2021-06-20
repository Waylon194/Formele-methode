using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;

namespace ProjectFormeleMethodes.Examples
{
    public class DFABuilder
    {
        public static Automata<string> BuildDFASampleOne()
        {
            //Alfabets
            char[] alphabetAB = { 'a', 'b' };
            char[] alphabetXY = { 'x', 'y' };

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

            //Tweede alfabet
            Automata<string> n = new Automata<string>(alphabetAB);

            m.AddTransition(new Transition<string>("q0", 'a', "q2"));
            m.AddTransition(new Transition<string>("q0", 'b', "q3"));

            m.AddTransition(new Transition<string>("q3", 'a', "q2"));
            m.AddTransition(new Transition<string>("q1", 'b', "q2"));

            m.AddTransition(new Transition<string>("q2", 'a', "q4"));
            m.AddTransition(new Transition<string>("q2", 'b', "q3"));

            m.AddTransition(new Transition<string>("q2", 'a', "q1"));
            m.AddTransition(new Transition<string>("q4", 'b', "q2"));

            //Error state, met fuik voor a en b
            m.AddTransition(new Transition<string>("q4", 'a'));
            m.AddTransition(new Transition<string>("q4", 'b'));

            //Start state
            m.DefineAsStartState("q0");

            //Final states
            m.DefineAsFinalState("q4");
            m.DefineAsFinalState("q3");

            return n;

            //Derde alfabet
            Automata<string> o = new Automata<string>(alphabetXY);

            m.AddTransition(new Transition<string>("q0", 'x', "q3"));
            m.AddTransition(new Transition<string>("q0", 'y', "q2"));

            m.AddTransition(new Transition<string>("q2", 'x', "q4"));
            m.AddTransition(new Transition<string>("q4", 'y', "q2"));

            m.AddTransition(new Transition<string>("q2", 'x', "q3"));
            m.AddTransition(new Transition<string>("q4", 'y', "q2"));

            m.AddTransition(new Transition<string>("q4", 'x', "q3"));
            m.AddTransition(new Transition<string>("q1", 'y', "q2"));

            //Error state, met fuik voor x en y
            m.AddTransition(new Transition<string>("q4", 'x'));
            m.AddTransition(new Transition<string>("q4", 'y'));

            //Start state
            m.DefineAsStartState("q0");

            //Final states
            m.DefineAsFinalState("q3");
            m.DefineAsFinalState("q2");

            return o;
        }
    }
}
