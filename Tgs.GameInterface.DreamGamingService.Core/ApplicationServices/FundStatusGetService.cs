using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class FundStatusGetService(IGameApiClient client) : IGetService<FundStatusQuery, bool>
{
	public ValueTask<bool> GetAsync(FundStatusQuery command, CancellationToken cancellationToken = default)
		=> client.CheckTransferAsync(
			apiUrl: command.ApiUrl,
			apiKey: command.ApiKey,
			agentName: command.AgentName,
			refTransactionCode: command.OrderId,
			cancellationToken: cancellationToken);
}
