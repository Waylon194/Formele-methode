// Our own usables
using ProjectFormeleMethodes.Examples;
using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.RegExpressions;
using System;

namespace ProjectFormeleMethodes
{
    public class Program
    {
        private static bool GraphVizEngineTOGGLE = false;
        private static TesterContainer testerUtil = new TesterContainer();

        public static void Main(string[] args)
        {
            //var normal = DFABuilder.BuildDFASampleOne();
            //var flipped = testerUtil.CreateNotVariantOfAutomata(normal);

            Console.WriteLine();

            var result = ConsoleBuilderSelecterTest();

            if (result != null)
            {
                ConsoleExecute(result.Item2, result.Item1);
            }
        }

        public static RegExp CreateRegExp(int choice, string regexp = "")
        {
            switch (choice)
            {
                case 1:
                    return RegExpBuilder.BuildRegExpSampleOne();

                case 2:
                    return RegExpBuilder.BuildRegExpSampleTwo();

                case 3:
                    return RegExpBuilder.BuildCustomRegExp(regexp);

                default:
                    return RegExpBuilder.BuildRegExpSampleTwo();
            }
        }

        public static Automata<string> CreateNDFA(int choice)
        {
            switch (choice)
            {
                case 1:
                    return NDFABuilder.BuildNDFASampleOne();

                case 2:
                    return NDFABuilder.BuildNDFASampleTwo();

                case 3:
                    return NDFABuilder.BuildNDFASampleThree();

                case 4:
                    return NDFABuilder.BuildNDFASampleFour();

                default:
                    return NDFABuilder.BuildNDFASampleOne();
            }
        }

        public static Automata<string> CreateDFA(int choice)
        {
            switch (choice)
            {
                case 1:
                    return DFABuilder.BuildDFASampleOne();

                case 2:
                    return DFABuilder.BuildDFASampleTwo();

                case 3:
                    return DFABuilder.BuildDFASampleThree();

                default:
                    return DFABuilder.BuildDFASampleOne();
            }
        }

        private static void handleRegExpChoice(RegExp regExp)
        {
            Console.WriteLine("Reguliere expressies - Conversie");
            Console.WriteLine("Welke optie?");
            //Hier stond DNFA, maar moet NDFA zijn denk ik
            Console.WriteLine("1: RegExp->NDFA");
            Console.WriteLine("2: RegExp->NDFA->DFA");
            Console.WriteLine("3: RegExp->NDFA->DFA->Minimaliseren");
            Console.WriteLine("-1: Stoppen");
            Console.WriteLine("0: Terug\n");

            Console.Write("=>> ");
            int value = -1;
            int.TryParse(Console.ReadLine(), out value);

            switch (value)
            {
                case 1: // RegExp -> NDFA
                        // Convert Regexp to NDFA via Thompson
                    var ndfa = testerUtil.ConvertRegExpToNDFA(regExp);
                    GraphVizEngine.PrintGraph(ndfa, "RegExpThompsonConvertedNDFAOption1");
                    break;

                case 2: // RegExp -> NDFA -> DFA
                        // Convert Regexp to NDFA via Thompson
                    ndfa = testerUtil.ConvertRegExpToNDFA(regExp);
                    // Convert NDFA to DFA
                    var dfa = testerUtil.ConvertNDFAToDFA(ndfa);
                    GraphVizEngine.PrintGraph(dfa, "RegExpNDFAConvertedDFAOption2");
                    break;

                case 3: // RegExp -> NDFA -> DFA -> Minimaliseren
                        // Convert Regexp to NDFA via Thompson
                    ndfa = testerUtil.ConvertRegExpToNDFA(regExp);
                    // Convert NDFA to DFA
                    dfa = testerUtil.ConvertNDFAToDFA(ndfa);
                    // Minimize DFA
                    dfa = testerUtil.MinimizeDFA(dfa);
                    GraphVizEngine.PrintGraph(dfa, "RegExpMinimizedDFAOption3");
                    break;

                // stoppen
                case -1:
                    break;

                    // terug
                case 0:
                    ConsoleBuilderSelecterTest();
                    break;

                default: // RegExp -> NDFA -> DFA -> Minimaliseren - Default
                         // Convert Regexp to NDFA via Thompson
                    ndfa = testerUtil.ConvertRegExpToNDFA(regExp);
                    // Convert NDFA to DFA
                    dfa = testerUtil.ConvertNDFAToDFA(ndfa);
                    // Minimize DFA
                    dfa = testerUtil.MinimizeDFA(dfa);
                    GraphVizEngine.PrintGraph(dfa, "RegExpMinimizedDFAOptionDEFAULT");
                    break;
            }
        }

        private static void handleDFAChoice(Automata<string> dfaGiven)
        {
            Console.WriteLine("DFA expressies - Conversie/Veranderen");
            Console.WriteLine("Welke optie?");
            Console.WriteLine("1: DFA->Minimaliseren");
            Console.WriteLine("2: Adjust DFA");
            Console.WriteLine("-1: Stoppen");
            Console.WriteLine("0: Terug\n");

            Console.Write("=>> ");
            int value = -1;
            int.TryParse(Console.ReadLine(), out value);

            switch (value)
            {
                case 1:
                    // Minimize DFA
                    var dfa = testerUtil.MinimizeDFA(dfaGiven);
                    GraphVizEngine.PrintGraph(dfa, "DFAMinimizedDFAOption1");
                    break;

                case 2: // Adjust DFA
                    // TODO
                    break;

                // stoppen
                case -1:
                    break;

                    // terug 
                case 0:
                    ConsoleBuilderSelecterTest();
                    break;

                default:  // Minimize DFA - Default
                    dfa = testerUtil.MinimizeDFA(dfaGiven);
                    GraphVizEngine.PrintGraph(dfa, "DFAMinimizedDFAOption1");
                    break;
            }
        }

        private static void handleNDFAChoice(dynamic ndfaGiven)
        {
            Console.WriteLine("DFA expressies - Conversie/Veranderen");
            Console.WriteLine("Welke optie?");
            Console.WriteLine("1: NDFA->DFA");
            Console.WriteLine("2: NDFA->DFA->Minimaliseren");
            Console.WriteLine("3: Adjust DFA");
            Console.WriteLine("-1: Stoppen");
            Console.WriteLine("0: Terug\n");

            Console.Write("=>> ");
            int value = -1;
            int.TryParse(Console.ReadLine(), out value);

            switch (value)
            {
                case 1: // NDFA -> DFA                    
                    // Convert NDFA to DFA
                    var dfa = testerUtil.ConvertNDFAToDFA(ndfaGiven);
                    GraphVizEngine.PrintGraph(dfa, "NDFANDFAConvertedDFAOption1");
                    break;

                case 2: // NDFA -> DFA -> Minimaliseren
                    // Convert NDFA to DFA
                    dfa = testerUtil.ConvertNDFAToDFA(ndfaGiven);
                    // Minimize DFA
                    dfa = testerUtil.MinimizeDFA(dfa);
                    GraphVizEngine.PrintGraph(dfa, "NDFAMinimizedDFAOption2");
                    break;

                case 3: // Adjust NDFA
                    // TODO
                    break;

                    // stoppen
                case -1:
                    break;

                case 0:
                    ConsoleBuilderSelecterTest();
                    break;

                default:  // Minimize DFA - Default
                    // Convert NDFA to DFA
                    dfa = testerUtil.ConvertNDFAToDFA(ndfaGiven);
                    // Minimize DFA
                    dfa = testerUtil.MinimizeDFA(dfa);
                    GraphVizEngine.PrintGraph(dfa, "NDFAMinimizedDFAOption2");
                    break;
            }
        }

        public static void ConsoleExecute(dynamic returnedValue, bool isNDFA)
        {
            switch (returnedValue)
            {
                case RegExp:
                    handleRegExpChoice(returnedValue);
                    break;

                case Automata<string>:
                    if (isNDFA)
                    {
                        handleNDFAChoice(returnedValue);
                    }
                    else
                    {
                        handleDFAChoice(returnedValue);
                    }
                    break;

                default:
                    break;
            }
        }

        public static Tuple<bool, dynamic> ConsoleBuilderSelecterTest()
        {
            Console.WriteLine("Test Formele Methode");
            Console.WriteLine("Welke optie?");
            Console.WriteLine("1: RegExp");
            Console.WriteLine("2: NDFA");
            Console.WriteLine("3: DFA");
            Console.WriteLine("-1: Stoppen\n");

            Console.Write("=>> ");
            int value = -1;
            int.TryParse(Console.ReadLine(), out value);

            switch (value)
            {
                case 1: // Reguliere expressies
                    Console.WriteLine("\nReguliere Expressies: ");
                    Console.WriteLine("Welke optie?");
                    Console.WriteLine("1: Sample 1");
                    Console.WriteLine("2: Sample 2");
                    Console.WriteLine("3: Eigen maken");
                    Console.WriteLine("0: Terug\n");

                    Console.Write("=>> ");
                    value = -1;
                    int.TryParse(Console.ReadLine(), out value);

                    switch (value)
                    {
                        case 1:
                        case 2:
                            return new Tuple<bool, dynamic>(false, CreateRegExp(value));

                        case 3:
                            Console.WriteLine("Maak eigen expressie");
                            Console.Write(">");
                            string input = Console.ReadLine();
                            return new Tuple<bool, dynamic>(false, CreateRegExp(value, input));

                            // Terug
                        case 0:
                            ConsoleBuilderSelecterTest();
                            break;

                        default:
                            return new Tuple<bool, dynamic>(false, CreateRegExp(-1)); // back to default        
                    }
                    break;

                case 2: // NDFA expressies
                    Console.WriteLine("\nNDFA Expressies: ");
                    Console.WriteLine("Welke optie?");
                    Console.WriteLine("1: Sample 1");
                    Console.WriteLine("2: Sample 2");
                    Console.WriteLine("3: Sample 3");
                    Console.WriteLine("4: Sample 4");
                    Console.WriteLine("0: Terug\n");

                    Console.Write("=>> ");
                    value = -1;
                    int.TryParse(Console.ReadLine(), out value);

                    switch (value)
                    {
                        case 1:
                            return new Tuple<bool, dynamic>(true, CreateNDFA(value));


                        case 2:
                            return new Tuple<bool, dynamic>(true, CreateNDFA(value));


                        case 3:
                            return new Tuple<bool, dynamic>(true, CreateNDFA(value));


                        case 4:
                            return new Tuple<bool, dynamic>(true, CreateNDFA(value));

                        // Terug
                        case 0:
                            ConsoleBuilderSelecterTest();
                            break;

                        default:
                            return new Tuple<bool, dynamic>(true, CreateNDFA(-1)); // back to default 

                    }
                    break;

                case 3: // DFA expressies
                    Console.WriteLine("\nDFA Expressies: ");
                    Console.WriteLine("Welke optie?");
                    Console.WriteLine("1: Sample 1");
                    Console.WriteLine("2: Sample 2");
                    Console.WriteLine("3: Sample 3");
                    Console.WriteLine("3: Terug\n");

                    Console.Write("=>> ");
                    value = -1;
                    int.TryParse(Console.ReadLine(), out value);

                    switch (value)
                    {
                        case 1:
                            return new Tuple<bool, dynamic>(false, CreateDFA(value));

                        case 2:
                            return new Tuple<bool, dynamic>(false, CreateDFA(value));

                        case 3:
                            return new Tuple<bool, dynamic>(false, CreateDFA(value));

                        // Terug
                        case 0:
                            ConsoleBuilderSelecterTest();
                            break;

                        default:
                            return new Tuple<bool, dynamic>(false, CreateDFA(-1)); // back to default 
                    }
                    break;

                case -1:
                    // stoppen met de loop
                    break;

                default:
                    ConsoleBuilderSelecterTest();
                    break;
            }
            return null;
        }    
    }
}