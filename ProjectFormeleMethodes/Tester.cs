using ProjectFormeleMethodes.ConversionEngines;
using ProjectFormeleMethodes.ConversionEngines.Minimizer;
using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.RegExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
