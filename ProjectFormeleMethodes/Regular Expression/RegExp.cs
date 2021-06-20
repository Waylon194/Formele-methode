using System;
using System.Collections.Generic;

namespace ProjectFormeleMethodes.RegExpressions
{
    public class RegExp
    {
        public string Terminals;

        // Possible operators for a Reg Exp ([+], [*], [|], [.]) 
        public RegExpOperatorTypes OperatorType;
        
        public RegExp Left;
        public RegExp Right;
        public string CurrentRegularExpression;

        // ?
        public SortedSet<string> CurrentAcceptedLanguage = null;
        public SortedSet<string> CurrentUnacceptedLanguage = null;

        // default constructor
        public RegExp()
        {
            this.OperatorType = RegExpOperatorTypes.ONCE;
            this.Terminals = "";
            this.Left = null;
            this.Right = null;
        }

        //  constructor
        public RegExp(string p)
        {
            this.OperatorType = RegExpOperatorTypes.ONCE;
            this.Terminals = p;
            this.Left = null;
            this.Right = null;

            // keep track of the current regular expression
            this.CurrentRegularExpression = p;

            
        }

        // definitions of the operators
        public RegExp Plus() 
        {
            RegExp result = new RegExp();
            result.OperatorType = RegExpOperatorTypes.PLUS;
            result.Left = this;

            // keep track of the current regular expression
            result.CurrentRegularExpression = this.CurrentRegularExpression + "+";
            return result;
        }

        public RegExp Star()
        {
            RegExp result = new RegExp();
            result.OperatorType = RegExpOperatorTypes.STAR;
            result.Left = this;

            // keep track of the current regular expression
            result.CurrentRegularExpression = this.CurrentRegularExpression + "*";
            return result;
        }

        public RegExp Or(RegExp e2)
        {
            RegExp result = new RegExp();
            result.OperatorType = RegExpOperatorTypes.OR;
            result.Left = this;
            result.Right = e2;

            // keep track of the current regular expression
            result.CurrentRegularExpression ="(" + this.CurrentRegularExpression + " | " + e2.CurrentRegularExpression + ")";
            return result;
        }

        public RegExp Dot(RegExp e2)
        {
            RegExp result = new RegExp();
            result.OperatorType = RegExpOperatorTypes.DOT;
            result.Left = this;
            result.Right = e2;

            // keep track of the current regular expression
            result.CurrentRegularExpression = this.CurrentRegularExpression + " ⋅ " + e2.CurrentRegularExpression;
            return result;
        }

        public void PrintRegularExpression()
        {
            if (this.CurrentRegularExpression != null)
            {
                Console.WriteLine("This is the current Regular Expression:");
                Console.WriteLine(this.ToString() + "\n");
            }
            else
            {
                Console.WriteLine("Regular Expression Empty!\n");
            }
        }

        public override string ToString()
        {
            if (this.CurrentRegularExpression != null)
            {
                return this.CurrentRegularExpression;
            }
            return "Regular Expression Empty!";
        }
    }
}