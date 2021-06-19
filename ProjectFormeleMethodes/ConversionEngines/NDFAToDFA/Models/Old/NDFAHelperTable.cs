using ProjectFormeleMethodes.ConversionEngines.NDFAToDFA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.NDFAToDFA.models
{
    public class NDFAHelperTable
    {
        //                State,   symbol           reachable states
        public Dictionary<string, Dictionary<char, SortedSet<string>>> HelperTable { get; private set; }

        public NDFAHelperTable()
        {
            this.HelperTable = new Dictionary<string, Dictionary<char, SortedSet<string>>>();
        }

        public NDFAHelperTable(Dictionary<string, Dictionary<char, SortedSet<string>>> table)
        {
            this.HelperTable = table;
        }

        public void AddRowToTable(string state, char symbol, SortedSet<string> reachableStates)
        {
            if (!this.HelperTable.ContainsKey(state))
            {
                this.HelperTable.Add(state, new Dictionary<char, SortedSet<string>>()); // set the new state
            }
            this.HelperTable[state].Add(symbol, reachableStates); // add the new state
        }

        public SortedSet<string> GetAllTotalStates()
        {
            SortedSet<string> states = new SortedSet<string>();
            states.Add(HelperTable.FirstOrDefault().Key);

            foreach (var rows in this.HelperTable)
            {
                foreach (var singleRow in rows.Value)
                {
                    var rowStates = singleRow.Value;
                    string rowState = "";
                    foreach (var item in rowStates)
                    {
                        rowState += item;
                    }
                    states.Add(rowState);
                }
            }
            return states;
        }
    }
}
