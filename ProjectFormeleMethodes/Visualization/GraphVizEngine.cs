using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;
using ProjectFormeleMethodes.Visualization;
using System;
using System.Collections.Generic;

namespace ProjectFormeleMethodes
{
    /// <summary>
    /// 
    /// Credits to https://github.com/MaurodeLyon/Formele-methoden/blob/master/Formele%20methoden/GraphVizParser.cs
    /// 
    /// </summary>
    public class GraphVizEngine
    {
        public static void PrintGraph(Automata<string> data, string filename)
        {
            try
            {
                string toPrint = "";
                toPrint += "digraph{";
                toPrint += " ";

                toPrint += "{ node[style = invis, shape = none, label = \" \", width = 0, height = 0] SSS }";

                toPrint += " ";
                toPrint += "node [shape = doublecircle];";

                ISet<Transition<string>> transitions = data.Transitions;

                SortedSet<string> finalStates = data.FinalStates;
                SortedSet<string> startStates = data.StartStates;

                foreach (string t in finalStates)
                {
                    toPrint += " " + (t) + " ";
                }
                toPrint += "; ";
                toPrint += " ";
                toPrint += "node [shape = circle];";

                foreach (string state in startStates)
                {
                    toPrint += " " + ("SSS") + "-> " + (state);
                }

                foreach (Transition<string> t in transitions)
                {
                    toPrint += " " + (t.FromState) + " -> " + (t.ToState) + " " + "[ label = " + "\"" +
                               t.Symbol + "\"" + " ];";
                }
                toPrint += " }";

                Console.WriteLine(toPrint);

                GenerateGraphFile(toPrint, filename);
            }
            catch (NullReferenceException nullEx)
            {
                Console.WriteLine("Something went wrong to get a null reference exception");
                Console.WriteLine("Check the following object: {0}", nullEx.Message);
            }    
        }

        static void GenerateGraphFile(string data, string filename)
        {
            string filePath = $@"..\..\..\Visualization\Graphs\{filename}";
            
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath + ".dot"))
            {
                file.Write(data);
            }

            DotGraphEngine.Run(filePath);
        }
    }
}