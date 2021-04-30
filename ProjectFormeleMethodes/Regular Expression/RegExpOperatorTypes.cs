using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.RegExpressions
{
    public enum RegExpOperatorTypes
    {
        // +    *      |    o     default
        PLUS,   // '+' -> used when occuring once       : e.g.    (a+)
        STAR,   // '*' -> used when occuring once       : e.g.    (b)*
        OR,     // '|' -> used when occuring once       : e.g.    (a | b)
        DOT,    // '.' -> used when occuring once       : e.g.    (a . b)
        ONCE    // 'default' Used when occuring once    : e.g.    (a)
    }
}
