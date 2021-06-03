using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;
using ProjectFormeleMethodes.RegExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines
{
    public class ThompsonConstructor
    {
        public static Automata<string> ConvertRegExp(RegExp regExp)
        {
            Automata<string> automaat = new Automata<string>();

            automaat.DefineAsStartState("0");
            automaat.DefineAsFinalState("1");
            int stateCounter = 2;

            Convert(regExp, ref automaat, ref stateCounter, 0, 1);
            automaat.Symbols = new SortedSet<char>(automaat.Transitions.Distinct().Select(e => e.Symbol).ToList());
            //Epsilon should not be in alphabet
            automaat.Symbols.Remove('ɛ');
            return automaat;
        }

        public static void Convert(RegExp regExp, ref Automata<string> automaat, ref int stateCounter, int leftState, int rightState)
        {
            switch (regExp.OperatorType)
            {
                case RegExpOperatorTypes.PLUS:
                    Plus(regExp, ref automaat, ref stateCounter, leftState, rightState);
                    break;
                case RegExpOperatorTypes.STAR:
                    Star(regExp, ref automaat, ref stateCounter, leftState, rightState);
                    break;
                case RegExpOperatorTypes.OR:
                    Or(regExp, ref automaat, ref stateCounter, leftState, rightState);
                    break;
                case RegExpOperatorTypes.DOT:
                    Dot(regExp, ref automaat, ref stateCounter, leftState, rightState);
                    break;
                case RegExpOperatorTypes.ONCE:
                    One(regExp, ref automaat, ref stateCounter, leftState, rightState);
                    break;
            }
        }

        public static void Plus(RegExp regExp, ref Automata<string> automaat, ref int stateCounter, int leftState, int rightState)
        {
            int stateTwo = stateCounter;
            int stateThree = stateCounter + 1;
            stateCounter = stateCounter + 2;
            automaat.AddTransition(new Transition<string>(leftState.ToString(), 'ɛ', stateTwo.ToString()));
            automaat.AddTransition(new Transition<string>(stateThree.ToString(), 'ɛ', stateTwo.ToString()));
            automaat.AddTransition(new Transition<string>(stateThree.ToString(), 'ɛ', rightState.ToString()));
            Convert(regExp.Left, ref automaat, ref stateCounter, stateTwo, stateThree);
        }

        public static void Star(RegExp regExp, ref Automata<string> automaat, ref int stateCounter, int leftState,
            int rightState)
        {
            int stateTwo = stateCounter;
            int stateThree = stateCounter + 1;
            stateCounter = stateCounter + 2;
            automaat.AddTransition(new Transition<string>(leftState.ToString(), 'ɛ', stateTwo.ToString()));
            automaat.AddTransition(new Transition<string>(stateThree.ToString(), 'ɛ', stateTwo.ToString()));
            automaat.AddTransition(new Transition<string>(stateThree.ToString(), 'ɛ', rightState.ToString()));
            automaat.AddTransition(new Transition<string>(leftState.ToString(), 'ɛ', rightState.ToString()));
            Convert(regExp.Left, ref automaat, ref stateCounter, stateTwo, stateThree);
        }

        public static void Or(RegExp regExp, ref Automata<string> automaat, ref int stateCounter, int leftState, int rightState)
        {
            int state2 = stateCounter;
            int state3 = stateCounter + 1;
            int state4 = stateCounter + 2;
            int state5 = stateCounter + 3;
            stateCounter = stateCounter + 4;
            automaat.AddTransition(new Transition<string>(leftState.ToString(), 'ɛ', state2.ToString()));
            automaat.AddTransition(new Transition<string>(leftState.ToString(), 'ɛ', state4.ToString()));
            automaat.AddTransition(new Transition<string>(state3.ToString(), 'ɛ', rightState.ToString()));
            automaat.AddTransition(new Transition<string>(state5.ToString(), 'ɛ', rightState.ToString()));
            Convert(regExp.Left, ref automaat, ref stateCounter, state2, state3);
            Convert(regExp.Right, ref automaat, ref stateCounter, state4, state5);
        }

        public static void Dot(RegExp regExp, ref Automata<string> automaat, ref int stateCounter, int leftState, int rightState)
        {
            int midState = stateCounter;
            stateCounter++;
            Convert(regExp.Left, ref automaat, ref stateCounter, leftState, midState);
            Convert(regExp.Right, ref automaat, ref stateCounter, midState, rightState);
        }

        public static void One(RegExp regExp, ref Automata<string> automaat, ref int stateCounter, int leftState, int rightState)
        {
            char[] characters = regExp.Terminals.ToCharArray();
            if (characters.Length == 1)
            {
                automaat.AddTransition(
                    new Transition<string>(leftState.ToString(), characters[0], rightState.ToString()));
            }
            else
            {
                automaat.AddTransition(
                    new Transition<string>(leftState.ToString(), characters[0], stateCounter.ToString()));
                int i = 1;
                while (i < characters.Length - 1)
                {
                    automaat.AddTransition(new Transition<string>(stateCounter.ToString(), characters[i],
                        (stateCounter + 1).ToString()));
                    stateCounter++;
                    i++;
                }
                automaat.AddTransition(
                    new Transition<string>(stateCounter.ToString(), characters[i], rightState.ToString()));
                stateCounter++;
            }
        }
    }
}
