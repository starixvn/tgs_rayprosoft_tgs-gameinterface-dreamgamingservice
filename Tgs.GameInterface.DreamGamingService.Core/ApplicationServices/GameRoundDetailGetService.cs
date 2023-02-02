using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class GameRoundDetailGetService : IGetService<GameRoundDetailQuery, string>
{
    private readonly IGameApiClient _client;

    public GameRoundDetailGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<string> GetAsync(GameRoundDetailQuery query, CancellationToken cancellationToken = default)
        => _client.GetGameResultAsync(query.VideoGameApiUrl, query.AgentName, query.Language, query.Id, cancellationToken);
}
