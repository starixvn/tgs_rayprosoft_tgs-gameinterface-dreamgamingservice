using System.Runtime.CompilerServices;
using Tgs.GameInterface.DreamGamingService.Protos;

namespace Tgs.GameInterface.DreamGamingService.Client;

internal class GameInterfaceDreamGamingClient : IGameInterfaceDreamGamingClient
{
    private readonly GameInterfaceDreamGamingService.GameInterfaceDreamGamingServiceClient _client;

    public GameInterfaceDreamGamingClient(GameInterfaceDreamGamingService.GameInterfaceDreamGamingServiceClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default)
    {
        var request = new CheckingServiceRequest
        {
            MachineName = machineName,
        };

        var reply = await _client
            .CheckingServiceAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.MachineName;
    }

    public async ValueTask<FundReply> FundInAsync(Uri apiUrl, string apiKey, string agentName, string username, string refTransactionCode, decimal amount, CancellationToken cancellationToken = default)
    {
        var request = new FundRequest
        {
            ApiUrl = apiUrl.ToString(),
            ApiKey = apiKey,
            AgentName = agentName,
            Username = username,
            RefTransactionCode = refTransactionCode,
            Amount = amount,
        };

        return await _client
            .FundInAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<FundReply> FundOutAsync(Uri apiUrl, string apiKey, string agentName, string username, string refTransactionCode, decimal amount, CancellationToken cancellationToken = default)
    {
        var request = new FundRequest
        {
            ApiUrl = apiUrl.ToString(),
            ApiKey = apiKey,
            AgentName = agentName,
            Username = username,
            RefTransactionCode = refTransactionCode,
            Amount = amount,
        };

        return await _client
            .FundOutAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<FundStatusReply> FundStatusAsync(Uri apiUrl, string apiKey, string agentName, string refTransactionCode, CancellationToken cancellationToken = default)
    {
        var request = new FundStatusRequest
        {
            ApiUrl = apiUrl.ToString(),
            ApiKey = apiKey,
            AgentName = agentName,
            RefTransactionCode = refTransactionCode,
        };

        return await _client
            .FundStatusAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<GameHistoryDetailReply> GameHistoryDetailAsync(Uri videoGameApiUrl, string agentName, string language, string id, CancellationToken cancellationToken = default)
    {
        var request = new GameHistoryDetailRequest
        {
            VideoGameApiUrl = videoGameApiUrl.ToString(),
            AgentName = agentName,
            Language = language,
            Id = id,
        };

        return await _client
            .GameHistoryDetailAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<BaseReply> GameHistoryMarkAsync(Uri apiUrl, string apiKey, string agentName, IEnumerable<string> externalRoundIds, CancellationToken cancellationToken = default)
    {
        var request = new GameHistoryMarkRequest
        {
            ApiUrl = apiUrl.ToString(),
            ApiKey = apiKey,
            AgentName = agentName,
        };

        foreach (var item in externalRoundIds)
        {
            request.ExternalRoundIds.Add(item);
        }

        return await _client
            .GameHistoryMarkAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async IAsyncEnumerable<GameHistorySettled> GameHistorySettledAsync(Uri apiUrl, string apiKey, string agentName, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new GameHistorySettledRequest()
        {
            ApiUrl = apiUrl.ToString(),
            ApiKey = apiKey,
            AgentName = agentName,
        };

        var reply = await _client
                .GameHistorySettledAsync(
                    request: request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

        if (!reply.Items.Any())
        {
            yield break;
        }

        foreach (var item in reply.Items)
        {
            yield return item;
        }
    }

    public async ValueTask<GameUrlReply> GameUrlAsync(Uri apiUrl, string apiKey, string agentName, string username, string password, bool isMobile, string lang, CancellationToken cancellationToken = default)
    {
        var request = new GameUrlRequest
        {
            ApiUrl = apiUrl.ToString(),
            ApiKey = apiKey,
            AgentName = agentName,
            Username = username,
            Password = password,
            IsMobile = isMobile,
            Lang = lang,
        };

        return await _client
            .GameUrlAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string apiKey, string agentName, string username, CancellationToken cancellationToken = default)
    {
        var request = new PlayerBalanceRequest
        {
            ApiUrl = apiUrl.ToString(),
            ApiKey = apiKey,
            AgentName = agentName,
            Username = username,
        };

        return await _client
            .PlayerBalanceAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask PlayerCreateAsync(Uri apiUrl, string apiKey, string agentName, string username, string password, string currencyName, string tableLimit, int winLimit, CancellationToken cancellationToken = default)
    {
        var request = new PlayerCreateRequest
        {
            ApiUrl = apiUrl.ToString(),
            ApiKey = apiKey,
            AgentName = agentName,
            Username = username,
            Password = password,
            CurrencyName = currencyName,
            TableLimit = tableLimit,
            WinLimit = winLimit,
        };

        _ = await _client
            .PlayerCreateAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask PlayerKickAsync(Uri apiUrl, string apiKey, string agentName, long memberId, CancellationToken cancellationToken = default)
    {
        var request = new PlayerKickRequest
        {
            ApiUrl = apiUrl.ToString(),
            ApiKey = apiKey,
            AgentName = agentName,
            MemberId = memberId
        };

        _ = await _client
            .PlayerKickAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async IAsyncEnumerable<PlayerOnline> PlayerOnlineAsync(Uri apiUrl, string apiKey, string agentName, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new PlayerOnlineRequest()
        {
            ApiUrl = apiUrl.ToString(),
            ApiKey = apiKey,
            AgentName = agentName,
        };

        var reply = await _client
                .PlayerOnlineAsync(
                    request: request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

        if (!reply.Items.Any())
        {
            yield break;
        }

        foreach (var item in reply.Items)
        {
            yield return item;
        }
    }
}
