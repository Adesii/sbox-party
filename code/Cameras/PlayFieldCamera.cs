namespace Party.Player;

public class PlayFieldCamera : CameraMode
{
	public override void Build()
	{
		base.Build();
		Camera.Position = new Vector3( 0, 0, 100 );
		Camera.Rotation = Rotation.LookAt( Vector3.Down + Vector3.Forward * 0.125f );
	}

}
