namespace Tgs.GameInterface.DreamGamingService.ViewModels;

public record GameHistorySettledResultViewModel(
	string PlayerCode,
	string ExternalRoundId,
	bool IsClosed,
	bool IsVoid,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset? ExternalClosedOn,
	decimal Bet,
	decimal ValidBet,
	decimal PlayerWinLoss,
	decimal Turnover,
	decimal Settle,
	string AdditionalData,
	string ExternalGameId);
