using System;

// Our own usables
using ProjectFormeleMethodes.ConversionEngines;
using ProjectFormeleMethodes.RegExpressions;
using ProjectFormeleMethodes.Languages;
using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Testing;

namespace ProjectFormeleMethodes
{
    public class Program
    {
        private static RegExp baa, bb, baaOrbb, regPlus, all, a, b, regStar;

        public static void Main(string[] args)
        {
            Automata<string> a = new Automata<string>();

            a = TestAutomata.ExampleSlide8Lesson2();
            Console.WriteLine("Test: " + a.ToString());
            //TestAutomata.ExampleSlide8Lesson2();
            
        }

        public static void TestRegExp()
        {
            a = new RegExp("a");
            b = new RegExp("b");

            // expr1: "baa"
            baa = new RegExp("baa");

            // expr2: "bb"
            bb = new RegExp("bb");

            // expr3: "(baa | bb)"
            baaOrbb = baa.Or(bb);

            // all: "(a|b)*"
            regStar = (a.Or(b)).Star();

            // expr4: "(baa | bb)+"
            regPlus = baaOrbb.Plus();

            // expr5: "(baa | bb)+ (a|b)*"
            all = regPlus.Dot(regStar);
        }

        public static void TestLanguage()
        {
            Console.WriteLine("taal van (baa):\n" + baa.getLanguage(5));
            Console.WriteLine("taal van (bb):\n" + bb.getLanguage(5));
            Console.WriteLine("taal van (baa | bb):\n" + baaOrbb.getLanguage(5));

            Console.WriteLine("taal van (a|b)*:\n" + regStar.getLanguage(5));
            Console.WriteLine("taal van (baa | bb)+:\n" + regPlus.getLanguage(5));
            Console.WriteLine("taal van (baa | bb)+ (a|b)*:\n" + all.getLanguage(6));
        }
    }
}
