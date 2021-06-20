using System.Collections.Generic;

namespace ProjectFormeleMethodes.ConversionEngines.Minimizer.Models
{
    public class StateEquivalencyModel
    {
        public Dictionary<char, int> SymbolOccurence;
        public StateSubType SubType = StateSubType.NonEnd;
        public StateSuperType SuperType;
        public bool IsStartState = false;

        public StateEquivalencyModel()
        {
            this.SymbolOccurence = new Dictionary<char, int>();
        }
    }
}
