using Party.Player;

namespace Party.System.PlayField.Fields;

[HammerEntity]
public class SplitField : BaseField
{
	[Property]
	public TagList NextFieldTags { get; set; } = new();

	protected override void FindFields()
	{
		NextFields.Clear();
		foreach ( var field in Entity.All.OfType<BaseField>() )
		{
			if ( field.Tags.HasAny( NextFieldTags ) )
			{
				NextFields.Add( field );
			}
		}

	}

	public override BaseField MoveToNext()
	{
		return NextFields[Game.Random.Int( 0, NextFields.Count - 1 )];
	}
}
