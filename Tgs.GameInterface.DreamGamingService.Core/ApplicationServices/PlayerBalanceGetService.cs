using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class PlayerBalanceGetService : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    private readonly IGameApiClient _client;

    public PlayerBalanceGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client.GetBalanceAsync(
            apiUrl: query.ApiUrl,
            apiKey: query.ApiKey,
            agentName: query.AgentName,
            username: query.Username,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return new PlayerBalanceResult(
             IsPlayerExist: result.IsPlayerExist,
             Balance: result.Balance);
    }
}
