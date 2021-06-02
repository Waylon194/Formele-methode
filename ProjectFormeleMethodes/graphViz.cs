using System;
using System.Collections.Generic;
/* TODO: Add graphviz files...
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
*/

namespace ProjectFormeleMethodes
{
    class graphViz
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
            //Errors below will be fixed once imports are fixed
            GetStartProcessQuery getStartProcessQuery = new GetStartProcessQuery();
            GetProcessStartInfoQuery getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            RegisterLayoutPluginCommand registerLayoutPluginCommand =
                new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);

            GraphGeneration wrapper = new GraphGeneration(getStartProcessQuery,
                getProcessStartInfoQuery,
                registerLayoutPluginCommand);

            //byte[] output = wrapper.GenerateGraph(data, Enums.GraphReturnType.Jpg);
            //System.IO.File.WriteAllBytes("Images/" + filename + ".jpg", output);
        }
    }
}
