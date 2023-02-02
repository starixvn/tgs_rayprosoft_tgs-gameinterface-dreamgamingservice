namespace Tgs.GameInterface.DreamGamingService.Core.Models;

public record GetBalanceResult(
    bool IsPlayerExist,
    decimal Balance);
