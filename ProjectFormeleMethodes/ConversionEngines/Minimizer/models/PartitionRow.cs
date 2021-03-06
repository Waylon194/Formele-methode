using System;

namespace ProjectFormeleMethodes.ConversionEngines.Minimizer.Models
{
    public class PartitionRow
    {
        public string ToState { get; private set; }

        public char Symbol { get; private set; }

        public char RowLetter { get; private set; }

        public char DesignatedLetter { get; set; }

        [Obsolete]
        public bool isStartState { get; private set; } // OBSOLETE
        public StateSubType SubType { get; private set; }
        public StateSuperType SuperType { get; private set; }

        public PartitionRow(string tostate, char symbol, char rowLetter)
        {
            this.ToState = tostate;
            this.Symbol = symbol;
            this.RowLetter = rowLetter;
        }

        public void SetStateTypes(StateSubType subType, StateSuperType superType)
        {
            this.SubType = subType;
            this.SuperType = superType;
        }
    }
}