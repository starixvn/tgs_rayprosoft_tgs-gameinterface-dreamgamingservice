namespace Tgs.GameInterface.DreamGamingService.ViewModels;

public record PlayerCreateViewModel(
    Uri ApiUrl,
    string ApiKey,
    string AgentName,
    string Username,
    string Password,
    string CurrencyName,
    string TableLimit,
    int WinLimit);