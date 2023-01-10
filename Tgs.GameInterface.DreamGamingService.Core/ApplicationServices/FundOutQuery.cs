namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

public record FundOutQuery(
    Uri ApiUrl,
    string ApiKey,
    string AgentName,
    string Username,
    string RefTransactionCode,
    decimal Amount);
