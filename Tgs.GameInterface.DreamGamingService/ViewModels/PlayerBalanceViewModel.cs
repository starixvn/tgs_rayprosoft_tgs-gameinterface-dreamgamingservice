namespace Tgs.GameInterface.DreamGamingService.ViewModels;

public record PlayerBalanceViewModel(
	Uri ApiUrl,
	string ApiKey,
	string AgentName,
	string Username);
