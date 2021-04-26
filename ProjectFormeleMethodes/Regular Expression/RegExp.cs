using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.Regular_Expression
{
    public class RegExp : IComparer<string>
    {
        public string Terminals;

        // Possible operators for a Reg Exp ([+], [*], [|], [.]) 
        // Daarnaast ook een operator definitie voor 1 keer repeteren (default)
        public OperatorTypes Operator;

        public RegExp left;
        public RegExp right;

        public int Compare(string s1, string s2)
        {
            if (s1.Length == s2.Length)
            { 
                return s1.CompareTo(s2); 
            }
            else
            { 
                return s1.Length - s2.Length; 
            }
        }

        // default constructor
        public RegExp()
        {
            Operator = OperatorTypes.ONCE;
            Terminals = "";
            left = null;
            right = null;
        }

        //  constructor
        public RegExp(string p)
        {
            Operator = OperatorTypes.ONCE;
            Terminals = p;
            left = null;
            right = null;
        }

        // definitions of the operators
        public RegExp Plus()
        {
            RegExp result = new RegExp();
            result.Operator = OperatorTypes.PLUS;
            result.left = this;
            return result;
        }

        public RegExp Star()
        {
            RegExp result = new RegExp();
            result.Operator = OperatorTypes.STAR;
            result.left = this;
            return result;
        }

        public RegExp Or(RegExp e2)
        {
            RegExp result = new RegExp();
            result.Operator = OperatorTypes.OR;
            result.left = this;
            result.right = e2;
            return result;
        }

        public RegExp Dot(RegExp e2)
        {
            RegExp result = new RegExp();
            result.Operator = OperatorTypes.DOT;
            result.left = this;
            result.right = e2;
            return result;
        }

        // getLanguages
        public SortedSet<String> getLanguage(int maxSteps)
        {
            SortedSet<string> emptyLanguage = new SortedSet<string>(this);
            SortedSet<string> languageResult = new SortedSet<string>(this);

            SortedSet<string> languageLeft, languageRight;

            if (maxSteps < 1) 
            {
                return emptyLanguage;
            } 

            switch (this.Operator) {

            case OperatorTypes.ONCE:
                languageResult.Add(this.Terminals);

                    break;

            case OperatorTypes.OR:
                languageLeft = left == null ? emptyLanguage : left.getLanguage(maxSteps - 1);
                languageRight = right == null ? emptyLanguage : right.getLanguage(maxSteps - 1);

                languageResult.Union(languageLeft);
                languageResult.Union(languageRight);
                    break;                

            case OperatorTypes.DOT:
                languageLeft = left == null ? emptyLanguage : left.getLanguage(maxSteps - 1);
                languageRight = right == null ? emptyLanguage : right.getLanguage(maxSteps - 1);
                foreach (string s1 in languageLeft)
                    foreach (string s2 in languageRight)
                    { languageResult.Add(s1 + s2); }                
                    break;

            // STAR(*) en PLUS(+) kunnen we bijna op dezelfde manier uitwerken:
            case OperatorTypes.STAR:
            case OperatorTypes.PLUS:
                languageLeft = left == null ? emptyLanguage : left.getLanguage(maxSteps - 1);
                languageResult.Union(languageLeft);
                for (int i = 1; i < maxSteps; i++)
                {
                    HashSet<string> languageTemp = new HashSet<string>(languageResult);
                    foreach (string s1 in languageLeft)
                    {
                        foreach (string s2 in languageTemp)
                        {
                            languageResult.Add(s1 + s2);
                        }
                    }
                }                
                    if (this.Operator == OperatorTypes.STAR)
                    {
                        languageResult.Add("");
                    }                
                    break;

                default:
                Console.WriteLine("getLanguage() encountered an unknown operator: " + this.Operator.ToString());
                    break;
            }
            return languageResult;
        }
    }
}

