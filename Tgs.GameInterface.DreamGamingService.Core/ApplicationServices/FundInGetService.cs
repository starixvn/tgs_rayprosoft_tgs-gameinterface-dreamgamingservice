using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class FundInGetService(IGameApiClient client) : IGetService<FundInQuery, FundResult>
{
    public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
    {
        var amount = query.Amount > 0 ? -query.Amount : query.Amount;
        var result = await client.TransferAsync(
            apiUrl: query.ApiUrl,
            apiKey: query.ApiKey,
            agentName: query.AgentName,
            username: query.Username,
            refTransactionCode: query.RefTransactionCode,
            amount: amount,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return new FundResult(
            RefTransactionCode: result.RefTransactionCode,
            TransactionId: result.TransactionId,
            Balance: result.Balance);
    }
}
