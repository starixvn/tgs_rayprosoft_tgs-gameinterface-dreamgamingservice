namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

public record GameRoundDetailQuery(
    Uri VideoGameApiUrl,
    string AgentName,
    string Language,
    string Id);