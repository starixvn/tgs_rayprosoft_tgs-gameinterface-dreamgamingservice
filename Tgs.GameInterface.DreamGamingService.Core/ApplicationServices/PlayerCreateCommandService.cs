using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class PlayerCreateCommandService(
    IGameApiClient client) : ICommandService<PlayerCreateCommand>
{
    public ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
        => client.SignUpAsync(
            apiUrl: command.ApiUrl,
            apiKey: command.ApiKey,
            agentName: command.AgentName,
            username: command.Username,
            password: command.Password,
            currencyName: command.CurrencyName,
            tableLimit: command.TableLimit,
            winLimit: command.WinLimit,
            cancellationToken);
}
