using ProjectFormeleMethodes.ConversionEngines.NDFAToDFA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.NDFAToDFA.models
{
    public class StateTable
    {
        //                State,  StateTableRow
        public Dictionary<string, StateTableRow> Table { get; private set; }

        public StateTable()
        {
            this.Table = new Dictionary<string, StateTableRow>();
        }

        public void AddRowToTable(string state, StateTableRow row)
        {
            this.Table.Add(state, row);
        }
    }
}
