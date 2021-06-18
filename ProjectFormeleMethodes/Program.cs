// Our own usables
using ProjectFormeleMethodes.ConversionEngines;
using ProjectFormeleMethodes.ConversionEngines.Minimizer;
using ProjectFormeleMethodes.ConversionEngines.Minimizer.Example;
using ProjectFormeleMethodes.ConversionEngines.NDFAToDFA;
using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.RegExpressions;
using ProjectFormeleMethodes.Regular_Expression;
using System;
using System.Collections.Generic;

namespace ProjectFormeleMethodes
{
    public class Program
    {
        private static RegExp baa, bb, baaOrbb, baaOrbborEp, regPlus, all, a, b, regStar;
        private static List<RegExp> regExps = new List<RegExp>();

        public static void Main(string[] args)
        {
            RunTest();
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

        public static void RunTest()
        {
            RegExp regB, regBaa, regBb;
            regB = new RegExp("b");

            // expr1: "baa"
            regBaa = new RegExp("baa");

            // expr2: "bb"
            regBb = new RegExp("bb");

            regExps.Add(regB);
            regExps.Add(regBaa);
            regExps.Add(regBb);

            RegExp rexp = new RegExp("a");
            RegExp b = new RegExp("b");
            rexp = rexp.Or(b).Plus();

            //rexp = GenerateRandomRegExp(5);
            rexp.PrintRegularExpression();

            // Test Thompson Conversion
            // Getting a NDFA model
            ThompsonEngine thomas = new ThompsonEngine();
            var ndfa = thomas.ConvertRegExpToDFA(rexp);
            //var dnfa =ThompsonConstructionExample.ConvertRegExp(rexp);

            // Getting a DFA from a NDFA
            //Automata<string> dfa = NDFAtoDFAEngineExample.Convert(ndfa); // NDFAtoDFAEngine Example
            NDFAToDFAEngine toDFAEngine = new NDFAToDFAEngine(); // NDFAtoDFAEngine 
            var dfaOpt = toDFAEngine.Convert(ndfa);

            // Own Thompson engine
            HopcroftEngine hopEngine = new HopcroftEngine();
            var optimizedDFAOwn = hopEngine.MinimizeDFA(dfaOpt);

            //var optimizedDFAOwnExample = HopCroftAlgorExample.MinimizeDfa(dfa);
            Console.WriteLine();

            GraphVizEngine.PrintGraph(ndfa, "TestGraphNDFA");

            //GraphVizEngine.PrintGraph(dfa, "TestGraphPreMinimizedSample");
            //GraphVizEngine.PrintGraph(optimizedDFAOwn, "TestGraphOwnDesignSample");
            //GraphVizEngine.PrintGraph(optimizedDFAOwnExample, "TestGraphExampleSample");

            //TestRegExpAndThompson();
            //TestLanguage();
        }

        public static RegExp GenerateRandomRegExp(int remainingSteps)
        {


            //while (remainingSteps > 0 )
            //{
            //    Random randomizer = new Random();
            //    int randValue = randomizer.Next(0, 4);


            //    remainingSteps--;
            //}
            //return rexp;
            return null;
        }

        public static void TestRegExpAndThompson()
        {
            a = new RegExp("a");
            b = new RegExp("b");

            // expr1: "baa"
            baa = new RegExp("baa");

            // expr2: "bb"
            bb = new RegExp("bb");

            // expr3: "(baa | bb)"
            baaOrbb = baa.Or(bb);

            // expr4: "(baa | bb | ɛ)"
            //baaOrbborEp = baaOrbb.AndOr(new RegExp());

            // expr5: "(a|b)*"
            regStar = (a.Or(b)).Star();

            // expr6: "(baa | bb)+"
            regPlus = baaOrbb.Plus();

            // all: "(baa | bb)+ ⋅ (a|b)*"
            all = regPlus.Dot(regStar);

            //all.PrintRegularExpression();

            all.PrintRegularExpression();

            TestLanguage();

            // Test Thompson Conversion
            ThompsonEngine thomas = new ThompsonEngine();
            var o2 = thomas.ConvertRegExpToDFA(all);

            Console.WriteLine();
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
