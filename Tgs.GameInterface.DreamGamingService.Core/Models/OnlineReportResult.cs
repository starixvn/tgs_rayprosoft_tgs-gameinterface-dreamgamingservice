namespace Tgs.GameInterface.DreamGamingService.Core.Models;

public record OnlineReportResult
{
	public string PlayerCode { get; init; } = default!;
	public long MemberId { get; init; }
};
