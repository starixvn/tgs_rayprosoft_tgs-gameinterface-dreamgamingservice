using System.Runtime.CompilerServices;
using Tgs.GameInterface.DreamGamingService.Protos;

namespace Tgs.GameInterface.DreamGamingService.Client;

internal class GameInterfaceDreamGamingClient(
	GameInterfaceDreamGamingService.GameInterfaceDreamGamingServiceClient client) : IGameInterfaceDreamGamingClient
{
	public async ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default)
	{
		var request = new CheckingServiceRequest
		{
			MachineName = machineName,
		};

		var reply = await client
			.CheckingServiceAsync(
				request: request,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return reply.MachineName;
	}

	public async ValueTask<FundReply> FundInAsync(Uri apiUrl, string apiKey, string agentName, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default)
	{
		var request = new FundRequest
		{
			ApiUrl = apiUrl.ToString(),
			ApiKey = apiKey,
			AgentName = agentName,
			PlayerCode = playerCode,
			OrderId = orderId,
			Amount = amount,
		};

		return await client
			.FundInAsync(
				request: request,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<FundReply> FundOutAsync(Uri apiUrl, string apiKey, string agentName, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default)
	{
		var request = new FundRequest
		{
			ApiUrl = apiUrl.ToString(),
			ApiKey = apiKey,
			AgentName = agentName,
			PlayerCode = playerCode,
			OrderId = orderId,
			Amount = amount,
		};

		return await client
			.FundOutAsync(
				request: request,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<FundStatusReply> FundStatusAsync(Uri apiUrl, string apiKey, string agentName, string orderId, CancellationToken cancellationToken = default)
	{
		var request = new FundStatusRequest
		{
			ApiUrl = apiUrl.ToString(),
			ApiKey = apiKey,
			AgentName = agentName,
			OrderId = orderId,
		};

		return await client
			.FundStatusAsync(
				request: request,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<GameHistoryDetailReply> GameHistoryDetailAsync(Uri videoGameApiUrl, string agentName, string language, string externalRoundId, CancellationToken cancellationToken = default)
	{
		var request = new GameHistoryDetailRequest
		{
			VideoGameApiUrl = videoGameApiUrl.ToString(),
			AgentName = agentName,
			Language = language,
			ExternalRoundId = externalRoundId,
		};

		return await client
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

		return await client
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

		var reply = await client.GameHistorySettledAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var items = reply.Items ?? Enumerable.Empty<GameHistorySettled>();

		if (!items.Any())
		{
			yield break;
		}

		foreach (var item in items)
		{
			yield return item;
		}
	}

	public async ValueTask<GameUrlReply> GameUrlAsync(Uri apiUrl, string apiKey, string agentName, string playerCode, string password, bool isMobile, string lang, CancellationToken cancellationToken = default)
	{
		var request = new GameUrlRequest
		{
			ApiUrl = apiUrl.ToString(),
			ApiKey = apiKey,
			AgentName = agentName,
			PlayerCode = playerCode,
			Password = password,
			IsMobile = isMobile,
			Lang = lang,
		};

		return await client
			.GameUrlAsync(
				request: request,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string apiKey, string agentName, string playerCode, CancellationToken cancellationToken = default)
	{
		var request = new PlayerBalanceRequest
		{
			ApiUrl = apiUrl.ToString(),
			ApiKey = apiKey,
			AgentName = agentName,
			PlayerCode = playerCode,
		};

		return await client
			.PlayerBalanceAsync(
				request: request,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask PlayerCreateAsync(Uri apiUrl, string apiKey, string agentName, string playerCode, string password, string currencyName, string tableLimit, int winLimit, CancellationToken cancellationToken = default)
	{
		var request = new PlayerCreateRequest
		{
			ApiUrl = apiUrl.ToString(),
			ApiKey = apiKey,
			AgentName = agentName,
			PlayerCode = playerCode,
			Password = password,
			CurrencyName = currencyName,
			TableLimit = tableLimit,
			WinLimit = winLimit,
		};

		_ = await client
			.PlayerCreateAsync(
				request: request,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask PlayerLogoutAsync(Uri apiUrl, string apiKey, string agentName, long memberId, CancellationToken cancellationToken = default)
	{
		var request = new PlayerLogoutRequest
		{
			ApiUrl = apiUrl.ToString(),
			ApiKey = apiKey,
			AgentName = agentName,
			MemberId = memberId
		};

		_ = await client.PlayerLogoutAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async IAsyncEnumerable<PlayerOnline> PlayerOnlineListAsync(Uri apiUrl, string apiKey, string agentName, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var request = new PlayerOnlineRequest()
		{
			ApiUrl = apiUrl.ToString(),
			ApiKey = apiKey,
			AgentName = agentName,
		};

		var reply = await client
				.PlayerOnlineAsync(
					request: request,
					cancellationToken: cancellationToken)
				.ConfigureAwait(false);

		var items = reply.Items ?? Enumerable.Empty<PlayerOnline>();

		if (!items.Any())
		{
			yield break;
		}

		foreach (var item in items)
		{
			yield return item;
		}
	}
}
