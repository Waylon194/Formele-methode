using ProjectFormeleMethodes.ConversionEngines;
using ProjectFormeleMethodes.ConversionEngines.Minimizer;
using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.RegExpressions;
using ProjectFormeleMethodes.Regular_Expression;
using System;

namespace ProjectFormeleMethodes
{
    public class TesterContainer
    {
        public Automata<string> ConvertRegExpToNDFA(RegExp regx)
        {
            ThompsonEngine converter = new ThompsonEngine();

            return converter.ConvertRegExpToNDFA(regx);
        }

        public Automata<string> ConvertNDFAToDFA(Automata<string> nDFA)
        {
            NDFAToDFAEngine converter = new NDFAToDFAEngine();

            return converter.Convert(nDFA);
        }

        public Automata<string> MinimizeDFAHopCroft(Automata<string> dfa)
        {
            Console.WriteLine("Minimizing DFA - HopCroft.....");

            HopcroftEngine converter = new HopcroftEngine();

            return converter.MinimizeDFA(dfa);
        }

        public Automata<string> MinimizeDFAReverseMethod(Automata<string> dfa)
        {
            Console.WriteLine("Minimizing DFA - Reversed.....");

            var optimizedDfa = dfa.MinimizeDFAReversedAlgorithm(dfa);

            return optimizedDfa;
        }

        public void TestRegExpLanguage(RegExp exp)
        {
            // create a new logic manipulator object
            RegExpLogicOperator rLogic = new RegExpLogicOperator();
            Console.Write("Language of {0}");
            exp.PrintRegularExpression();
            Console.WriteLine("Language:\n" + rLogic.getAcceptedLanguages(exp, 5));
        }

        public Automata<string> CreateNotVariantOfAutomata(Automata<string> toFlip)
        {
            // simple reverse of DFA
            return Automata<string>.CreateReversedVariant(toFlip);
        }
    }
}