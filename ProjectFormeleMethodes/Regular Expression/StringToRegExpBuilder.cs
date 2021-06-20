using ProjectFormeleMethodes.RegExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.Regular_Expression
{
    /// <summary>
    /// Contains logic handling of the various RegExp methods, such as AND, OR & NOT
    /// </summary>
    public class StringToRegExpBuilder
    {
        public static RegExp StringToRegex(string stringToRegex, RegExp regex)
        {
            // Read the string values to convert to regexp
            for (int index = 0; index < stringToRegex.Length; index++)
            {
                // gets the first character of the string
                char currentChar = stringToRegex[index];

                switch (currentChar)
                {
                    case '(':
                        int closingBracketPosition = -1;
                        int bracketCount = 0;
                        for (int i = index + 1; i < stringToRegex.Length; i++)
                        {
                            if (stringToRegex[i] == '(') bracketCount++;
                            if (stringToRegex[i] == ')' && bracketCount == 0)
                            {
                                closingBracketPosition = i;
                                break;
                            }
                            if (stringToRegex[i] == ')' && bracketCount != 0)
                            {
                                bracketCount--;
                            }
                        }
                        string between = stringToRegex.Substring(index + 1, closingBracketPosition - 1 - index);
                        RegExp regExp = StringToRegex(between, new RegExp());
                        if (closingBracketPosition + 1 < stringToRegex.Length)
                        {
                            index = closingBracketPosition + 1;
                            currentChar = stringToRegex[index];
                            if (currentChar == '+')
                            {
                                regExp = regExp.Plus();
                            }
                            else if (currentChar == '*')
                            {
                                regExp = regExp.Star();
                            }
                        }
                        if (regex.Terminals == "" && regex.OperatorType == RegExpOperatorTypes.ONCE)
                        {
                            regex = regExp;
                        }
                        else
                        {
                            regex = regex.Dot(regExp);
                        }
                        break;

                        // if currentChar is op type PLUS
                    case '+':
                        regex = regex.Plus();
                        break;

                        // if currentChar is op type STAR
                    case '*':
                        regex = regex.Star();
                        break;

                        // if currentChar is op type OR
                    case'|':
                        regex = regex.Or(new RegExp(stringToRegex[index + 1].ToString()));
                        index++;
                        break;

                        // if currentChar is of type ONCE
                    default:
                        if (regex.Terminals == "" && regex.OperatorType == RegExpOperatorTypes.ONCE)
                        {
                            regex.Terminals = currentChar.ToString();
                        }
                        else
                        {
                            regex = regex.Dot(new RegExp(currentChar.ToString()));
                        }
                        break;
                }
            }
            return regex;
        }
    }
}
