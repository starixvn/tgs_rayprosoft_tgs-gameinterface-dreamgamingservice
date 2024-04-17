namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

public record GameUrlQuery(
	Uri ApiUrl,
	string ApiKey,
	string AgentName,
	string Username,
	string Password,
	bool IsMobile,
	string Lang = "en");
