using Tgs.GameInterface.DreamGamingService.Protos;

namespace Tgs.GameInterface.DreamGamingService.Client;

public interface IGameInterfaceDreamGamingClient
{
    ValueTask<FundReply> FundInAsync(
        Uri apiUrl,
        string apiKey,
        string agentName,
        string username,
        string refTransactionCode,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundOutAsync(
        Uri apiUrl,
        string apiKey,
        string agentName,
        string username,
        string refTransactionCode,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<FundStatusReply> FundStatusAsync(
        Uri apiUrl,
        string apiKey,
        string agentName,
        string refTransactionCode,
        CancellationToken cancellationToken = default);

    ValueTask PlayerCreateAsync(
        Uri apiUrl,
        string apiKey,
        string agentName,
        string username,
        string password,
        string currencyName,
        string tableLimit,
        int winLimit,
        CancellationToken cancellationToken = default);

    ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
        Uri apiUrl,
        string apiKey,
        string agentName,
        string username,
        CancellationToken cancellationToken = default);

    ValueTask PlayerKickAsync(
        Uri apiUrl,
        string apiKey,
        string agentName,
        long memberId,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<PlayerOnline> PlayerOnlineAsync(
        Uri apiUrl,
        string apiKey,
        string agentName,
        CancellationToken cancellationToken = default);

    ValueTask<GameUrlReply> GameUrlAsync(
        Uri apiUrl,
        string apiKey,
        string agentName,
        string username,
        string password,
        bool isMobile,
        string lang,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameHistorySettled> GameHistorySettledAsync(
        Uri apiUrl,
        string apiKey,
        string agentName,
        CancellationToken cancellationToken = default);

    ValueTask<BaseReply> GameHistoryMarkAsync(
        Uri apiUrl,
        string apiKey,
        string agentName,
        IEnumerable<string> externalRoundIds,
        CancellationToken cancellationToken = default);

    ValueTask<GameHistoryDetailReply> GameHistoryDetailAsync(
       Uri videoGameApiUrl,
       string agentName,
       string language,
       string id,
       CancellationToken cancellationToken = default);
}
