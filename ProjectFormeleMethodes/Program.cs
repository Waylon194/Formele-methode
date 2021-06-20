// Our own usables
using ProjectFormeleMethodes.ConversionEngines;
using ProjectFormeleMethodes.ConversionEngines.Minimizer;
using ProjectFormeleMethodes.Examples;
using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;
using ProjectFormeleMethodes.RegExpressions;
using ProjectFormeleMethodes.Regular_Expression;
using System;
using System.Collections.Generic;

namespace ProjectFormeleMethodes
{
    public class Program
    {
        private static bool GraphVizEngineTOGGLE = false;

        public static void Main(string[] args)
        {
            //var ndfa = GenerateNDFA();
            ////GraphVizEngine.PrintGraph(ndfa, "NDFAGraph");

            //NDFAToDFAEngine toDFAEngine = new NDFAToDFAEngine(); // NDFAtoDFAEngine 
            //var dfaOpt = toDFAEngine.Convert(ndfa);

            //GraphVizEngine.PrintGraph(dfaOpt, "NDFAToDFAGraph");

            

            Console.WriteLine();
            RunConversionTestFull();
            

            //TestNFA test = new TestNFA();
            //test.Test();
        }

        public static void Testing()
        {
            // Testing if the UnionWith methode can merge both SortedSets
            SortedSet<string> statesPartOne = new SortedSet<string>();
            statesPartOne.Add("A");
            statesPartOne.Add("B");
            statesPartOne.Add("C");
            statesPartOne.Add("D");

            SortedSet<string> statesPartTwo = new SortedSet<string>();
            statesPartTwo.Add("A");
            statesPartTwo.Add("B");
            statesPartTwo.Add("F");
            statesPartTwo.Add("E");

            // the answer is yes
            statesPartOne.UnionWith(statesPartTwo);

            Console.WriteLine();//
        }

        public static void RunConversionTestFull()
        {          

            // Test Thompson Conversion
            // Getting a NDFA model
            ThompsonEngine thomas = new ThompsonEngine();

            GraphVizEngine.PrintGraph(DFABuilder.BuildDFASampleOne(), "ThompsonEngineConversion");

            // Test NDFAToDFA
            // Getting a DFA from a NDFA
            NDFAToDFAEngine toDFAEngine = new NDFAToDFAEngine(); // NDFAtoDFAEngine 
            var dfa = toDFAEngine.Convert(thompsonNdfa);
            GraphVizEngine.PrintGraph(dfa, "NDFAToDFAConversion");

            // HopCroftEngine Test
            HopcroftEngine hopEngine = new HopcroftEngine();
            var optimizedDFAOwn = hopEngine.MinimizeDFA(dfa);
            GraphVizEngine.PrintGraph(optimizedDFAOwn, "MinimizedDFA");

            if (GraphVizEngineTOGGLE)
            {
                //GraphVizEngine.PrintGraph(optimizedDFAOwn, "TestGraphNDFA");
            }
        }

        private static Automata<string> NDFABuilder()
        {
            throw new NotImplementedException();
        }

        public static void TestLanguage(RegExp exp = null)
        {
            // create a new logic manipulator object
            RegExpLogicOperator rLogic = new RegExpLogicOperator(exp);

            Console.WriteLine("taal van (baa):\n" + rLogic.getAcceptedLanguages(baa, 5));
            Console.WriteLine("taal van (bb):\n" + rLogic.getAcceptedLanguages(bb, 5));
            Console.WriteLine("taal van (baa | bb):\n" + rLogic.getAcceptedLanguages(baaOrbb, 5));

            Console.WriteLine("taal van (a|b)*:\n" + rLogic.getAcceptedLanguages(regStar, 5));
            Console.WriteLine("taal van (baa | bb)+:\n" + rLogic.getAcceptedLanguages(regPlus, 5));
            Console.WriteLine("taal van (baa | bb)+ (a|b)*:\n" + rLogic.getAcceptedLanguages(all, 6));
        }
    }
}
