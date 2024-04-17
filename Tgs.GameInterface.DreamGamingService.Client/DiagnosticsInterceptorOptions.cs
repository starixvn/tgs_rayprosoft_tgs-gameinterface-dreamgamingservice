using System.Diagnostics;

namespace Tgs.GameInterface.DreamGamingService.Client;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource($"Tgs.GameInterface.DreamGamingService.Client.DiagnosticsInterceptor");
}
