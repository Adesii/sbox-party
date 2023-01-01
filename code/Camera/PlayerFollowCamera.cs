namespace Party.Player;

public class PlayerFollowCamera : CameraMode
{
	public override void Build()
	{
		if ( PartyGame.Current.CurrentTurnIClient?.Pawn is not PartyPlayer player || player.ControllingPawn is not PartyPawn Target )
			return;
		Camera.Position = Camera.Position.LerpTo( Target.Position + Vector3.Up * 500 + -Vector3.Forward * 500, Time.Delta * 10 );
		Camera.Rotation = Rotation.LookAt( -Vector3.Up * 500 + Vector3.Forward * 500 );
	}
}
