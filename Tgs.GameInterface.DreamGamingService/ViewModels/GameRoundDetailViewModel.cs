namespace Tgs.GameInterface.DreamGamingService.ViewModels;

public record GameRoundDetailViewModel(
	Uri VideoGameApiUrl,
	string AgentName,
	string Language,
	string Id);
