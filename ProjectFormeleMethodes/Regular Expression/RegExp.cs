using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.RegExpressions
{
    public class RegExp : IComparer<string>
    {
        public string Terminals;

        // Possible operators for a Reg Exp ([+], [*], [|], [.]) 
        // Daarnaast ook een operator definitie voor 1 keer repeteren (default)
        public RegExpOperatorTypes OperatorType;

        public RegExp left;
        public RegExp right;
        public SortedSet<string> curLang;

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
            this.OperatorType = RegExpOperatorTypes.ONCE;
            this.Terminals = "";
            this.left = null;
            this.right = null;
        }

        //  constructor
        public RegExp(string p)
        {
            this.OperatorType = RegExpOperatorTypes.ONCE;
            this.Terminals = p;
            this.left = null;
            this.right = null;
        }

        // definitions of the operators
        public RegExp Plus() 
        {
            RegExp result = new RegExp();
            result.OperatorType = RegExpOperatorTypes.PLUS;
            result.left = this;
            return result;
        }

        public RegExp Star()
        {
            RegExp result = new RegExp();
            result.OperatorType = RegExpOperatorTypes.STAR;
            result.left = this;
            return result;
        }

        public RegExp Or(RegExp e2)
        {
            RegExp result = new RegExp();
            result.OperatorType = RegExpOperatorTypes.OR;
            result.left = this;
            result.right = e2;
            return result;
        }

        public RegExp Dot(RegExp e2)
        {
            RegExp result = new RegExp();
            result.OperatorType = RegExpOperatorTypes.DOT;
            result.left = this;
            result.right = e2;
            return result;
        }

        // getLanguages
        public SortedSet<string> getLanguage(int amountOfIterations)
        {
            SortedSet<string> emptyLanguage = new SortedSet<string>(this); // this type gets added to ensure the end of the recursion
            SortedSet<string> languageResult = new SortedSet<string>(this); // this is the variable which keeps tracks of the possibilities

            SortedSet<string> languageLeft, languageRight; // languages of the left and right sides of an expression

            if (amountOfIterations < 1)
            {
                return emptyLanguage;
            }

            switch (this.OperatorType)
            {
                case RegExpOperatorTypes.ONCE:
                    languageResult.Add(this.Terminals);
                    break;

                case RegExpOperatorTypes.OR:
                    // Condition description
                    // LanguageLeft.left == null:
                    //  When true -> {languageLeft = emptyLanguage }
                    //  When false -> { languageLeft = this.left.getLanguage(maxSteps - 1) }
                    languageLeft = this.left == null ? emptyLanguage : this.left.getLanguage(amountOfIterations - 1);
                    languageRight = this.right == null ? emptyLanguage : this.right.getLanguage(amountOfIterations - 1);

                    languageResult.UnionWith(languageLeft);
                    languageResult.UnionWith(languageRight);
                    break;

                case RegExpOperatorTypes.DOT:
                    languageLeft = this.left == null ? emptyLanguage : this.left.getLanguage(amountOfIterations - 1);
                    languageRight = this.right == null ? emptyLanguage : this.right.getLanguage(amountOfIterations - 1);
                    foreach (string s1 in languageLeft)
                        foreach (string s2 in languageRight)
                        { languageResult.Add(s1 + s2); }
                    break;

                // STAR(*) and PLUS(+) operators are almost exactly the same, *=0 or more, +=1 or more:
                case RegExpOperatorTypes.STAR:
                case RegExpOperatorTypes.PLUS:
                    languageLeft = this.left == null ? emptyLanguage : this.left.getLanguage(amountOfIterations - 1);
                    languageResult.UnionWith(languageLeft);
                    for (int i = 1; i < amountOfIterations; i++)
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
                    if (this.OperatorType == RegExpOperatorTypes.STAR) // if the regular expression is a star variant add the possiblity of "" (epsilon)
                    {
                        languageResult.Add("");
                    }
                    break;

                default:
                    Console.WriteLine("getLanguage() encountered an unknown operator: " + this.OperatorType.ToString());
                    break;
            }
            return languageResult;
        }
    }
}

