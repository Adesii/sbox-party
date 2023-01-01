using Party.Player;

namespace Party.System.PlayField.Fields;

[HammerEntity]
public class LogField : BaseField
{
	public override void OnEnter( PartyPlayer player )
	{
		base.OnEnter( player );
		Log.Info( "Entered " + this );
	}

	public override void OnLeave( PartyPlayer player )
	{
		base.OnLeave( player );
		Log.Info( "Left " + this );
	}
}
