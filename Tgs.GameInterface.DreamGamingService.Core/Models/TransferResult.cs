namespace Tgs.GameInterface.DreamGamingService.Core.Models;

public record TransferResult(
    string RefTransactionCode,
    string TransactionId,
    decimal Balance);