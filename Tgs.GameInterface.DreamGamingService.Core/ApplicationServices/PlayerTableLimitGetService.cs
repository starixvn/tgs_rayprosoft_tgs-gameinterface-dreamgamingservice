using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class PlayerTableLimitGetService(
    IGameApiClient client) : IGetService<PlayerTableLimitCommand, bool>
{
    public ValueTask<bool> GetAsync(PlayerTableLimitCommand query, CancellationToken cancellationToken = default)
        => client.UpdateLimitAsync(query.ApiUrl, query.ApiKey, query.AgentName, query.Username, query.TableLimit, cancellationToken);
}
