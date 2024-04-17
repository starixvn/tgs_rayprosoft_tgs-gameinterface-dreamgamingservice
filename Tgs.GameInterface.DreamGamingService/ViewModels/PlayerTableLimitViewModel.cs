namespace Tgs.GameInterface.DreamGamingService.ViewModels;

public record PlayerTableLimitViewModel(
	Uri ApiUrl,
	string ApiKey,
	string AgentName,
	string Username,
	string TableLimit);
