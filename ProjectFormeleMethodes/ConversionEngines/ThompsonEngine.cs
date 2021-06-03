using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;
using ProjectFormeleMethodes.RegExpressions;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 
///  Build with use of https://github.com/MaurodeLyon/Formele-methoden/blob/master/Formele%20methoden/ThompsonConstruction.cs 
/// 
///  The Rules of the Thompson Construction
///  
///  *Rule 1: The translation of a single Terminal Symbol (e.g. "a", "b", "ɛ", etc..)
///  - The Rule states to place a new blackbox, which creates two new states (a start and end state) then place the correct Terminal in between these two states
///
///  *Rule 2: The translation of an empty Terminal Symbol ( '' ) or Empty-operator
///  - The Rule states to do the exact same procedure with Rule 1, except instead of a empty Terminal place an Epsilon "ɛ" in between these new states.
///
///  *Rule 3: The translation of a concat operator ( '.' ) or DOT-operator
///  - The Rule states to place give every Terminal its own blackbox and thus a total of 4 new states, with an "ɛ" Terminal as a link between the two. 
///  
///  *Rule 4: The translation of a choice operator ( '|' ) or OR-operator
///  - The Rule states to first create two new states. Afterwards create two blackboxes (with each its own two new states) 
///     and link them parallel of each other. These states are linked to the first two new states at the beginning and end via a "ɛ" Terminal 
/// 
///  *Rule 5: The translation of a plus operator ( '+' ) or PLUS-operator
///  - The Rule states to first create two new states. Afterwards create a new blackbox, which has an "ɛ" Terminal going back to the first state.
///     Finally make sure to link the states with an "ɛ" Terminal or Epsilon.
///     
///  *Rule 6: The translation of a star operator ( '*' ) or STAR-operator
///  - The Rule states to the exact same thing as done with the PLUS-operator (Rule 5), but add an additional transition which goes from the start state to the end state. 
/// 
///  *** To ensure all pieces are touched this thompson constructor works for out to in. 
///         So when dealing with (a|ab)* the star gets done first. -> (a|ab)* <-, then  a ->|<- ab, and so on..  
/// 
/// </summary>

namespace ProjectFormeleMethodes.ConversionEngines
{
    /// <summary>
    /// Class responsible for converting a Regular Expression to a NDFA format
    /// </summary>
    public class ThompsonEngine
    {
        private const string STATE_DEFINER_SYMBOL = "q";

        public ThompsonEngine()
        {
            // default Constructor to acces methods
        }

        // using the keyword ref to make use of call by reference options in C#
        public Automata<string> ConvertToDFA(RegExp regularExpression)
        {
            Automata<string> ndfaModel = new Automata<string>();

            // define start and end states as a start of the thompson construction, with start state "S" and end state "F"
            // here we create our first blackbox "around" our regular expression

            // define start and stop states
            ndfaModel.DefineAsStartState(STATE_DEFINER_SYMBOL + "S"); // leftState
            ndfaModel.DefineAsFinalState(STATE_DEFINER_SYMBOL + "F"); // rightState
            int currentStateCounter = 1; // let the states begin counting from 1. 

            // we convert the regular expression step by step to a NDFA, make ndfaModel and stateCounter referable, to make it possible to use a call-by-reference design.
            convert(regularExpression, ref ndfaModel, ref currentStateCounter, STATE_DEFINER_SYMBOL + "S", STATE_DEFINER_SYMBOL + "F");

            // add the symbols of the regular expression to the NDFA
            ndfaModel.Symbols = new SortedSet<char>(ndfaModel.Transitions.Distinct().Select(e => e.Symbol).ToList());
            return ndfaModel;
        }

        // Method to handle all forms of conversion
        private void convert(RegExp expression, ref Automata<string> ndfa, ref int currentStateCounter, string leftState, string rightState)
        {
            switch (expression.OperatorType)
            {
                case RegExpOperatorTypes.PLUS:
                    plusConversion(expression, ref ndfa, ref currentStateCounter, leftState, rightState); // convert plus-operator to NDFA
                    break;

                case RegExpOperatorTypes.STAR:
                    starConversion(expression, ref ndfa, ref currentStateCounter, leftState, rightState); // convert star-operator to NDFA
                    break;

                case RegExpOperatorTypes.OR:
                    orConversion(expression, ref ndfa, ref currentStateCounter, leftState, rightState); // convert or-operator to NDFA
                    break;

                case RegExpOperatorTypes.DOT:
                    dotConversion(expression, ref ndfa, ref currentStateCounter, leftState, rightState); // convert dot-operator to NDFA
                    break;

                case RegExpOperatorTypes.ONCE:
                    onceConversion(expression, ref ndfa, ref currentStateCounter, leftState, rightState); // convert once-operator to NDFA
                    break;

                default:
                    Console.WriteLine("Unsupported Operation!");
                    break;
            }
        } 

        private void plusConversion(RegExp expression, ref Automata<string> ndfa, ref int currentStateCounter, string stateA, string stateB)
        {
            // "create" the new states
            string stateC = STATE_DEFINER_SYMBOL + currentStateCounter;
            string stateD = STATE_DEFINER_SYMBOL + (currentStateCounter + 1);
            currentStateCounter += 2; // up the state counter by 2 times to keep the keep track of the new states

            // add the new states to the automata / NDFA
            ndfa.AddTransition(new Transition<string>(stateA, 'ɛ', stateC)); // bind the incoming start state to the new state
            ndfa.AddTransition(new Transition<string>(stateD, 'ɛ', stateC)); // bind the second new state to the first new state 
            ndfa.AddTransition(new Transition<string>(stateD, 'ɛ', stateB)); // bind the second new state to the last incoming state 

            // set the new "A" and "B" states and call the convert method again to start another conversion
            string newStateA = stateC; // the new start state 
            string newStateB = stateD; // the new end state 

            // call the convert method
            convert(expression.Left, ref ndfa, ref currentStateCounter, newStateA, newStateB);
        }

        private void starConversion(RegExp expression, ref Automata<string> ndfa, ref int currentStateCounter, string stateA, string stateB)
        {
            // "create" the new states
            string stateC = STATE_DEFINER_SYMBOL + currentStateCounter;
            string stateD = STATE_DEFINER_SYMBOL + (currentStateCounter + 1);
            currentStateCounter += 2; // up the state counter by 2 times to keep the keep track of the new states

            // add the new states to the automata / NDFA
            ndfa.AddTransition(new Transition<string>(stateA, 'ɛ', stateB)); // bind both incoming states together
            ndfa.AddTransition(new Transition<string>(stateA, 'ɛ', stateC)); // bind the incoming start state to the new state
            ndfa.AddTransition(new Transition<string>(stateD, 'ɛ', stateC)); // bind the second new state to the first new state 
            ndfa.AddTransition(new Transition<string>(stateD, 'ɛ', stateB)); // bind the second new state to the last incoming state 

            // set the new "A" and "B" states and call the convert method again to start another conversion
            string newStateA = stateC; // the new start state 
            string newStateB = stateD; // the new end state 

            // call the convert method
            convert(expression.Left, ref ndfa, ref currentStateCounter, newStateA, newStateB);
        }

        private void orConversion(RegExp expression, ref Automata<string> ndfa, ref int currentStateCounter, string stateA, string stateB)
        {
            // "create" the new states
            string stateC = STATE_DEFINER_SYMBOL + currentStateCounter;
            string stateD = STATE_DEFINER_SYMBOL + (currentStateCounter + 1);
            string stateE = STATE_DEFINER_SYMBOL + (currentStateCounter + 2);
            string stateF = STATE_DEFINER_SYMBOL + (currentStateCounter + 3);
            currentStateCounter += 4; // up the state counter by 2 times to keep the keep track of the new states

            // add the new states to the automata / NDFA
            ndfa.AddTransition(new Transition<string>(stateA, 'ɛ', stateC)); // bind the incoming start state to the new state
            ndfa.AddTransition(new Transition<string>(stateA, 'ɛ', stateE)); // bind the incoming start state to another new state
            ndfa.AddTransition(new Transition<string>(stateD, 'ɛ', stateB)); // bind the new state to the last incoming state 
            ndfa.AddTransition(new Transition<string>(stateF, 'ɛ', stateB)); // bind another new state to the last incoming state 

            // set the new "A" and "B" states, by assigning the states C and D, and call the convert method again to start another conversion
            string newStateA = stateC; // the new start state 
            string newStateB = stateD; // the new end state 

            // call the convert method
            convert(expression.Left, ref ndfa, ref currentStateCounter, newStateA, newStateB);

            // Now to the same but for the other blackbox (StateE and StateF)
            // set the new "A" and "B" states and call the convert method again to start another conversion
            newStateA = stateE; // the new start state 
            newStateB = stateF; // the new end state 

            // call the convert method
            convert(expression.Right, ref ndfa, ref currentStateCounter, newStateA, newStateB);
        }

        private void dotConversion(RegExp expression, ref Automata<string> ndfa, ref int currentStateCounter, string stateA, string stateB)
        {
            // "create" the new states
            string stateC = STATE_DEFINER_SYMBOL + currentStateCounter;
            string stateD = STATE_DEFINER_SYMBOL + (currentStateCounter + 1);
            currentStateCounter += 2; // up the state counter by 2 times to keep the keep track of the new states

            // add the new states to the automata / NDFA
            ndfa.AddTransition(new Transition<string>(stateC, 'ɛ', stateD)); // link both the blackboxes together

            // set the new "A" and "B" states, by assigning the states C and D, and call the convert method again to start another conversion
            string newStateB = stateC; // the new end state 

            // call the convert method
            convert(expression.Left, ref ndfa, ref currentStateCounter, stateA, newStateB);

            // Now to the same but for the other blackbox (StateE and StateF)
            // set the new "A" and "B" states and call the convert method again to start another conversion
            string newStateA = stateD; // the new start state 

            // call the convert method
            convert(expression.Right, ref ndfa, ref currentStateCounter, newStateA, stateB);
        }

        private void onceConversion(RegExp expression, ref Automata<string> ndfa, ref int currentStateCounter, string stateA, string stateB)
        {
            char[] terminals = expression.Terminals.ToCharArray();
            if (terminals.Length == 1) // if the Terminals only contain one Terminal e.g. "a", or "b" , etc...
            {
                // add it to the transitions
                ndfa.AddTransition(new Transition<string>(stateA, terminals[0], stateB));
            }
            else if(terminals.Length == 0) // if a terminal contains an epsilon symbol add it to the transitions
            {
                ndfa.AddTransition(new Transition<string>(stateA, 'ɛ'));
            }
            else
            {
                // assign the temporary state via the currentStateCounter
                string temporaryStateB = STATE_DEFINER_SYMBOL + currentStateCounter;
                ndfa.AddTransition(new Transition<string>(stateA.ToString(), terminals[0], temporaryStateB));
                int i = 1;

                // constantly subtract from the length to ensure the right terminals are done. 
                while (i < terminals.Length - 1)
                {
                    string newStateA = STATE_DEFINER_SYMBOL + currentStateCounter;
                    string newStateB = STATE_DEFINER_SYMBOL + (currentStateCounter + 1);
                    ndfa.AddTransition(new Transition<string>(newStateA, terminals[i], newStateB));
                    currentStateCounter++;
                    i++;
                }
                // bring the temporary-state up to date
                temporaryStateB = STATE_DEFINER_SYMBOL + currentStateCounter;
                ndfa.AddTransition(new Transition<string>(temporaryStateB, terminals[i], stateB));
                currentStateCounter++;
            }
        }
    }
}
