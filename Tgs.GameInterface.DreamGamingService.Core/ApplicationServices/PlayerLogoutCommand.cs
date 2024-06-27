namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

public record PlayerLogoutCommand(
	Uri ApiUrl,
	string ApiKey,
	string AgentName,
	long MemberId);
