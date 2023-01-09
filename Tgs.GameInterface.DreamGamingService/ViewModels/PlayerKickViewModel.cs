namespace Tgs.GameInterface.DreamGamingService.ViewModels;

public record PlayerKickViewModel(
    Uri ApiUrl,
    string ApiKey,
    string AgentName,
    long MemberId);