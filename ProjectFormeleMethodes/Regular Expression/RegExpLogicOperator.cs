using ProjectFormeleMethodes.RegExpressions;
using System;
using System.Collections.Generic;

namespace ProjectFormeleMethodes.Regular_Expression
{
    /// <summary>
    /// Here methods will come such as, starts & ends with, includes & equals 
    /// </summary>
    public class RegExpLogicOperator : IComparer<string>
    {
        private RegExp exp;

        public RegExpLogicOperator()
        {

        }

        // Returns language which is accepted 
        public SortedSet<string> getAcceptedLanguages(RegExp expression, int amountOfIterations)
        {
            SortedSet<string> emptyLanguage = new SortedSet<string>(this); // this type gets added to ensure the end of the recursion
            SortedSet<string> languageResult = new SortedSet<string>(this); // this is the variable which keeps tracks of the possibilities

            SortedSet<string> languageLeft, languageRight; // languages of the left and right sides of an expression

            if (amountOfIterations < 1)
            {
                return emptyLanguage;
            }

            switch (expression.OperatorType)
            {
                case RegExpOperatorTypes.ONCE:
                    languageResult.Add(expression.Terminals);
                    break;

                case RegExpOperatorTypes.OR:
                    // Condition description
                    // LanguageLeft.left == null:
                    //  When true -> {languageLeft = emptyLanguage }
                    //  When false -> { languageLeft = this.left.getLanguage(maxSteps - 1) }
                    languageLeft = expression.Left == null ? emptyLanguage : getAcceptedLanguages(expression.Left, amountOfIterations - 1);
                    languageRight = expression.Right == null ? emptyLanguage : getAcceptedLanguages(expression.Right, amountOfIterations - 1);

                    languageResult.UnionWith(languageLeft);
                    languageResult.UnionWith(languageRight);
                    break;

                case RegExpOperatorTypes.DOT:
                    languageLeft = expression.Left == null ? emptyLanguage : getAcceptedLanguages(expression.Left, amountOfIterations - 1);
                    languageRight = expression.Right == null ? emptyLanguage : getAcceptedLanguages(expression.Right, amountOfIterations - 1);
                    foreach (string s1 in languageLeft)
                        foreach (string s2 in languageRight)
                        { languageResult.Add(s1 + s2); }
                    break;

                // STAR(*) and PLUS(+) operators are almost exactly the same, *=0 or more, +=1 or more:
                case RegExpOperatorTypes.STAR:
                case RegExpOperatorTypes.PLUS:
                    languageLeft = expression.Left == null ? emptyLanguage : getAcceptedLanguages(expression.Left, amountOfIterations - 1);
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
                    if (expression.OperatorType == RegExpOperatorTypes.STAR) // if the regular expression is a star variant add the possiblity of "" (epsilon)
                    {
                        languageResult.Add("");
                    }
                    break;

                default:
                    Console.WriteLine("getLanguage() encountered an unknown operator: " + expression.OperatorType.ToString());
                    break;
            }
            if (this.exp != null)
            {
                this.exp.CurrentAcceptedLanguage = languageResult;
            }
            return languageResult;
        }

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
    }
}