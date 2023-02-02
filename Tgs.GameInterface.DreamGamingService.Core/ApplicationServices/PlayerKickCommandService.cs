using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class PlayerKickCommandService : ICommandService<PlayerKickCommand>
{
    private readonly IGameApiClient _client;

    public PlayerKickCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask ExecuteAsync(PlayerKickCommand command, CancellationToken cancellationToken = default)
    {
        _ = await _client
            .OfflineAsync(command.ApiUrl, command.ApiKey, command.AgentName, command.MemberId, cancellationToken)
            .ConfigureAwait(false);

        return;
    }
}
