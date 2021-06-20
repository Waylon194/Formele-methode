// Our own usables
using ProjectFormeleMethodes.ConversionEngines;
using ProjectFormeleMethodes.ConversionEngines.Minimizer;
using ProjectFormeleMethodes.ConversionEngines.Minimizer.Example;
using ProjectFormeleMethodes.ConversionEngines.NDFAToDFA;
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

        private static RegExp baa, bb, baaOrbb, baaOrbborEp, regPlus, all, a, b, regStar;
        private static List<RegExp> regExps = new List<RegExp>();

        public static void Main(string[] args)
        {
            var ndfa = GenerateNDFA();
            //GraphVizEngine.PrintGraph(ndfa, "NDFAGraph");

            NDFAToDFAEngine toDFAEngine = new NDFAToDFAEngine(); // NDFAtoDFAEngine 
            var dfaOpt = toDFAEngine.Convert(ndfa);

            //Console.WriteLine();
            ////RunTest();
            ///

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

            rexp.PrintRegularExpression();

            // Test Thompson Conversion
            // Getting a NDFA model
            ThompsonEngine thomas = new ThompsonEngine();
            var ndfa = thomas.ConvertRegExpToDFA(rexp);
            //var dnfa =ThompsonConstructionExample.ConvertRegExp(rexp); // example version

            var ndfaForTesting = GenerateNDFA(); 

            // Getting a DFA from a NDFA
            //Automata<string> dfa = NDFAtoDFAEngineExample.Convert(ndfa); // NDFAtoDFAEngine Example version
            NDFAToDFAEngine toDFAEngine = new NDFAToDFAEngine(); // NDFAtoDFAEngine 
            var dfaOpt = toDFAEngine.Convert(ndfaForTesting);

            // Own Thompson engine
            HopcroftEngine hopEngine = new HopcroftEngine();
            var optimizedDFAOwn = hopEngine.MinimizeDFA(ndfaForTesting);

            //var optimizedDFAOwnExample = HopCroftAlgorExample.MinimizeDfa(dfa); // example version
            Console.WriteLine();

            if (GraphVizEngineTOGGLE)
            {
                GraphVizEngine.PrintGraph(ndfaForTesting, "TestGraphNDFA");
            }

            //GraphVizEngine.PrintGraph(dfa, "TestGraphPreMinimizedSample");
            //GraphVizEngine.PrintGraph(optimizedDFAOwn, "TestGraphOwnDesignSample");
            //GraphVizEngine.PrintGraph(optimizedDFAOwnExample, "TestGraphExampleSample");

            //TestRegExpAndThompson();
            //TestLanguage();
        }

        public static Automata<string> GenerateNDFA()
        {
            Automata<string> ndfa = new Automata<string>();
            ndfa.Symbols.Add('a'); 
            ndfa.Symbols.Add('b'); 
            ndfa.Symbols.Add('ɛ');

            ndfa.DefineAsStartState("q0");
            ndfa.DefineAsFinalState("qF");

            // transitions of first state
            ndfa.AddTransition(new Transition<string>("q0", 'a', "q1"));
            ndfa.AddTransition(new Transition<string>("q0", 'a', "q2"));
            ndfa.AddTransition(new Transition<string>("q0", 'b', "q3"));

            // transitions of second state
            ndfa.AddTransition(new Transition<string>("q1", 'a', "q2"));
            ndfa.AddTransition(new Transition<string>("q1", 'b', "q0"));
            ndfa.AddTransition(new Transition<string>("q1", 'ɛ', "q2"));

            // transitions of third state
            ndfa.AddTransition(new Transition<string>("q2", 'a', "q2"));
            ndfa.AddTransition(new Transition<string>("q2", 'ɛ', "q3"));
            ndfa.AddTransition(new Transition<string>("q2", 'a', "qF"));

            // transitions of fourth state
            ndfa.AddTransition(new Transition<string>("q3", 'a', "qF"));

            // transitions of final state
            ndfa.AddTransition(new Transition<string>("qF", 'a', "q3"));

            return ndfa;
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
