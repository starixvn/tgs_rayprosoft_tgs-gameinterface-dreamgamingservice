namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

public record FundResult(
    string RefTransactionCode,
    string TransactionId,
    decimal Balance);
