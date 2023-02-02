using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class PlayerOnlineQueryService : IQueryService<PlayerOnlineQuery, PlayerOnlineResult>
{
    private readonly IGameApiClient _client;

    public PlayerOnlineQueryService(
        IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<PlayerOnlineResult> QueryAsync(PlayerOnlineQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var onlineResults = await _client
                .OnlineReportAsync(
                    apiUrl: query.ApiUrl,
                    apiKey: query.ApiKey,
                    agentName: query.AgentName,
                    cancellationToken: cancellationToken)
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false);

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
