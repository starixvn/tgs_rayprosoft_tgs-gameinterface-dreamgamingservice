using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;

internal class PlayerCreateCommandService : ICommandService<PlayerCreateCommand>
{
    private readonly IGameApiClient _client;

    public PlayerCreateCommandService(
        IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
        => _client.SignUpAsync(
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