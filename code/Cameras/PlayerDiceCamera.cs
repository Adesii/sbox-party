namespace Party.Player;

public class PlayerDiceCamera : CameraMode
{
	public override void Build()
	{
		if ( PartyGame.Current.CurrentTurnIClient?.Pawn is not PartyPlayer player || player.ControllingPawn is not PartyPawn pawn )
		{
			Log.Info( "No player or pawn" );
			return;
		}

		Camera.Position = Camera.Position.LerpTo( player.AimRay.Position + player.AimRay.Forward * 128 + Vector3.Up * 25, Time.Delta * 10 );
		Camera.Rotation = Rotation.Lerp( Camera.Rotation, Rotation.LookAt( (player.AimRay.Position + Vector3.Up * 25) - Camera.Position ), Time.Delta * 10 );
	}
}
