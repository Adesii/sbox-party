using Party.StateSystem;

namespace Party;

public partial class TurnStateMachine : StateMachine
{

	[Net]
	public IList<IClient> TurnOrder { get; set; }

	[Net, Predicted]
	public bool TurnFinished { get; set; } = false;
	public IClient CurrentTurn
	{
		get
		{
			if ( TurnOrder == null || TurnOrder.Count == 0 )
				return null;
			if ( TurnIndex >= TurnOrder.Count )
			{
				TurnIndex = 0;
			}
			if ( !TurnOrder[TurnIndex].IsValid() )
			{
				TurnOrder.RemoveAt( TurnIndex );
				return CurrentTurn;
			}
			return TurnOrder[TurnIndex];
		}
	}

	[Net, Predicted] public int TurnIndex { get; set; }

	public override void Spawn()
	{
		base.Spawn();
		PreSpawnEntities();
	}

	public override void OnGamemodeStart()
	{
		base.OnGamemodeStart();

		TurnOrder.Clear();
		foreach ( var item in Game.Clients )
		{
			TurnOrder.Add( item );
		}
		TurnIndex = 0;
		SetState( nameof( WaitState ) );
	}

	public override void Tick()
	{
		base.Tick();
		if ( !Debug.Enabled )
			return;
		DebugOverlay.ScreenText( $"TurnStateMachine: {CurrentTurn?.Name}, State: {CurrentState}", 15 );
	}

	public virtual void EndTurn()
	{
		TurnFinished = true;
	}

}
