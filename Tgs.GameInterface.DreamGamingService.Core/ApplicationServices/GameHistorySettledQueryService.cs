using System.Runtime.CompilerServices;
using Tgs.GameInterface.DreamGamingService.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class GameHistorySettledQueryService(
	IGameApiClient client) : IQueryService<GameHistorySettledQuery, GameHistorySettledResult>
{
	public async IAsyncEnumerable<GameHistorySettledResult> QueryAsync(GameHistorySettledQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var bettingHistoryBets = await client.GetReportAsync(
			apiUrl: query.ApiUrl,
			apiKey: query.ApiKey,
			agentName: query.AgentName,
			cancellationToken: cancellationToken)
			.ToArrayAsync(cancellationToken)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GetReportResult>();

		if (!bettingHistoryBets.Any())
		{
			yield break;
		}

		foreach (var bettingHistoryBet in bettingHistoryBets)
		{
			var playerWinLoss = 0.0m;
			if (bettingHistoryBet.IsClosed && !bettingHistoryBet.IsVoid)
			{
				playerWinLoss = bettingHistoryBet.Settle - bettingHistoryBet.Bet;
			}

			yield return new GameHistorySettledResult(
				PlayerCode: bettingHistoryBet.PlayerCode,
				ExternalRoundId: bettingHistoryBet.ExternalRoundId,
				IsClosed: bettingHistoryBet.IsClosed,
				IsVoid: bettingHistoryBet.IsVoid,
				ExternalCreatedOn: bettingHistoryBet.ExternalCreatedOn,
				ExternalClosedOn: bettingHistoryBet.ExternalClosedOn,
				Bet: bettingHistoryBet.Bet,
				ValidBet: bettingHistoryBet.ValidBet,
				PlayerWinLoss: playerWinLoss,
				Turnover: bettingHistoryBet.ValidBet,
				Settle: bettingHistoryBet.Settle,
				AdditionalData: bettingHistoryBet.AdditionalData,
				ExternalGameId: bettingHistoryBet.ExternalGameId);
		}
	}
}
