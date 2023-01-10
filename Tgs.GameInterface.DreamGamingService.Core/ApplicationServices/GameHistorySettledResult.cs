namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

public record GameHistorySettledResult(
    string PlayerCode,
    string ExternalRoundId,
    bool IsClosed,
    DateTimeOffset ExternalCreatedOn,
    DateTimeOffset? ExternalClosedOn,
    decimal Bet,
    decimal? ValidBet,
    decimal PlayerWinLoss,
    decimal Turnover,
    decimal? Settle,
    string AdditionalData,
    string ExternalGameId);