using ProjectFormeleMethodes.ConversionEngines;
using ProjectFormeleMethodes.ConversionEngines.Minimizer;
using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.RegExpressions;
using ProjectFormeleMethodes.Regular_Expression;
using System;

namespace ProjectFormeleMethodes
{
    public class Tester
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

        public Automata<string> MinimizeDFA(Automata<string> dfa)
        {
            HopcroftEngine converter = new HopcroftEngine();

            return converter.MinimizeDFA(dfa);
        }

        public static void TestRegExpLanguage(RegExp exp)
        {
            // create a new logic manipulator object
            RegExpLogicOperator rLogic = new RegExpLogicOperator();
            Console.WriteLine("taal van (baa):\n" + rLogic.getAcceptedLanguages(exp, 5));
        }
    }
}