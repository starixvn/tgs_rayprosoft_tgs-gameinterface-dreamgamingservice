namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

public record PlayerBalanceQuery(
    Uri ApiUrl,
    string ApiKey,
    string AgentName,
    string Username);