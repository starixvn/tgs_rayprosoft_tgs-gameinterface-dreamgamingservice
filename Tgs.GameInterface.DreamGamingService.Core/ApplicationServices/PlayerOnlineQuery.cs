namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

public record PlayerOnlineQuery(
	Uri ApiUrl,
	string ApiKey,
	string AgentName);
