namespace Tgs.GameInterface.DreamGamingService.ViewModels;

public record GameHistoryMarkViewModel(
    Uri ApiUrl,
    string ApiKey,
    string AgentName,
    IEnumerable<string> ExternalRoundIds);
