namespace Party.StateSystem;

[Library]
public partial class WaitState : PredictedBaseState<TurnStateMachine>
{
	[Net, Predicted]
	public TimeSince CreationTime { get; set; }

	public override void OnEnter()
	{
		CreationTime = 0;
	}

	public override void CheckSwitchState()
	{
		if ( CreationTime > 0.2 )
		{
			StateMachine.TurnIndex += 1 % StateMachine.TurnOrder.Count;
			StateMachine.TurnFinished = false;
			StateMachine.SetState( nameof( TurnState ) );
		}
	}

	public override void OnTick()
	{
		if ( !Debug.Enabled )
			return;
		DebugOverlay.ScreenText( $"{(Game.IsClient ? "[CL]" : "[SV]")}WaitState: {CreationTime}", (Game.IsClient ? 0 : 1) );
	}
}

