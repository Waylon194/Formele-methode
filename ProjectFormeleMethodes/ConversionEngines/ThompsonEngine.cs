using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.RegExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ndfaModel.DefineAsStartState("S");
            ndfaModel.DefineAsFinalState("F");

            // we convert the regular expression step by step to a NDFA
            convert(regularExpression, ref ndfaModel);

            return ndfaModel;
        }

        //
        private void convert(RegExp inputExpression, ref Automata<string> outputFormat)
        {
            switch (inputExpression.OperatorType)
            {
                case RegExpOperatorTypes.PLUS:
                    break;
                case RegExpOperatorTypes.STAR:
                    break;
                case RegExpOperatorTypes.OR:
                    break;
                case RegExpOperatorTypes.DOT:
                    break;
                case RegExpOperatorTypes.ONCE:
                    break;
                default:
                    break;
            }
        }

        // 
        private void convertPlusOperator()
        {

        }

        private void convertStarOperator()
        {

        }



            
    }
}
