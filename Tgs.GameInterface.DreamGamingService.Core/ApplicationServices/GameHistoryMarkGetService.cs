using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class GameHistoryMarkGetService(
	IGameApiClient client) : IGetService<GameHistoryMarkQuery, bool>
{
	public ValueTask<bool> GetAsync(GameHistoryMarkQuery query, CancellationToken cancellationToken = default)
		=> client.MarkReportAsync(
			apiUrl: query.ApiUrl,
			apiKey: query.ApiKey,
			agentName: query.AgentName,
			externalRoundIds: query.ExternalRoundIds,
			cancellationToken: cancellationToken);
}
