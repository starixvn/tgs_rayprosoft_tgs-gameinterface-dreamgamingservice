namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

public record FundInQuery(
	Uri ApiUrl,
	string ApiKey,
	string AgentName,
	string PlayerCode,
	string OrderId,
	decimal Amount);
