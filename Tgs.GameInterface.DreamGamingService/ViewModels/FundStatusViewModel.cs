namespace Tgs.GameInterface.DreamGamingService.ViewModels;

public record FundStatusViewModel(
    Uri ApiUrl,
    string ApiKey,
    string AgentName,
    string RefTransactionCode);
