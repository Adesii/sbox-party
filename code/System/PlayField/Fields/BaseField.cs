using Party.Player;
using Sandbox.ModelEditor;

namespace Party.System.PlayField.Fields;

[Category( "Fields" ), HideInEditor]
[EditorModel( "models/light_arrow.vmdl" )]
public partial class BaseField : Entity
{
	[Net]
	public IList<BaseField> NextFields { get; set; } = new List<BaseField>();
	[Net]
	public IList<BaseField> PrevFields { get; set; } = new List<BaseField>();

	public void DebugDraw()
	{
		DebugOverlay.Text( $"NextFields: {NextFields.Count}", Position, Color.White );
		foreach ( var field in NextFields )
		{
			if ( !field.IsValid() ) continue;
			DebugOverlay.Line( Position, field.Position, Color.White, 0, true );
		}
	}

	protected virtual void FindFields()
	{
		NextFields.Clear();
		foreach ( var field in FieldManager.GetFields() )
		{
			if ( field == this ) continue;
			if ( Rotation.LookAt( field.Position - Position ).Forward.Angle( Rotation.Forward ) < 25f )
			{
				NextFields.Add( field );
				field.RegisterLastField( this );
			}
		}
		if ( NextFields.Count > 1 )
		{
			//remove the furthest fields and only keep the nearest one
			var nearest = NextFields.OrderBy( x => x.Position.Distance( Position ) ).First();
			NextFields.Clear();
			NextFields.Add( nearest );
		}
	}

	public virtual void RegisterLastField( BaseField last )
	{
		PrevFields.Add( last );
	}

	public virtual void OnEnter( PartyPlayer player )
	{
	}

	public virtual void OnLeave( PartyPlayer player )
	{
	}

	public virtual void Init()
	{
		FindFields();
	}

	public virtual BaseField MoveToNext()
	{
		return NextFields.First();
	}

}
