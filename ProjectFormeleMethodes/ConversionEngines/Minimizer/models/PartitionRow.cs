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

        public bool isStartState { get; private set; }
        public StateSubType SubType { get; private set; }
        public StateSuperType SuperType { get; private set; }

        public PartitionRow(string tostate, char symbol, char rowLetter, bool isStartState)
        {
            this.ToState = tostate;
            this.Symbol = symbol;
            this.RowLetter = rowLetter;
            this.isStartState = isStartState;
        }

        public void SetStateTypes(StateSubType subType, StateSuperType superType)
        {
            this.SubType = subType;
            this.SuperType = superType;
        }
    }
}
