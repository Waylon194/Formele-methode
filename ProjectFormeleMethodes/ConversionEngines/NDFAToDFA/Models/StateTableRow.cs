using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.NDFAToDFA.Models
{
    public class StateTableRow
    {
        //                Symbol,  States
        public Dictionary<char, List<string>> ReachableStates { get; private set; }

        public StateTableRow()
        {
            this.ReachableStates = new Dictionary<char, List<string>>();
        }

        public void AddReachableState(char symbol, List<string> reachableStates)
        {
            this.ReachableStates.Add(symbol, reachableStates);
        }

    }
}
