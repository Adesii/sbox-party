namespace Party.Player;

public class PlayerCloseupCamera : CameraMode
{

	public override void Build()
	{
		if ( PartyGame.Current.CurrentTurnIClient?.Pawn is not PartyPlayer player || player.ControllingPawn is not PartyPawn pawn )
		{
			Log.Info( "No player or pawn" );
			return;
		}

		Camera.Position = Camera.Position.LerpTo( player.AimRay.Position + player.AimRay.Forward * 86 + Vector3.Up * 10 + pawn.Rotation.Right * 20, Time.Delta * 10 );
		Camera.Rotation = Rotation.Lerp( Camera.Rotation, Rotation.LookAt( (player.AimRay.Position + Vector3.Up * -15 + pawn.Rotation.Right * 20) - Camera.Position ), Time.Delta * 10 );
	}
}
