using System;

// Our own usables
using ProjectFormeleMethodes.DFA;
using ProjectFormeleMethodes.RegExpressions;
using ProjectFormeleMethodes.Regular_Expression;

namespace ProjectFormeleMethodes
{
    public class Program
    {
        private static RegExp expr1, expr2, expr3, expr4, expr5, a, b, all;

        public static void Main(string[] args)
        {
            TestRegExp();
            testLanguage();
        }

        public static void TestRegExp()
        {
            a = new RegExp("a");
            b = new RegExp("b");

            // expr1: "baa"
            expr1 = new RegExp("baa");
            // expr2: "bb"
            expr2 = new RegExp("bb");
            // expr3: "(baa | bb)"
            expr3 = expr1.Or(expr2);

            // all: "(a|b)*"
            all = (a.Or(b)).Star();

            // expr4: "(baa | baa)+"
            expr4 = expr3.Plus();

            // expr5: "(baa | baa)+ (a|b)*"
            expr5 = expr4.Dot(all);
        }

        public static void testLanguage()
        {
            //Console.WriteLine("taal van (baa):\n" + expr1.getLanguage(5));
            //Console.WriteLine("taal van (bb):\n" + expr2.getLanguage(5));
            Console.WriteLine("taal van (baa | bb):\n" + expr3.getLanguage(5));

            Console.WriteLine("taal van (a|b)*:\n" + all.getLanguage(5));
            Console.WriteLine("taal van (baa | bb)+:\n" + expr4.getLanguage(5));
            Console.WriteLine("taal van (baa | bb)+ (a|b)*:\n" + expr5.getLanguage(6));
        }
    }
}
