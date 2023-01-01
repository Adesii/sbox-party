namespace Party.StateSystem;

public static class StateMachineExtensions
{
	public static StateMachine GetStateMachine( this IClient cl )
	{
		return cl.GetStateMachine<StateMachine>();
	}

	public static T GetStateMachine<T>( this IClient cl ) where T : StateMachine
	{
		if ( PartyGame.Current is IStateMachine<T> gamemode )
			return gamemode.StateMachine;
		return null;
	}

	public static T GetStateMachine<T>( this Entity ent ) where T : StateMachine
	{
		if ( ent is IStateMachine<T> gamemode )
			return gamemode.StateMachine;
		return null;
	}

	public static T GetStateMachine<T>( this GameManager ent ) where T : StateMachine
	{
		if ( ent is IStateMachine<T> gamemode )
			return gamemode.StateMachine;
		return null;
	}
}
