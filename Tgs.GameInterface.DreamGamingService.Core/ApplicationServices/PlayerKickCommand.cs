namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

public record PlayerKickCommand(
    Uri ApiUrl,
    string ApiKey,
    string AgentName,
    long MemberId);
