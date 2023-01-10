using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class GameUrlGetService : IGetService<GameUrlQuery, string?>
{
    private readonly IGameApiClient _client;

    public GameUrlGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<string?> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
        => _client.LoginAsync(
            query.ApiUrl,
            query.ApiKey,
            query.AgentName,
            query.Username,
            query.Password,
            query.IsMobile,
            query.Lang,
            cancellationToken);
}