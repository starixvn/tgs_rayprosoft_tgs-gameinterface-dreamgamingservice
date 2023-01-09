using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class GameHistoryMarkGetService : IGetService<GameHistoryMarkQuery, bool>
{
    private readonly IGameApiClient _client;

    public GameHistoryMarkGetService(
        IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<bool> GetAsync(GameHistoryMarkQuery query, CancellationToken cancellationToken = default)
        => _client.MarkReportAsync(
            apiUrl: query.ApiUrl,
            apiKey: query.ApiKey,
            agentName: query.AgentName,
            externalRoundIds: query.ExternalRoundIds,
            cancellationToken: cancellationToken);
}