using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.NDFAToDFA.Models
{
    public class NDFAHelperTableRow
    {
        //                Symbol,  States
        public Tuple<char, SortedSet<string>> RowData { get; private set; }

        public NDFAHelperTableRow(char symbol)
        {
            this.RowData = new Tuple<char, SortedSet<string>>(symbol, new SortedSet<string>());
        }

        public NDFAHelperTableRow()
        {

        }

        public void SetRowData(char symbol, SortedSet<string> reachableStates)
        {
            if (RowData == null)
            {
                this.RowData = new Tuple<char, SortedSet<string>>(symbol, reachableStates);
            }
        }

        public void AddReachableState(string state)
        {
            this.RowData.Item2.Add(state);
        }
    }
}
