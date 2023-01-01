using Sandbox.Diagnostics;

namespace Party;

public static class Debug
{
	[ConVar.Replicated( "p_debug" )]
	public static int Level { get; set; } = 100;

	public static bool Enabled => Level > 0;
}

public static class LoggerExtension
{
	public static void Debug( this Logger log, object obj )
	{
		if ( !Party.Debug.Enabled ) return;

		log.Info( $"[{(Game.IsClient ? "CL" : "SV")}] {obj}" );
	}

	public static void Debug( this Logger log, object obj, int level )
	{
		if ( !(Party.Debug.Level >= level) ) return;

		log.Info( $"[{(Game.IsClient ? "CL" : "SV")}] {obj}" );
	}
}
