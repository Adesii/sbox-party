using Party.StateSystem;
using Party.System.PlayField;
using Party.System.PlayField.Fields;

namespace Party.Player;

public partial class PartyPlayer : Entity
{

	public BaseField LastField { get; set; }
	public BaseField CurrentField { get; set; }

	[Net]
	public PartyPawn ControllingPawn { get; set; }

	public override void Spawn()
	{
		base.Spawn();
		Transmit = TransmitType.Always;

		CurrentField = FieldManager.GetField<StartField>();
		ControllingPawn = new()
		{
			Transform = CurrentField.Transform,
			Parent = this
		};
		Components.Create<PlayerFollowCamera>();
	}

	public override void OnKilled()
	{
		base.OnKilled();
		ControllingPawn?.Delete();
	}

	public override void Simulate( IClient cl )
	{
		base.Simulate( cl );
		if ( !cl.IsAuthority ) return;
		if ( ControllingPawn.IsValid() )
		{
			ControllingPawn.DressUp( cl );
		}
		if ( Game.IsServer && cl.HasTurn() && PartyGame.CurrentState.GetType() == typeof( TurnState ) )
		{
			FieldManager.Move( this, Game.Random.Int( 1, 4 ) );
		}
	}

	public override void FrameSimulate( IClient cl )
	{
		base.FrameSimulate( cl );
		HandleCamera();
	}

	private void HandleCamera()
	{
		var camera = Components.Get<CameraMode>();
		//camera ??= Components.Create<PlayFieldCamera>();
		camera?.Build();
	}
}
