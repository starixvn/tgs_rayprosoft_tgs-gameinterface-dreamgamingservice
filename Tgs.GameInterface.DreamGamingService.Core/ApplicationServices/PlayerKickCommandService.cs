using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class PlayerKickCommandService(
    IGameApiClient client) : ICommandService<PlayerKickCommand>
{
    public async ValueTask ExecuteAsync(PlayerKickCommand command, CancellationToken cancellationToken = default)
    {
        _ = await client
            .OfflineAsync(command.ApiUrl, command.ApiKey, command.AgentName, command.MemberId, cancellationToken)
            .ConfigureAwait(false);

        return;
    }
}
