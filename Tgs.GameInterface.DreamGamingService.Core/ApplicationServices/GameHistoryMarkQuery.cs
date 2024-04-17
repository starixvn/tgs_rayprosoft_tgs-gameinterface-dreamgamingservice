namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

public record GameHistoryMarkQuery(
	Uri ApiUrl,
	string ApiKey,
	string AgentName,
	IEnumerable<string> ExternalRoundIds);
