using System.Runtime.CompilerServices;
using Tgs.GameInterface.DreamGamingService.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class PlayerOnlineQueryService(
    IGameApiClient client) : IQueryService<PlayerOnlineQuery, PlayerOnlineResult>
{
    public async IAsyncEnumerable<PlayerOnlineResult> QueryAsync(PlayerOnlineQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var onlineResults = await client.OnlineReportAsync(
            apiUrl: query.ApiUrl,
            apiKey: query.ApiKey,
            agentName: query.AgentName,
            cancellationToken: cancellationToken)
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<OnlineReportResult>();

        if (!onlineResults.Any())
        {
            yield break;
        }

        foreach (var onlineResult in onlineResults)
        {
            yield return new PlayerOnlineResult(onlineResult.PlayerCode, onlineResult.MemberId);
        }
    }
}
