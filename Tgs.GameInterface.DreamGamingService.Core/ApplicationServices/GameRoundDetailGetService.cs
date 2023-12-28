using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class GameRoundDetailGetService(IGameApiClient client) : IGetService<GameRoundDetailQuery, string>
{
    public ValueTask<string> GetAsync(GameRoundDetailQuery query, CancellationToken cancellationToken = default)
        => client.GetGameResultAsync(query.VideoGameApiUrl, query.AgentName, query.Language, query.Id, cancellationToken);
}
