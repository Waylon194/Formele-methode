using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.Minimizer.Models
{
    public class PartitionRow
    {
        public string ToState { get; private set; }

        public char Symbol { get; private set; }

        public char RowLetter { get; private set; }

        public char DesignatedLetter { get; set; }

        public StateType type { get; private set; }

        public PartitionRow(string tostate, char symbol, char rowLetter)
        {
            this.ToState = tostate;
            this.Symbol = symbol;
            this.RowLetter = rowLetter;
        }

        public void SetStateType(StateType type)
        {
            this.type = type;
        }
    }
}
