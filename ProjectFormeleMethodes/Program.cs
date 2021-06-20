// Our own usables
using ProjectFormeleMethodes.ConversionEngines;
using ProjectFormeleMethodes.ConversionEngines.Minimizer;
using ProjectFormeleMethodes.Examples;
using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;
using ProjectFormeleMethodes.Examples;
using ProjectFormeleMethodes.RegExpressions;
using ProjectFormeleMethodes.Regular_Expression;
using System;
using System.Collections.Generic;

namespace ProjectFormeleMethodes
{
    public class Program
    {
        private static bool GraphVizEngineTOGGLE = false;

        public static void Main(string[] args)
        {

        }

        public static void Testing()
        {
            // Testing if the UnionWith methode can merge both SortedSets
            SortedSet<string> statesPartOne = new SortedSet<string>();
            statesPartOne.Add("A");
            statesPartOne.Add("B");
            statesPartOne.Add("C");
            statesPartOne.Add("D");

            SortedSet<string> statesPartTwo = new SortedSet<string>();
            statesPartTwo.Add("A");
            statesPartTwo.Add("B");
            statesPartTwo.Add("F");
            statesPartTwo.Add("E");

            // the answer is yes
            statesPartOne.UnionWith(statesPartTwo);

            Console.WriteLine();//
        }    
    }
}
