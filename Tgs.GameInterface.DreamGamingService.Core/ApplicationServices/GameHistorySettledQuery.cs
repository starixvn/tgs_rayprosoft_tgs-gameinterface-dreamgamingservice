namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

public record GameHistorySettledQuery(
    Uri ApiUrl,
    string ApiKey,
    string AgentName);
