using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class PlayerLogoutCommandService(
	IGameApiClient client) : ICommandService<PlayerLogoutCommand>
{
	public async ValueTask ExecuteAsync(PlayerLogoutCommand command, CancellationToken cancellationToken = default)
	{
		_ = await client
			.OfflineAsync(command.ApiUrl, command.ApiKey, command.AgentName, command.MemberId, cancellationToken)
			.ConfigureAwait(false);

		return;
	}
}
