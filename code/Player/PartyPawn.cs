namespace Party.Player;

public class PartyPawn : AnimatedEntity
{
	ClothingContainer ClothingContainer { get; set; }
	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/citizen/citizen.vmdl" );
		SetupPhysicsFromModel( PhysicsMotionType.Keyframed );
	}

	public void DressUp( IClient client )
	{
		if ( ClothingContainer == null )
		{
			ClothingContainer = new();
			ClothingContainer.LoadFromClient( client );
			ClothingContainer.DressEntity( this, false );
			Owner = (Entity)client;
		}
	}
}
