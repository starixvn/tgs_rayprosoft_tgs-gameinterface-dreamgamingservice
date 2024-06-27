namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

public record GameUrlQuery(
	Uri ApiUrl,
	string ApiKey,
	string AgentName,
	string PlayerCode,
	string Password,
	bool IsMobile,
	string Lang = "en");
