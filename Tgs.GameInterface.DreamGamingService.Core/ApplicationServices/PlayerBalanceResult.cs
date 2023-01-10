namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

public record PlayerBalanceResult(
    bool IsPlayerExist,
    decimal? Balance);
