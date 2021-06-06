using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.Minimizer.Example
{
    public class HopCroftAlgor
    {
        public static Automata<string> MinimizeDfa(Automata<string> automaat)
        {
            SortedSet<PartitionExample> partitions = new SortedSet<PartitionExample>();
            PartitionExample nonFinals = new PartitionExample('A');
            PartitionExample finals = new PartitionExample('B');

            // Setup first step of minimalisation (ready given Automaat for the recursion method
            SortedSet<string> states = automaat.States;
            foreach (string state in states)
            {
                List<Transition<string>> trans = automaat.GetTransition(state);
                RowExample row = new RowExample();
                foreach (Transition<string> t in trans)
                {
                    row.AddRoute(t.Symbol, t.ToState);
                }

                if (automaat.FinalStates.Contains(state))
                {
                    finals.AddRow(state, row);
                }
                else
                {
                    nonFinals.AddRow(state, row);
                }
            }

            partitions.Add(nonFinals);
            partitions.Add(finals);

            // Enter minimizing recursion
            partitions = MinimizePartitions(partitions);

            // Define start and final states
            foreach (PartitionExample p in partitions)
            {
                foreach (string finalState in automaat.FinalStates)
                {
                    if (p.ContainsState(finalState))
                    {
                        p.IsFinal = true;
                    }
                }
                foreach (string startState in automaat.StartStates)
                {
                    if (p.ContainsState(startState))
                    {
                        p.IsStart = true;
                    }
                }
            }

            return RetrieveDfaFromPartitions(partitions, automaat.Symbols);
        }

        private static SortedSet<PartitionExample> MinimizePartitions(SortedSet<PartitionExample> partitions)
        {
            List<Dictionary<string, SortedSet<string>>> splittedPartitions =
                new List<Dictionary<string, SortedSet<string>>>();
            SortedSet<PartitionExample> newPartitions = new SortedSet<PartitionExample>();

            foreach (PartitionExample p in partitions)
            {
                Dictionary<string, SortedSet<string>> newPartition = new Dictionary<string, SortedSet<string>>();
                foreach (KeyValuePair<string, RowExample> row in p.Rows)
                {
                    string phrase = "";
                    foreach (KeyValuePair<char, string> innerRow in row.Value.InnerRows)
                    {
                        foreach (PartitionExample p2 in partitions)
                        {
                            if (p2.ContainsState(innerRow.Value))
                            {
                                phrase += p2.Identifier + "-";
                            }
                        }
                    }
                    phrase = phrase.TrimEnd('-');
                    if (!newPartition.ContainsKey(phrase))
                    {
                        newPartition.Add(phrase, new SortedSet<string>());
                    }

                    newPartition[phrase].Add(row.Key);
                }
                splittedPartitions.Add(newPartition);
            }

            int index = 0;
            foreach (Dictionary<string, SortedSet<string>> newPartition in splittedPartitions)
            {
                foreach (KeyValuePair<string, SortedSet<string>> entry in newPartition)
                {
                    PartitionExample partition = new PartitionExample((char)(index + 65));
                    foreach (string s in entry.Value)
                    {
                        foreach (PartitionExample p2 in partitions)
                        {
                            if (p2.ContainsState(s))
                            {
                                partition.AddRow(s, p2.GetRow(s));
                                break;
                            }
                        }
                    }
                    newPartitions.Add(partition);
                    index++;
                }
            }

            if (newPartitions.Count != partitions.Count)
            {
                newPartitions = MinimizePartitions(newPartitions);
            }
            return newPartitions;
        }


        private static Automata<string> RetrieveDfaFromPartitions(SortedSet<PartitionExample> partitions,
            SortedSet<char> symbols)
        {
            Automata<string> automaat = new Automata<string>(symbols);
            foreach (PartitionExample p in partitions)
            {
                if (p.IsFinal)
                    automaat.DefineAsFinalState(p.Identifier.ToString());
                if (p.IsStart)
                    automaat.DefineAsStartState(p.Identifier.ToString());
                foreach (KeyValuePair<string, RowExample> row in p.Rows)
                {
                    foreach (KeyValuePair<char, string> innerRow in row.Value.InnerRows)
                    {
                        string toState = "";
                        foreach (PartitionExample p2 in partitions)
                        {
                            if (p2.ContainsState(innerRow.Value))
                            {
                                toState = p2.Identifier.ToString();
                                break;
                            }
                        }
                        automaat.AddTransition(new Transition<string>(p.Identifier.ToString(), innerRow.Key, toState));
                    }
                }
            }

            return automaat;
        }

        public static void PrintPartitions(SortedSet<PartitionExample> partitions)
        {
            Console.WriteLine("-----------------------------------------------------------");
            foreach (PartitionExample p in partitions)
            {
                Console.WriteLine(p.Identifier + ":");

                foreach (KeyValuePair<string, RowExample> row in p.Rows)
                {
                    Console.Write("     " + row.Key + ":");

                    foreach (KeyValuePair<char, string> innerRow in row.Value.InnerRows)
                    {
                        Console.Write(" " + innerRow.Key + "-" + innerRow.Value + " ");
                    }

                    Console.WriteLine("");
                }
            }
        }
    }


    public class PartitionExample : IComparable<PartitionExample>
    {
        //State and row
        //Example 1: a-2 b-3
        public Dictionary<string, RowExample> Rows { get; }

        public char Identifier { get; }

        public bool IsFinal { get; set; }

        public bool IsStart { get; set; }
        // private SortedSet<char> symbols;

        public PartitionExample(char id)
        {
            Identifier = id;
            IsFinal = false;
            IsStart = false;
            Rows = new Dictionary<string, RowExample>();
        }

        public void AddRow(string state, RowExample row)
        {
            Rows[state] = row;
        }

        public RowExample GetRow(string state)
        {
            return Rows[state];
        }

        public bool ContainsState(string state)
        {
            foreach (KeyValuePair<string, RowExample> row in Rows)
            {
                if (row.Key == state)
                {
                    return true;
                }
            }
            return false;
        }

        public int CompareTo(PartitionExample other)
        {
            if (other == null) return 1;

            return Identifier.CompareTo(other.Identifier);
        }
    }

    public class RowExample
    {
        //innerRow means a char of the alphabet + its corresponding state
        //Example char:a string:2
        public Dictionary<char, string> InnerRows { get; }

        public RowExample()
        {
            InnerRows = new Dictionary<char, string>();
        }

        public void AddRoute(char symbol, string toState)
        {
            InnerRows[symbol] = toState;
        }
    }
}
