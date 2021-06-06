using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.Minimizer.Models
{
    public class StateLetterModel
    {
        public string State { get; private set; }
        public char LetterAssigned { get; private set; }

        public StateLetterModel(string state, char letterAssigned)
        {
            this.State = state;
            this.LetterAssigned = letterAssigned;
        }

        public override string ToString()
        {
            return "State: " + State + ", LAssigned: " + LetterAssigned; 
        }
    }
}
