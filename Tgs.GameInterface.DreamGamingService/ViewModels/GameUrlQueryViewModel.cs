namespace Tgs.GameInterface.DreamGamingService.ViewModels;

public record GameUrlQueryViewModel(
	Uri ApiUrl,
	string ApiKey,
	string AgentName,
	string Username,
	string Password,
	bool IsMobile,
	string Lang);
