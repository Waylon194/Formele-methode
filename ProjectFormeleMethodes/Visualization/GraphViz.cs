using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;
using ProjectFormeleMethodes.Visualization;
using QuikGraph.Graphviz;
using System;
using System.Collections.Generic;
/* TODO: Add graphviz files...
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
*/

namespace ProjectFormeleMethodes
{
    class GraphViz
    {
        public static void PrintGraph(Automata<string> data, string filename)
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
                toPrint += " " + ("S" + t) + " ";
            }
            toPrint += "; ";
            toPrint += " ";
            toPrint += "node [shape = circle];";

            foreach (string state in startStates)
            {
                toPrint += " " + ("SSS") + "-> " + ("S" + state);
            }

            foreach (Transition<string> t in transitions)
            {
                toPrint += " " + ("S" + t.FromState) + " -> " + ("S" + t.ToState) + " " + "[ label = " + "\"" +
                           t.Symbol + "\"" + " ];";
            }
            toPrint += " }";

            Console.WriteLine(toPrint);

            GenerateGraphFile(toPrint, filename);
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
