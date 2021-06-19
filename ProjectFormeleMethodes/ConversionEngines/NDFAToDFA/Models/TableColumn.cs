using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.NDFAToDFA.Models
{
    public class TableColumn
    {
        public char Symbol { get; private set; }
        public Dictionary<string, SortedSet<string>> ReachableStates { get;set; }

        public TableColumn(char symbol)
        {
            this.Symbol = symbol;
            this.ReachableStates = new Dictionary<string, SortedSet<string>>();
        }

        public void AddReachableState(string state, SortedSet<string> reachableStates)
        {
            if (!this.ReachableStates.ContainsKey(state))
            {
                this.ReachableStates.Add(state, new SortedSet<string>());
            }
            this.ReachableStates[state] = reachableStates;
        }
    }
}
