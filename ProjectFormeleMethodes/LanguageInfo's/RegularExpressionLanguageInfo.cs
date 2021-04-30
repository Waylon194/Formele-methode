using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.Languages
{
    public class RegularExpressionLanguageInfo
    {
        public IList<string> Languages; // e.g. { abbaaabbaaaabaacc ... } the possible options are defined by the Terminals
        public IList<string> Terminals; // e.g. { a, b, aab, ba, c, ...}
        public IList<string> NonTerminals; // e.g. { S, A, B, ... }
    }
}
