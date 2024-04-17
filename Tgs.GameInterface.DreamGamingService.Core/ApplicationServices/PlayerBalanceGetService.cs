using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class PlayerBalanceGetService(
	IGameApiClient client) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
	public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client.GetBalanceAsync(
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
