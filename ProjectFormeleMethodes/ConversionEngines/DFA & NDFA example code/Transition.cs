using System;

/// <summary>
/// The class Automata represents both DFA and NDFA: some NDFA's are also DFA
/// Using the method isDFA we can check this
/// 
/// We use '$' to denote the empty symbol epsilon
/// 
/// @author Paul de Mast
/// @version 1.0
/// </summary>

public class Transition<T> : IComparable<Transition<T>> where T : IComparable
{
	public const char EPSILON = '$';
	private T fromState;
	private char symbol;
	private T toState;

   // this constructor can be used to define loops:
	public Transition(T fromOrTo, char s) : this(fromOrTo, s, fromOrTo)
	{

	}

	public Transition(T from, T to) : this(from, EPSILON, to)
	{

	}

	public Transition(T from, char s, T to)
	{
		this.fromState = from;
		this.symbol = s;
		this.toState = to;
	}

	// overriding equals
	public override bool Equals(object other)
	{
	   if (other == null)
	   {
		  return false;
	   }
	   else if (other is Transition)
	   {
			   return this.fromState.Equals(((Transition)other).fromState) && this.toState.Equals(((Transition)other).toState) && this.symbol == (((Transition)other).symbol);
	   }
	   else
	   {
		   return false;
	   }
	}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public int compareTo(Transition<T> t)
	public int CompareTo(Transition<T> t)
	{
		int fromCmp = fromState.CompareTo(t.fromState);
		int symbolCmp = (new char?(symbol)).compareTo(new char?(t.symbol));
		int toCmp = toState.CompareTo(t.toState);

		return (fromCmp != 0 ? fromCmp : (symbolCmp != 0 ? symbolCmp : toCmp));
	}

	public T FromState
	{
		get
		{
			return fromState;
		}
	}

	public T ToState
	{
		get
		{
			return toState;
		}
	}

	public char Symbol
	{
		get
		{
			return symbol;
		}
	}

	public override string ToString()
	{
	   return "(" + this.FromState + ", " + this.Symbol + ")" + "-->" + this.ToState;
	}
}