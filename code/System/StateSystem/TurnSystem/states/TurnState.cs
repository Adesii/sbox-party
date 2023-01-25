using Party.Player;

namespace Party.StateSystem;

[Library]
public partial class TurnState : BaseState<TurnStateMachine>
{
	public static TurnState Current => PartyGame.Current.StateMachine.CurrentState as TurnState;

	public override void OnEnter()
	{
		base.OnEnter();
		if ( Game.IsServer )
		{
			PartyGame.ForceCamera( typeof( PlayerCloseupCamera ) );
		}
	}
}
