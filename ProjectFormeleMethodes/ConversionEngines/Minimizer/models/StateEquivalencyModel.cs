using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.Minimizer.Models
{
    public class StateEquivalencyModel
    {
        public Dictionary<char, int> SymbolOccurence;

        public StateEquivalencyModel()
        {
            this.SymbolOccurence = new Dictionary<char, int>();
        }
    }
}
