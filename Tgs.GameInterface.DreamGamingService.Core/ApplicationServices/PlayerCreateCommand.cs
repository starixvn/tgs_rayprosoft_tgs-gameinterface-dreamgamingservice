namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

public record PlayerCreateCommand(
    Uri ApiUrl,
    string ApiKey,
    string AgentName,
    string Username,
    string Password,
    string CurrencyName,
    string TableLimit = "A",
    int WinLimit = 0);
