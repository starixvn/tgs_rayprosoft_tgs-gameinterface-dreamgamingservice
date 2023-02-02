using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class GameHistorySettledQueryService : IQueryService<GameHistorySettledQuery, GameHistorySettledResult>
{
    private readonly IGameApiClient _client;

    public GameHistorySettledQueryService(
        IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameHistorySettledResult> QueryAsync(GameHistorySettledQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var bettingHistoryBets = await _client
                .GetReportAsync(
                    apiUrl: query.ApiUrl,
                    apiKey: query.ApiKey,
                    agentName: query.AgentName,
                    cancellationToken: cancellationToken)
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false);

        if (!bettingHistoryBets.Any())
        {
            yield break;
        }

        foreach (var bettingHistoryBet in bettingHistoryBets)
        {
            var playerWinLoss = bettingHistoryBet.Settle - bettingHistoryBet.Bet;
            if (bettingHistoryBet.IsVoid)
            {
                playerWinLoss = 0;
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
                Turnover: bettingHistoryBet.Bet,
                Settle: bettingHistoryBet.Settle,
                AdditionalData: bettingHistoryBet.AdditionalData,
                ExternalGameId: bettingHistoryBet.ExternalGameId);
        }
    }
}
