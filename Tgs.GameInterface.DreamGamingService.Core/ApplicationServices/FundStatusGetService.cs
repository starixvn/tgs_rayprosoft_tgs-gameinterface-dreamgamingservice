using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class FundStatusGetService : IGetService<FundStatusQuery, bool>
{
    private readonly IGameApiClient _client;

    public FundStatusGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<bool> GetAsync(FundStatusQuery command, CancellationToken cancellationToken = default)
        => _client.CheckTransferAsync(
            apiUrl: command.ApiUrl,
            apiKey: command.ApiKey,
            agentName: command.AgentName,
            refTransactionCode: command.RefTransactionCode,
            cancellationToken: cancellationToken);
}