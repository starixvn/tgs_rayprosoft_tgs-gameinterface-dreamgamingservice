namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

public record PlayerTableLimitCommand(
    Uri ApiUrl,
    string ApiKey,
    string AgentName,
    string Username,
    string TableLimit);
