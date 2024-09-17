namespace Tgs.GameInterface.DreamGamingService.Core.Models;

public record GetReportResult
{
	public string PlayerCode { get; init; } = default!;
	public string ExternalRoundId { get; init; } = default!;
	public bool IsVoid { get; init; }
	public bool IsClosed { get; init; }
	public DateTimeOffset ExternalCreatedOn { get; init; }
	public DateTimeOffset ExternalClosedOn { get; init; }
	public decimal Bet { get; init; }
	public decimal ValidBet { get; init; }
	public decimal Settle { get; init; }
	public string AdditionalData { get; init; } = default!;
	public string ExternalGameId { get; init; } = default!;
	public string GameType { get; init; } = default!;
	public string GameId { get; init; } = default!;
};
