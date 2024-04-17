using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class FundOutGetService(IGameApiClient client) : IGetService<FundOutQuery, FundResult>
{
	public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client.TransferAsync(
			apiUrl: query.ApiUrl,
			apiKey: query.ApiKey,
			agentName: query.AgentName,
			username: query.Username,
			refTransactionCode: query.RefTransactionCode,
			amount: query.Amount,
			cancellationToken: cancellationToken).ConfigureAwait(false);

		return new FundResult(
			RefTransactionCode: result.RefTransactionCode,
			TransactionId: result.TransactionId,
			Balance: result.Balance);
	}
}
