using ProjectFormeleMethodes.ConversionEngines.NDFAToDFA.models;
using ProjectFormeleMethodes.ConversionEngines.NDFAToDFA.Models;
using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectFormeleMethodes.ConversionEngines.NDFAToDFA
{
    public class NDFAToDFAEngineOld
    {
        private readonly char EPSILON = 'ɛ';
        private readonly string EMPTY_STATE_ID = "";

        public NDFAToDFAEngineOld()
        {

        }

        public Automata<string> Convert(Automata<string> ndfaToConvert)
        {
            // create our automata object (DFA)
            Automata<string> dfa = new Automata<string>(ndfaToConvert.Symbols);

            // begin search for Epsilon closures
            var hTable = generateHelperTable(ndfaToConvert); // this generates our helperTable to easily add a switch values around

            // convert it to a suitable format which can be used for proper transition assignment
            DFAStateTable stateTable = new DFAStateTable();

            // we can now use the StateTable to assign the transitions
            finalizeConversion(hTable, ref dfa);
            return dfa;
        }

        private NDFAHelperTable generateHelperTable(Automata<string> ndfa)
        {
            // Helper table which contains all of the reachable states 
            NDFAHelperTable helperTable = new NDFAHelperTable();
            // gather all the available states
            var allStates = ndfa.States;
            var validSymbols = ndfa.Symbols;

            // remove the epsilon symbol from the validSymbols
            validSymbols.Remove(EPSILON);

            foreach (var state in allStates) // current state to search for
            {
                foreach (var symbol in validSymbols) // current symbol to search for
                {
                    determineEpsilonClosures(state, symbol, ndfa, ref helperTable);
                }
            }

            // the helper table is now filled, but states 
            return helperTable;
        }

        private void determineEpsilonClosures(string stateToSearch, char symbolToSearch, Automata<string> auto, ref NDFAHelperTable helperTable)
        {
            List<Transition<string>> transitions = auto.GetTransition(stateToSearch);

            SortedSet<string> reachableStates = new SortedSet<string>();

            // gets the correct element, if no element is found this means no transition for the symbol is found and an EMPTY transition must be created
            var transitionToGet = transitions.Where(item => item.Symbol == symbolToSearch).FirstOrDefault(); 

            if (transitionToGet != null)
            {
                // start reachable state searcher
                var transitionPath = auto.GetTransition(transitionToGet.ToState); // get all transition after the 'a'-symbol transition with the 

                foreach (var symbolToRemove in auto.Symbols)
                {
                    transitionPath.RemoveAll(item => item.Symbol == symbolToRemove);
                }

                // add all possible new transitions to the helperTable transition row
                foreach (var item in transitionPath)
                {
                    reachableStates.Add(item.FromState); // add state from
                    reachableStates.Add(item.ToState); // add state to
                }

                helperTable.AddRowToTable(stateToSearch, symbolToSearch, reachableStates);
            }
            else if (transitionToGet == null) // if no state was found with the appropriate letter this means no state transition with the symbol was found. 
            {
                reachableStates.Add(EMPTY_STATE_ID); // set the empty state for the given symbol
                helperTable.AddRowToTable(stateToSearch, symbolToSearch, reachableStates); // add the row to the helper table
            }
        }

        private SortedSet<string> getAllTotalStates(Dictionary<string, Dictionary<char, string>> stateTable)
        {
            SortedSet<string> states = new SortedSet<string>();
            states.Add(stateTable.FirstOrDefault().Key);

            foreach (var rows in stateTable)
            {
                foreach (var singleRow in rows.Value)
                {
                    var rowStates = singleRow.Value;
                    string rowState = "";
                    foreach (var item in rowStates)
                    {
                        rowState += item;
                    }
                    states.Add(rowState);
                }
            }
            return states;
        }

        private void finalizeConversion(NDFAHelperTable helperTable, ref Automata<string> dfa)
        {
            var dfaTable = new Dictionary<string, Dictionary<char, string>>();

            var start = helperTable.HelperTable.FirstOrDefault();

            dfaTable.Add(start.Key, new Dictionary<char, string>());


            string combinedStates = ""; // keeps track of the combined state

            foreach (var allStates in helperTable.GetAllTotalStates())
            {
                if (!dfaTable.ContainsKey(allStates))
                {
                    dfaTable.Add(allStates, new Dictionary<char, string>());
                }

                if (allStates.Equals(EMPTY_STATE_ID))
                {

                }
                else
                {
                    Dictionary<char, SortedSet<string>> combined = new Dictionary<char, SortedSet<string>>();
                    Dictionary<char, string> dfaTableData = new Dictionary<char, string>();

                    foreach (var rowInformationFull in helperTable.HelperTable)
                    {
                        foreach (var rowStatesInfo in rowInformationFull.Value)
                        {
                            if (!dfaTableData.ContainsKey(rowStatesInfo.Key))
                            {
                                dfaTableData.Add(rowStatesInfo.Key, "");
                            }
                            if (!combined.ContainsKey(rowStatesInfo.Key))
                            {
                                combined.Add(rowStatesInfo.Key, new SortedSet<string>());
                            }
                            combinedStates = "";

                            combined[rowStatesInfo.Key].UnionWith(rowStatesInfo.Value);                            

                            if (!dfaTable.ContainsKey(combinedStates))
                            {
                                dfaTable.Add(combinedStates, new Dictionary<char, string>());
                            }
                        }
                    }
                    Console.WriteLine();
                    foreach (var reachableStatesCollection in combined)
                    {
                        combinedStates = "";
                        foreach (var reachableState in reachableStatesCollection.Value)
                        {
                            combinedStates += reachableState; // create a string with the values of the collection
                        }

                        dfaTableData[reachableStatesCollection.Key] = combinedStates; // assign states to the dfaTable row and column
                    }
                    dfaTable[allStates] = dfaTableData;
                }              
            }
            Console.WriteLine();

            //var allStates = helperTable.GetAllTotalStates();

            //var cpyHelperTable = new NDFAHelperTable(helperTable.HelperTable);

            //var dfaStateTable = new Dictionary<string, Dictionary<char, SortedSet<string>>>();

            //string combinedState = "";

            //allStates.Remove(EMPTY_STATE_ID);

            //foreach (var state in allStates)
            //{
            //    Dictionary<char, SortedSet<string>> sSet = new Dictionary<char, SortedSet<string>>();
            //    dfaStateTable.Add(state, new Dictionary<char, SortedSet<string>>());
            //    combinedState = "";

            //    foreach (var item in helperTable.HelperTable)
            //    {
            //        var tableRow = cpyHelperTable.HelperTable[item.Key];

            //        foreach (var smbl in dfa.Symbols)
            //        {
            //            if (!sSet.ContainsKey(smbl))
            //            {
            //                sSet.Add(smbl, new SortedSet<string>());
            //            }
            //            sSet[smbl].UnionWith(tableRow[smbl]);
            //        }
            //    }
            //    dfaStateTable[state] = sSet;
            //}

            //foreach (var state in allStates)
            //{
            //    optimzedStateTable.Add(state, new Dictionary<char, string>()); // add the available state to the dictionary
            //    foreach (var symbol in dfa.Symbols)
            //    {
            //        foreach (var tableRow in helperTable.HelperTable)
            //        {
            //            if (state.Contains(tableRow.Key))
            //            {

            //            }
            //            else if(state.Equals(EMPTY_STATE_ID))
            //            {

            //            }
            //        }
            //    }

            //}
        }
    }
}
