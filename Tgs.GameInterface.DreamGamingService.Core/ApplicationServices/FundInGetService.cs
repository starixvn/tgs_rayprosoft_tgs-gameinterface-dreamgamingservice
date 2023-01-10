using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class FundInGetService : IGetService<FundInQuery, FundResult>
{
    private readonly IGameApiClient _client;

    public FundInGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client.TransferAsync(
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