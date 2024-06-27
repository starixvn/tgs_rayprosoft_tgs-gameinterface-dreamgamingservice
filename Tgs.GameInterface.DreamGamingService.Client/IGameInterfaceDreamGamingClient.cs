using Tgs.GameInterface.DreamGamingService.Protos;

namespace Tgs.GameInterface.DreamGamingService.Client;

public interface IGameInterfaceDreamGamingClient
{
	ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default);

	ValueTask<FundReply> FundInAsync(Uri apiUrl, string apiKey, string agentName, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default);

	ValueTask<FundReply> FundOutAsync(Uri apiUrl, string apiKey, string agentName, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default);

	ValueTask<FundStatusReply> FundStatusAsync(Uri apiUrl, string apiKey, string agentName, string orderId, CancellationToken cancellationToken = default);

	ValueTask PlayerCreateAsync(Uri apiUrl, string apiKey, string agentName, string playerCode, string password, string currencyName, string tableLimit, int winLimit, CancellationToken cancellationToken = default);

	ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string apiKey, string agentName, string playerCode, CancellationToken cancellationToken = default);

	ValueTask PlayerKickAsync(Uri apiUrl, string apiKey, string agentName, long memberId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<PlayerOnline> PlayerOnlineListAsync(Uri apiUrl, string apiKey, string agentName, CancellationToken cancellationToken = default);

	ValueTask<GameUrlReply> GameUrlAsync(Uri apiUrl, string apiKey, string agentName, string playerCode, string password, bool isMobile, string lang, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameHistorySettled> GameHistorySettledAsync(Uri apiUrl, string apiKey, string agentName, CancellationToken cancellationToken = default);

	ValueTask<BaseReply> GameHistoryMarkAsync(Uri apiUrl, string apiKey, string agentName, IEnumerable<string> externalRoundIds, CancellationToken cancellationToken = default);

	ValueTask<GameHistoryDetailReply> GameHistoryDetailAsync(Uri videoGameApiUrl, string agentName, string language, string externalRoundId, CancellationToken cancellationToken = default);
}
