using Party.Player;
using Party.System.PlayField;

namespace Party.StateSystem;

public class DiceState : BaseState<TurnStateMachine>
{
	public override void OnEnter()
	{
		if ( Game.IsServer )
		{
			PartyGame.ForceCamera( typeof( PlayerDiceCamera ) );
		}
	}

	public override void Simulate( IClient cl )
	{
		if ( Game.IsClient && Input.Pressed( InputButton.PrimaryAttack ) )
		{
			FieldManager.MoveOnServer( Game.Random.Int( 1, 5 ) );
		}
	}
}
