namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

public record FundStatusQuery(
	Uri ApiUrl,
	string ApiKey,
	string AgentName,
	string RefTransactionCode);
