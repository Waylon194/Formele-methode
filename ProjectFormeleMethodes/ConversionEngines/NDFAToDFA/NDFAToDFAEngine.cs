using ProjectFormeleMethodes.ConversionEngines.NDFAToDFA.Models;
using ProjectFormeleMethodes.NDFA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFormeleMethodes.ConversionEngines.NDFAToDFA
{
    public class NDFAToDFAEngine
    {
        public NDFAToDFAEngine()
        {

        }

        public Automata<string> Convert(Automata<string> ndfa)
        {
            Automata<string> dfa = new Automata<string>(ndfa.Symbols);

            var helperTable = createEmptyTable(ndfa);
            createHelperTable(ndfa, ref helperTable);

            return dfa;
        }

        private void createHelperTable(Automata<string> ndfa, ref Table helperTable)
        {
            foreach (var states in helperTable.Columns)
            {
                
            }
        }

        private Table createEmptyTable(Automata<string> ndfa)
        {
            Table helpTable = new Table();
            helpTable.AvailableStates = ndfa.States;

            var symbols = ndfa.Symbols;
            symbols.Remove('ɛ'); // remove epsilon from the list of symbols

            foreach (var symbol in ndfa.Symbols)
            {
                helpTable.AddColumn(new TableColumn(symbol));
            }
            return helpTable;
        }
    }
}
