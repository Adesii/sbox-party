using Party.StateSystem;

namespace Party;

public static class TurnManagerExtensions
{
	public static bool HasTurn( this IClient cl )
	{
		return PartyGame.Current.CurrentTurnIClient == cl;
	}

	private static TurnStateMachine GetTurnStateMachine( this IClient cl )
	{
		return cl?.GetStateMachine<TurnStateMachine>();
	}

	public static void EndTurn( this IClient cl )
	{
		if ( !cl.HasTurn() )
			return;
		cl.GetTurnStateMachine().EndTurn();
	}
}
