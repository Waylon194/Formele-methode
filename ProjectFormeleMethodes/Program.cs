// Our own usables
using ProjectFormeleMethodes.ConversionEngines;
using ProjectFormeleMethodes.ConversionEngines.Minimizer;
using ProjectFormeleMethodes.Examples;
using ProjectFormeleMethodes.NDFA;
using ProjectFormeleMethodes.NDFA.Transitions;
using ProjectFormeleMethodes.RegExpressions;
using ProjectFormeleMethodes.Regular_Expression;
using System;
using System.Collections.Generic;

namespace ProjectFormeleMethodes
{
    public class Program
    {
        private static bool GraphVizEngineTOGGLE = false;

        public static void Main(string[] args)
        {
            Tester tester = new Tester();
            ConsoleBuilderSelecterTest();
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
                    return DFABuilder.BuildDFASampleOne();

                case 3:
                    return DFABuilder.BuildDFASampleOne();

                default:
                    return DFABuilder.BuildDFASampleOne();
            }
        }

        public static void ConsoleExecute()
        {

        }

        public static dynamic ConsoleBuilderSelecterTest()
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
                    Console.WriteLine("3: Eigen maken\n");

                    Console.Write("=>> ");
                    value = -1;
                    int.TryParse(Console.ReadLine(), out value);

                    switch (value)
                    {
                        case 1:
                            return CreateRegExp(value);


                        case 2:
                            return CreateRegExp(value);


                        case 3:
                            Console.WriteLine("Maak eigen expressie");
                            Console.Write(">");
                            string input = Console.ReadLine();
                            return CreateRegExp(value, input);


                        default:
                            return CreateRegExp(-1); // back to default        
                    }

                case 2: // NDFA expressies
                    Console.WriteLine("\nNDFA Expressies: ");
                    Console.WriteLine("Welke optie?");
                    Console.WriteLine("1: Sample 1");
                    Console.WriteLine("2: Sample 2");
                    Console.WriteLine("3: Sample 3");
                    Console.WriteLine("4: Sample 4\n");

                    Console.Write("=>> ");
                    value = -1;
                    int.TryParse(Console.ReadLine(), out value);

                    switch (value)
                    {
                        case 1:
                            return CreateNDFA(value);


                        case 2:
                            return CreateNDFA(value);


                        case 3:
                            return CreateNDFA(value);


                        case 4:
                            return CreateNDFA(value);


                        default:
                            return CreateNDFA(-1); // back to default 

                    }

                case 3: // DFA expressies
                    Console.WriteLine("\nDFA Expressies: ");
                    Console.WriteLine("Welke optie?");
                    Console.WriteLine("1: Sample 1");
                    Console.WriteLine("2: Sample 2");
                    Console.WriteLine("3: Sample 3\n");

                    Console.Write("=>> ");
                    value = -1;
                    int.TryParse(Console.ReadLine(), out value);

                    switch (value)
                    {
                        case 1:
                            return CreateDFA(value);


                        case 2:
                            return CreateDFA(value);


                        case 3:
                            return CreateDFA(value);


                        default:
                            return CreateDFA(-1); // back to default 

                    }

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
