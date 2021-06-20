using ProjectFormeleMethodes.RegExpressions;
using ProjectFormeleMethodes.Regular_Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.Examples
{
    public class RegExpBuilder
    {
        public static RegExp BuildRegExpSampleOne()
        {
            RegExp baa, bb, baaOrbb, regPlus, all, a, b, regStar;

            a = new RegExp("a");
            b = new RegExp("b");

            // expr1: "baa"
            baa = new RegExp("baa");

            // expr2: "bb"
            bb = new RegExp("bb");

            // expr3: "(baa | bb)"
            baaOrbb = baa.Or(bb);

            // expr4: "(a|b)*"
            regStar = (a.Or(b)).Star();

            // expr5: "(baa | bb)+"
            regPlus = baaOrbb.Plus();

            // all: "(baa | bb)+ ⋅ (a|b)*"
            all = regPlus.Dot(regStar);

            return all;
        }

        public static RegExp BuildRegExpSampleTwo()
        {
            RegExp rexp = new RegExp("a");
            RegExp b = new RegExp("b");
            rexp = rexp.Or(b).Plus();

            return rexp;
        }

        public static RegExp BuildCustomRegExp(string regex)
        {
            return StringToRegExpBuilder.StringToRegex(regex, new RegExp());
        }
    }
}
