using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;
using ProjectFormeleMethodes.RegExpressions;
using System;

/// <summary>
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
            ndfaModel.DefineAsStartState("qS"); // leftState
            ndfaModel.DefineAsFinalState("qF"); // rightState
            int currentStateCounter = 1; // let the states begin counting from 1. 

            // we convert the regular expression step by step to a NDFA, make ndfaModel and stateCounter referable, to make it possible to use a call-by-reference design.
            convert(regularExpression, ref ndfaModel, ref currentStateCounter, "qS", "qF"); 



            return ndfaModel;
        }

        //
        private void convert(RegExp expression, ref Automata<string> ndfa, ref int currentStateCounter, string leftState, string rightState)
        {
            switch (expression.OperatorType)
            {
                case RegExpOperatorTypes.PLUS:
                    plusConversion(expression, ref ndfa, ref currentStateCounter, leftState, rightState); // convert plus to NDFA
                    break;

                case RegExpOperatorTypes.STAR:
                    starConversion(expression, ref ndfa, ref currentStateCounter, leftState, rightState); // convert star to NDFA
                    break;

                case RegExpOperatorTypes.OR:
                    orConversion(expression, ref ndfa, ref currentStateCounter, leftState, rightState); // convert or to NDFA
                    break;

                case RegExpOperatorTypes.DOT:
                    dotConversion(expression, ref ndfa, ref currentStateCounter, leftState, rightState); // convert dot to NDFA
                    break;

                case RegExpOperatorTypes.ONCE:
                    onceConversion(expression, ref ndfa, ref currentStateCounter, leftState, rightState); // convert once to NDFA
                    break;

                default:
                    Console.WriteLine("Unsupported Operation!");
                    break;
            }
        }

        // 
        private void plusConversion(RegExp expression, ref Automata<string> ndfa, ref int currentStateCounter, string stateA, string stateB)
        {
            // "create" the new states
            string stateC = "q" + currentStateCounter;
            string stateD = "q" + (currentStateCounter + 1);
            currentStateCounter += 2; // up the state counter by 2 times to keep the keep track of the new states

            // add the new states to the automata / NDFA
            ndfa.AddTransition(new Transition<string>(stateA, 'ɛ', stateC)); // bind the incoming start state to the new state
            ndfa.AddTransition(new Transition<string>(stateD, 'ɛ', stateC)); // bind the second new state to the first new state 
            ndfa.AddTransition(new Transition<string>(stateD, 'ɛ', stateB)); // bind the second new state to the last incoming state 

            // set the new "A" and "B" states and call the convert method again to start another conversion
            string newStateA = stateC; // the new start state 
            string newStateB = stateD; // the new end state 

            // call the convert method
            convert(expression, ref ndfa, ref currentStateCounter, newStateA, newStateB);
        }

        private void starConversion(RegExp expression, ref Automata<string> ndfa, ref int currentStateCounter, string stateA, string stateB)
        {
            // "create" the new states
            string stateC = "q" + currentStateCounter;
            string stateD = "q" + (currentStateCounter + 1);
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
            convert(expression, ref ndfa, ref currentStateCounter, newStateA, newStateB);
        }

        private void orConversion(RegExp expression, ref Automata<string> ndfa, ref int currentStateCounter, string leftState, string rightState)
        {

        }

        private void dotConversion(RegExp expression, ref Automata<string> ndfa, ref int currentStateCounter, string leftState, string rightState)
        {

        }

        private void onceConversion(RegExp expression, ref Automata<string> ndfa, ref int currentStateCounter, string leftState, string rightState)
        {
            // "create" the new states
            string stateA = "q" + currentStateCounter;
            string stateB = "q" + (currentStateCounter + 1);
            currentStateCounter += 2; // up the state counter by 2 times to keep the keep track of the states

            // add the new states to the automata / NDFA

        }
    }
}
