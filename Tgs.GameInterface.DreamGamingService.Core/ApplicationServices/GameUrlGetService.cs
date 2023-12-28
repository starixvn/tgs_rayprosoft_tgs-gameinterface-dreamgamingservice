using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class GameUrlGetService(IGameApiClient client) : IGetService<GameUrlQuery, string?>
{
    public ValueTask<string?> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
        => client.LoginAsync(
            query.ApiUrl,
            query.ApiKey,
            query.AgentName,
            query.Username,
            query.Password,
            query.IsMobile,
            query.Lang,
            cancellationToken);
}
