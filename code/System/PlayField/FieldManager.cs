using System.Collections.Generic;
using Party.Player;
using Party.StateSystem;
using Party.System.PlayField.Fields;

namespace Party.System.PlayField;

public partial class FieldManager : Entity
{
	public static FieldManager Current => PartyGame.Current.FieldManager;
	[Net]
	private IList<BaseField> PlayField { get; set; } = new List<BaseField>();

	[Net]
	public IList<BaseField> CurrentPath { get; set; } = new List<BaseField>();

	public static IEnumerable<BaseField> GetFields()
	{
		return Current.PlayField;
	}


	public static void AddField( BaseField field )
	{
		Current.PlayField.Add( field );
	}

	public static void RemoveField( BaseField field )
	{
		Current.PlayField.Remove( field );
	}

	public static void InitField()
	{
		Log.Info( Current );
		Current.PlayField.Clear();

		Current.PlayField = Entity.All.OfType<BaseField>().ToList();
		foreach ( var item in Current.PlayField )
		{
			item.Init();
		}

	}

	public static BaseField GetField<T>()
	{
		return Current.PlayField.FirstOrDefault( x => x is T );
	}

	public static void Move( PartyPlayer partyPlayer, int Amount )
	{
		Current.CurrentPath.Clear();
		List<BaseField> Fields = new();
		BaseField currentField = partyPlayer.CurrentField;
		for ( int i = 0; i <= Amount; i++ )
		{
			Fields.Add( currentField );
			currentField = currentField.MoveToNext();
		}
		Current.CurrentPath = Fields;

		PartyGame.Current.StateMachine.SetState<MoveState>();
	}
}
