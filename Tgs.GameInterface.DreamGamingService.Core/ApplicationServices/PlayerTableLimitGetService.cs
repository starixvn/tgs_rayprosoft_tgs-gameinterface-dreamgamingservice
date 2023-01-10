using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices
{
    internal class PlayerTableLimitGetService : IGetService<PlayerTableLimitCommand, bool>
    {
        private readonly IGameApiClient _client;

        public PlayerTableLimitGetService(IGameApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public ValueTask<bool> GetAsync(PlayerTableLimitCommand query, CancellationToken cancellationToken = default)
            => _client.UpdateLimitAsync(query.ApiUrl, query.ApiKey, query.AgentName, query.Username, query.TableLimit, cancellationToken);
    }
}
