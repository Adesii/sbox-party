using Party.Player;
using Party.System.PlayField;
using Party.System.PlayField.Fields;

namespace Party.StateSystem;

public partial class MoveState : BaseState<TurnStateMachine>
{
	[Net]
	public List<BaseField> Path { get; set; }

	public PartyPlayer Player => (StateMachine.CurrentTurn.Pawn as PartyPlayer);
	public PartyPawn PawnToMove => Player?.ControllingPawn;

	public override void OnEnter()
	{
		Path = FieldManager.Current.CurrentPath.ToList();
		if ( Path.Count > 0 )
		{
			Player.LastField = Player.CurrentField;
			Player.CurrentField.OnLeave( Player );
			Final = Path.Last();
			Player.CurrentField = Final;
		}

		PartyGame.ForceCamera( typeof( PlayerFollowCamera ) );
	}

	public override void OnExit()
	{
		Path.Clear();
	}
	public override void CheckSwitchState()
	{
		if ( Path.Count == 0 )
		{
			Final.OnEnter( Player );
			Player.Client.EndTurn();
		}
		if ( StateMachine.TurnFinished )
		{
			StateMachine.TurnFinished = false;
			StateMachine.SetState( nameof( WaitState ) );
		}
	}
	public BaseField Final;
	public override void OnTick()
	{
		if ( Game.IsClient || Path.Count == 0 || (!PawnToMove?.IsAuthority ?? false) ) return;


		//DebugDraw Path
		for ( int i = 0; i < Path.Count - 1; i++ )
		{
			DebugOverlay.Line( Path[i].Position + Vector3.Up * 64, Path[i + 1].Position + Vector3.Up * 64, Color.Red );
		}

		DebugOverlay.Text( $"Path: {Path.Count}", PawnToMove.Position, Color.Red );
		if ( MovePlayer( PawnToMove, Path[0].Transform ) )
		{
			Path.RemoveAt( 0 );
		}
	}

	private bool MovePlayer( PartyPawn player, Transform field )
	{
		if ( player.IsValid() )
		{
			if ( player.Position.Distance( field.Position ) > 10 )
			{
				player.Position += (field.Position - player.Position).Normal * 200 * Time.Delta;
				player.Rotation = Rotation.LookAt( field.Position.WithZ( 0 ) - player.Position.WithZ( 0 ) );
			}
			else
			{
				return true;
			}
		}
		return false;
	}
}
