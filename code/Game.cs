using Party.Player;
using Party.StateSystem;
using Party.System.PlayField;
using Party.System.PlayField.Fields;

namespace Party;

public partial class PartyGame : GameManager, IStateMachine<TurnStateMachine>
{
	public static new PartyGame Current => GameManager.Current as PartyGame;

	public IClient CurrentTurnIClient => StateMachine.CurrentTurn;

	public static BaseState CurrentState => Current.StateMachine.CurrentState;

	[Net]
	public TurnStateMachine StateMachine { get; set; }

	[Net]
	public FieldManager FieldManager { get; internal set; }

	public PartyGame()
	{
	}
	[Event.Entity.PostSpawn]
	public void init()
	{
		StateMachine = new TurnStateMachine();
		FieldManager = new FieldManager();
		FieldManager.InitField();
	}

	public override void ClientSpawn()
	{
		Game.RootPanel?.Delete( true );
		Game.RootPanel = new UI.Hud();
	}

	[ConCmd.Server]
	public static void StartGame()
	{
		Current.StateMachine.OnGamemodeStart();
	}
	[ConCmd.Server]
	public static void EndGame()
	{
		Current.StateMachine.OnGamemodeEnd();
	}

	[ConCmd.Server]
	public static void EndTurn()
	{
		Current.StateMachine.EndTurn();
	}

	[ConCmd.Server]
	public static void RecalculateField()
	{
		FieldManager.InitField();
	}
	[ConCmd.Server]
	public static void KickBots()
	{
		foreach ( var client in Game.Clients )
		{
			if ( client.IsBot )
			{
				client.Kick();
			}
		}
	}

	[ConCmd.Server]
	public static void SetTurnState( string state )
	{
		Current.StateMachine.SetState( state );
	}
	public static void ForceCamera( Type cameraMode )
	{
		if ( !Game.IsServer ) return;

		foreach ( var item in Game.Clients )
		{

			(item.Pawn as PartyPlayer).Camera = TypeLibrary.Create<CameraMode>( cameraMode );
		}
	}

	public override void Simulate( IClient cl )
	{
		base.Simulate( cl );

		StateMachine?.Simulate( cl );

		if ( !Debug.Enabled )
			return;
		foreach ( var field in Entity.All.OfType<BaseField>() )
		{
			field.DebugDraw();
		}
	}

	public override void ClientJoined( IClient client )
	{
		base.ClientJoined( client );

		var pawn = new PartyPlayer();
		client.Pawn = pawn;

		var spawnpoints = Entity.All.OfType<SpawnPoint>();

		var randomSpawnPoint = spawnpoints.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

		if ( randomSpawnPoint != null )
		{
			var tx = randomSpawnPoint.Transform;
			tx.Position += Vector3.Up * 50.0f;
			pawn.Transform = tx;
		}
		pawn.Camera = new PlayerFollowCamera();
	}
}
