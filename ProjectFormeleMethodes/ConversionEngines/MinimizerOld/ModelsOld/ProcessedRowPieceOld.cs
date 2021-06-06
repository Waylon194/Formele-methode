using System.Collections.Generic;

namespace ProjectFormeleMethodes.ConversionEngines.Minimizer.models
{
    /// <summary>
    /// Class which makes keeping track of simple blockids a simple task
    /// </summary>
    public class ProcessedRowPieceOld
    {
        public string StateId { get; private set; }
        public List<string> PieceIds { get; } 

        public ProcessedRowPieceOld(string stateId)
        {
            this.PieceIds = new List<string>();
            this.StateId = stateId;
        }

        public void AddPieceId(string id) 
        {
            this.PieceIds.Add(id);        
        }
    }
}