using ProjectFormeleMethodes.ConversionEngines.NDFAToDFA.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.NDFAToDFA.Models
{
    public class DFAStateTable
    {
        //               main state         symbol  fullState
        private Dictionary<string, Dictionary<char, string>> _StateTable;

        public DFAStateTable()
        {
            this._StateTable = new Dictionary<string, Dictionary<char, string>>();
        }

        public Dictionary<string, Dictionary<char, string>> GetTable()
        {
            return this._StateTable;
        }

        public Dictionary<string, Dictionary<char, string>> ConvertToStateTable(NDFAHelperTable helperTable)
        {
            string rowState = "";

            // Set fill the state table with base
            foreach (var state in helperTable.HelperTable)
            {
                this._StateTable.Add(state.Key, new Dictionary<char, string>());
                var helperRowData = helperTable.HelperTable.Where(item => item.Key == state.Key).FirstOrDefault();

                foreach (var data in helperRowData.Value)
                {
                    rowState = "";
                    foreach (var row in data.Value)
                    {
                        rowState += row;
                    }
                    this._StateTable[state.Key].Add(data.Key, rowState);
                }
            }
            return this._StateTable;
        }
    }
}
