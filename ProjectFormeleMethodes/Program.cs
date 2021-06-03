// Our own usables
using ProjectFormeleMethodes.ConversionEngines;
using ProjectFormeleMethodes.RegExpressions;
using ProjectFormeleMethodes.NDFA;

using System;
using ProjectFormeleMethodes.Regular_Expression;
using ProjectFormeleMethodes.NDFA.Testing;

namespace ProjectFormeleMethodes
{
    public class Program
    {
        private static RegExp baa, bb, baaOrbb, baaOrbborEp, regPlus, all, a, b, regStar;

        public static void Main(string[] args)
        {
            Automata<string> a = new Automata<string>();

            a = TestAutomata.ExampleSlide8Lesson2();
            Console.WriteLine("Test: " + a.ToString());
            TestAutomata.ExampleSlide8Lesson2();

            GraphVizEngine.PrintGraph(a, "TestGraph");

            //TestRegExpAndThompson();
            //TestLanguage();
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
            var o2 = thomas.ConvertToDFA(all);

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
