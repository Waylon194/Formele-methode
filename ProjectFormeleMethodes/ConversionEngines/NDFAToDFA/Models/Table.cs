using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.NDFAToDFA.Models
{
    public class Table
    {
        // rows
        public SortedSet<string> AvailableStates;
        public List<TableColumn> Columns;

        public Table()
        {
            this.Columns = new List<TableColumn>();              
        }

        public void AddColumn(TableColumn column)
        {
            this.Columns.Add(column);
        }
    }
}
