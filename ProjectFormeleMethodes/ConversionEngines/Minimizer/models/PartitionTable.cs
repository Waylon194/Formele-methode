using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectFormeleMethodes.ConversionEngines.Minimizer.Models
{
    public class PartitionTable
    {
        public List<Tuple<string, PartitionRow>> Rows { get; set; } // fromState, row({string} toState,{char} symbol,{char} designatedLetter, {StateType} type)
        public Automata<string> DFA;
        public char LetterIndex = 'A'; // 65 
        public List<StateLetterModel> StateLetters;

        public PartitionTable(Automata<string> dfa)
        {
            this.Rows = new List<Tuple<string, PartitionRow>>();
            this.StateLetters = new List<StateLetterModel>();
            this.DFA = dfa;
            this.LetterIndex--; // prepare the letter for use
        }

        public List<Tuple<StateLetterModel, char>> GetColumn(char letterToGet, char symbolFilter, string stateToFilter)
        {
            List<Tuple<StateLetterModel, char>> stateLetter = new List<Tuple<StateLetterModel, char>>();
            var data = this.Rows.Where(item => item.Item2.RowLetter.Equals(letterToGet) 
                && item.Item2.Symbol.Equals(symbolFilter)
                && !item.Item1.Equals(stateToFilter)).ToList();

            foreach (var item in data)
            {
                stateLetter.Add(new Tuple<StateLetterModel, char>(new StateLetterModel(item.Item1, item.Item2.DesignatedLetter), item.Item2.Symbol));
            }
            return stateLetter;
        }

        public string GetCorrectLetterByState(string state)
        {
            foreach (var item in StateLetters)
            {
                if (item.State.Equals(state))
                {
                    return item.LetterAssigned.ToString();
                }
            }
            return state;
        }

        public List<Tuple<string, PartitionRow>> GetRowsByState(char letterToGet, string stateToFilter)
        {
            var data = this.Rows.Where(item => item.Item2.RowLetter.Equals(letterToGet)
                && item.Item1.Equals(stateToFilter)).ToList();

            return data.ToList();
        }


        public List<Tuple<string, PartitionRow>> GetRowsExpect(string fromStateKey, char letterToGet)
        {
            List<Tuple<string, PartitionRow>> copyOfRows;
            copyOfRows = this.Rows.Where(item => item.Item2.RowLetter.Equals(letterToGet) && !item.Item1.Equals(fromStateKey)).ToList();
            return copyOfRows;
        }

        public void AddRowsToPartitionTable(Dictionary<string, StateSuperType> states, StateSubType subType, bool isStartState, bool allowLetterTick)
        {
            if (allowLetterTick)
            {
                this.LetterIndex++;
            }

            PartitionRow pRow;
            foreach (var state in states)
            {
                this.StateLetters.Add(new StateLetterModel(state.Key, this.LetterIndex));
                List<Transition<string>> transitions = this.DFA.GetTransition(state.Key);
                foreach (var transition in transitions)
                {
                    pRow = new PartitionRow(transition.ToState, transition.Symbol, LetterIndex);
                    pRow.SetStateTypes(subType, state.Value);
                    this.Rows.Add(new Tuple<string, PartitionRow>(state.Key, pRow));
                }
            }
        }

        public void SetCorrectDesignatedLetters()
        {
            foreach (var row in this.Rows)
            {
                foreach (var sLetter in this.StateLetters)
                {
                    if (row.Item2.ToState.Equals(sLetter.State))
                    {
                        row.Item2.DesignatedLetter = sLetter.LetterAssigned;
                    }
                }
            }
        }
    }
}