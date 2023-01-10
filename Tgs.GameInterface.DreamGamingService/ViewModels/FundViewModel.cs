namespace Tgs.GameInterface.DreamGamingService.ViewModels;

public record FundViewModel(
    Uri ApiUrl,
    string ApiKey,
    string AgentName,
    string Username,
    string RefTransactionCode,
    decimal Amount);