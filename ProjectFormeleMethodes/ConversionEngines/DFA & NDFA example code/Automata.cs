using System;
using System.Collections.Generic;

/// <summary>
/// The class Automata represents both DFA and NDFA: some NDFA's are also DFA
/// Using the method isDFA we can check this
/// 
/// We use '$' to denote the empty symbol epsilon
/// 
/// @author Paul de Mast
/// @version 1.0
/// 
/// </summary>

public class Automata<T> where T : IComparable
{
	// Or use a Map structure
	private ISet<Transition <T>> transitions;
	private SortedSet<T> states;
	private SortedSet<T> startStates;
	private SortedSet<T> finalStates;
	private SortedSet<char> symbols;

	public Automata() : this(new SortedSet<char>())
	{

	}

	public Automata(char?[] s) : this(new SortedSet<char>(Arrays.asList(s)))
	{

	}

	public Automata(SortedSet<char> symbols)
	{
		transitions = new SortedSet<Transition<T>>();
		states = new SortedSet<T>();
		startStates = new SortedSet<T>();
		finalStates = new SortedSet<T>();
		this.setAlphabet(symbols);
	}

	public virtual char?[] Alphabet
	{
		set
		{
			this.setAlphabet(new SortedSet<char>(Arrays.asList(value)));
		}
		get
		{
		   return symbols;
		}
	}

	public virtual SortedSet<char> Alphabet
	{
		set
		{
		   this.symbols = value;
		}
	}

	public virtual void addTransition(Transition<T> t)
	{
		transitions.Add(t);
		states.Add(t.FromState);
		states.Add(t.ToState);
	}

	public virtual void defineAsStartState(T t)
	{
		// if already in states no problem because a Set will remove duplicates.
		states.Add(t);
		startStates.Add(t);
	}

	public virtual void defineAsFinalState(T t)
	{
		// if already in states no problem because a Set will remove duplicates.
		states.Add(t);
		finalStates.Add(t);
	}

	public virtual void printTransitions()
	{
		foreach (Transition<T> t in transitions)
		{
			Console.WriteLine(t);
		}
	}

	public virtual bool DFA
	{
		get
		{
			bool isDFA = true;
    
			foreach (T from in states)
			{
				foreach (char symbol in symbols)
				{
					isDFA = isDFA && getToStates(from, symbol).size() == 1;
				}
			}
    
			return isDFA;
		}
	}
}