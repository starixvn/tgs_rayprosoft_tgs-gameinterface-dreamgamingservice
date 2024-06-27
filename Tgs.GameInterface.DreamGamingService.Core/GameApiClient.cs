using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Polly;
using Tgs.GameInterface.DreamGamingService.Core.Exceptions;
using Tgs.GameInterface.DreamGamingService.Core.Extensions;
using Tgs.GameInterface.DreamGamingService.Core.Models;

namespace Tgs.GameInterface.DreamGamingService.Core;

internal class GameApiClient(
	HttpClient client,
	ILogger<GameApiClient> logger) : IGameApiClient
{
	public async ValueTask SignUpAsync(
		Uri apiUrl,
		string apiKey,
		string agentName,
		string username,
		string password,
		string currencyName,
		string tableLimit = "A",
		int winLimit = 0,
		CancellationToken cancellationToken = default)
	{
		var random = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		var token = GetHashMD5($"{agentName}{apiKey}{random}");

		var request = new
		{
			token,
			random,
			data = tableLimit,
			member = new
			{
				username,
				password = GetHashMD5(password),
				currencyName,
				winLimit
			}
		};

		var url = new Uri(apiUrl, $"/user/signup/{agentName}");

		var jsonNode = await PostAsync(
			url,
			JsonDocument.Parse(JsonSerializer.Serialize(request)),
			nameof(SignUpAsync),
			cancellationToken).ConfigureAwait(false);

		var codeId = jsonNode!["codeId"]!.GetValue<int>();

		if (codeId != 0)
		{
			if (codeId != 116)
			{
				throw new GameInterfaceException(codeId);
			}
		}
	}

	public async ValueTask<string?> LoginAsync(
		Uri apiUrl,
		string apiKey,
		string agentName,
		string username,
		string password,
		bool isMobile,
		string lang = "en",
		CancellationToken cancellationToken = default)
	{
		var random = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		var token = GetHashMD5($"{agentName}{apiKey}{random}");

		var request = new
		{
			token,
			random,
			lang,
			domains = "1",
			member = new
			{
				username,
				password = GetHashMD5(password)
			}
		};

		var url = new Uri(apiUrl, $"/user/login/{agentName}");

		var jsonNode = await PostAsync(
			url,
			JsonDocument.Parse(JsonSerializer.Serialize(request)),
			nameof(LoginAsync),
			cancellationToken).ConfigureAwait(false);

		var codeId = jsonNode!["codeId"]!.GetValue<int>();

		if (codeId is not 0)
		{
			throw new GameInterfaceException(codeId);
		}

		var list = jsonNode!["list"]!.AsArray();

		var gameUrl = isMobile ? list![1]!.ToString() : list![0]!.ToString();
		gameUrl += $"{jsonNode!["token"]!}&language={lang}";

		return gameUrl;
	}

	public async ValueTask<GetBalanceResult> GetBalanceAsync(Uri apiUrl, string apiKey, string agentName, string username, CancellationToken cancellationToken = default)
	{
		var random = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		var token = GetHashMD5($"{agentName}{apiKey}{random}");

		var request = new
		{
			token,
			random,
			member = new
			{
				username
			}
		};

		var url = new Uri(apiUrl, $"/user/getBalance/{agentName}");

		var jsonNode = await PostAsync(
			url,
			JsonDocument.Parse(JsonSerializer.Serialize(request)),
			nameof(GetBalanceAsync),
			cancellationToken).ConfigureAwait(false);

		var codeId = jsonNode!["codeId"]!.GetValue<int>();

		if (codeId is not 0)
		{
			return codeId is not 114
				? throw new GameInterfaceException(codeId)
				: new(false, 0);
		}

		var member = jsonNode!["member"]!.AsObject();

		var balance = member["balance"]!.GetValue<decimal>();

		return new GetBalanceResult(true, balance);
	}

	public async ValueTask<TransferResult> TransferAsync(
		Uri apiUrl,
		string apiKey,
		string agentName,
		string username,
		string refTransactionCode,
		decimal amount,
		CancellationToken cancellationToken = default)
	{
		var random = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		var token = GetHashMD5($"{agentName}{apiKey}{random}");

		var request = new
		{
			token,
			random,
			data = refTransactionCode.Replace("-", ""),
			member = new
			{
				username,
				amount
			}
		};

		var url = new Uri(apiUrl, $"/account/transfer/{agentName}");

		var jsonNode = await PostAsync(
			url,
			JsonDocument.Parse(JsonSerializer.Serialize(request)),
			nameof(TransferAsync),
			cancellationToken).ConfigureAwait(false);

		var codeId = jsonNode!["codeId"]!.GetValue<int>();

		if (codeId is not 0)
		{
			throw new GameInterfaceException(codeId);
		}

		var transactionId = jsonNode!["data"]!.ToString();

		var member = jsonNode!["member"]!.AsObject();

		var balance = member["balance"]!.GetValue<decimal>();

		return new TransferResult(
			RefTransactionCode: refTransactionCode,
			TransactionId: transactionId,
			Balance: balance,
			Resp: jsonNode.ToJsonString());
	}

	public async ValueTask<bool> CheckTransferAsync(
		Uri apiUrl,
		string apiKey,
		string agentName,
		string refTransactionCode,
		CancellationToken cancellationToken = default)
	{
		var random = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		var token = GetHashMD5($"{agentName}{apiKey}{random}");

		var request = new
		{
			token,
			random,
			data = refTransactionCode.Replace("-", "")
		};

		var url = new Uri(apiUrl, $"/account/checkTransfer/{agentName}");

		var jsonNode = await PostAsync(
			url,
			JsonDocument.Parse(JsonSerializer.Serialize(request)),
			nameof(CheckTransferAsync),
			cancellationToken).ConfigureAwait(false);

		var codeId = jsonNode!["codeId"]!.GetValue<int>();

		return codeId is 0;
	}

	public async IAsyncEnumerable<GetReportResult> GetReportAsync(
		Uri apiUrl,
		string apiKey,
		string agentName,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var tryCount = 1;
		var seconds = 30;
		var random = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		var token = GetHashMD5($"{agentName}{apiKey}{random}");
		var request = new
		{
			token,
			random
		};

		var retryPolicy = Policy.HandleResult<JsonNode>(jsonNode
			 => jsonNode["codeId"]!.GetValue<int>() is 405 or 406)
		 .WaitAndRetryAsync(tryCount,
			 retryAttempt => TimeSpan.FromSeconds(seconds),
			 (response, retryTime, context) =>
			 {
				 var data = new
				 {
					 GameProviderCode = "DGTF",
					 Response = response,
					 Delay = retryTime,
					 CreationTime = DateTimeOffset.UtcNow
				 };
				 logger.LogWarning("{JsonSerializer.Serialize(data)}", JsonSerializer.Serialize(data));
			 });

		var url = new Uri(apiUrl, $"/game/getReport/{agentName}");

		var jsonNode = await retryPolicy.ExecuteAsync(async ()
			=> await PostAsync(
				apiUrl: url,
				request: JsonDocument.Parse(JsonSerializer.Serialize(request)),
				logPrefix: nameof(GetReportAsync))
			.ConfigureAwait(false))
			.ConfigureAwait(false);

		var codeId = jsonNode!["codeId"]!.GetValue<int>();

		if (codeId is not 0)
		{
			throw new GameInterfaceException(codeId);
		}

		if (jsonNode["list"] == null)
			yield break;

		var listArray = jsonNode["list"]!.AsArray();

		if (listArray is null || listArray.Count == 0)
			yield break;

		var betResults = listArray.Select(x => new GetReportResult
		{
			//会员登入账号
			PlayerCode = x!["userName"]!.ToString(),
			//注单唯一Id
			ExternalRoundId = x["id"]!.ToString(),
			//是否结算：0:未結算 1：已结算 2:撤销 3:凍結
			IsClosed = x["isRevocation"]!.GetValue<int>() is 1 or 3,
			//是否作廢：0:未結算 1：已结算 2:撤销 3:凍結
			IsVoid = x["isRevocation"]!.GetValue<int>() == 2,
			//游戏下注时间
			ExternalCreatedOn = new DateTimeOffset(DateTime.Parse(x["betTime"]!.ToString()), TimeSpan.FromHours(8D)),
			//游戏结算时间
			ExternalClosedOn = x["calTime"] is not null
				? new DateTimeOffset(DateTime.Parse(x["calTime"]!.ToString()), TimeSpan.FromHours(8D))
				: DateTimeOffset.UnixEpoch,
			//下注金额
			Bet = x["betPoints"]!.GetValue<decimal>(),
			//有效下注金额
			ValidBet = x["availableBet"] == null
				? 0
				: x["availableBet"]!.GetValue<decimal>(),
			//派彩金额 (输赢应扣除下注金额)
			Settle = x["winOrLoss"] == null
				? 0 :
				x["winOrLoss"]!.GetValue<decimal>(),
			ExternalGameId = x["tableId"]!.ToString(),
			AdditionalData = x.ToString(),
		}) ?? [];

		if (!betResults.Any())
		{
			yield break;
		}

		foreach (var bet in betResults)
		{
			yield return bet;
		}
	}

	public async ValueTask<string> GetGameResultAsync(
		Uri videoGameApiUrl,
		string agentName,
		string language,
		string id,
		CancellationToken cancellationToken = default)
	{
		var httpResponseMessage = await client
			.GetAsync($"{videoGameApiUrl?.ToString()}ag/result/getAgentFix.html?username={agentName}", cancellationToken)
			.ConfigureAwait(false);

		using var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result);
		var response = await sr.ReadToEndAsync(cancellationToken: cancellationToken).ConfigureAwait(false) ?? string.Empty;

		var agentFix = JsonSerializer.Deserialize<string>(response);

		return $"{videoGameApiUrl?.ToString()}ag/result/result.html?language={language}&agentFix={agentFix}&id={id}";
	}

	public async ValueTask<bool> MarkReportAsync(
		Uri apiUrl,
		string apiKey,
		string agentName,
		IEnumerable<string> externalRoundIds,
		CancellationToken cancellationToken = default)
	{
		var tryCount = 1;
		var seconds = 30;
		var random = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		var token = GetHashMD5($"{agentName}{apiKey}{random}");
		var request = new
		{
			token,
			random,
			list = externalRoundIds.ToArray()
		};

		var retryPolicy = Policy.HandleResult<JsonNode>(jsonNode
			 => jsonNode["codeId"]!.GetValue<int>() == 405)
			.WaitAndRetryAsync(tryCount,
			 retryAttempt => TimeSpan.FromSeconds(seconds),
			 (response, retryTime, context) =>
			 {
				 var data = new
				 {
					 GameProviderCode = "DGTF",
					 Response = response,
					 Delay = retryTime,
					 CreationTime = DateTimeOffset.UtcNow
				 };
				 logger.LogWarning("{JsonSerializer.Serialize(data)}", JsonSerializer.Serialize(data));
			 });

		var url = new Uri(apiUrl, $"/game/markReport/{agentName}");

		var jsonNode = await retryPolicy.ExecuteAsync(async ()
			 => await PostAsync(
				 url,
				 JsonDocument.Parse(JsonSerializer.Serialize(request)),
				 nameof(MarkReportAsync)).ConfigureAwait(false)).ConfigureAwait(false);

		return jsonNode["codeId"]!.GetValue<int>() == 0;
	}

	public async IAsyncEnumerable<OnlineReportResult> OnlineReportAsync(
		Uri apiUrl,
		string apiKey,
		string agentName,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var tryCount = 1;
		var seconds = 30;
		var random = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		var token = GetHashMD5($"{agentName}{apiKey}{random}");
		var request = new
		{
			token,
			random
		};

		var retryPolicy = Policy.HandleResult<JsonNode>(jsonNode
			 => jsonNode["codeId"]!.GetValue<int>() == 405)
		 .WaitAndRetryAsync(tryCount,
			 retryAttempt => TimeSpan.FromSeconds(seconds),
			 (response, retryTime, context) =>
			 {
				 var data = new
				 {
					 GameProviderCode = "DGTF",
					 Response = response,
					 Delay = retryTime,
					 CreationTime = DateTimeOffset.UtcNow
				 };
				 logger.LogWarning("{JsonSerializer.Serialize(data)}", JsonSerializer.Serialize(data));
			 });

		var url = new Uri(apiUrl, $"/user/onlineReport/{agentName}");

		var jsonNode = await retryPolicy.ExecuteAsync(async ()
			=> await PostAsync(
				url,
				JsonDocument.Parse(JsonSerializer.Serialize(request)),
				nameof(OnlineReportAsync)).ConfigureAwait(false)).ConfigureAwait(false);

		var codeId = jsonNode["codeId"]!.GetValue<int>();

		if (codeId is not 0)
		{
			throw new GameInterfaceException(codeId);
		}

		if (jsonNode["list"] == null)
			yield break;

		var jsonList = jsonNode["list"]!.AsArray();

		var onlineList = jsonList.Select(x => new OnlineReportResult
		{
			PlayerCode = x!["username"]!.ToString().ToLower(),
			MemberId = x["memberId"]!.GetValue<long>(),
		}) ?? [];

		if (!onlineList.Any())
		{
			yield break;
		}

		foreach (var online in onlineList)
		{
			yield return online;
		}
	}

	public async ValueTask<bool> OfflineAsync(
		Uri apiUrl,
		string apiKey,
		string agentName,
		long memberId,
		CancellationToken cancellationToken = default)
	{
		var tryCount = 1;
		var seconds = 30;
		var random = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		var token = GetHashMD5($"{agentName}{apiKey}{random}");
		var request = new
		{
			token,
			random,
			list = new[] { memberId }
		};

		var retryPolicy = Policy.HandleResult<JsonNode>(jsonNode
			 => jsonNode["codeId"]!.GetValue<int>() == 405)
			.WaitAndRetryAsync(tryCount,
			 retryAttempt => TimeSpan.FromSeconds(seconds),
			 (response, retryTime, context) =>
			 {
				 var data = new
				 {
					 GameProviderCode = "DGTF",
					 Response = response,
					 Delay = retryTime,
					 CreationTime = DateTimeOffset.UtcNow
				 };
				 logger.LogWarning("{JsonSerializer.Serialize(data)}", JsonSerializer.Serialize(data));
			 });

		var url = new Uri(apiUrl, $"/user/offline/{agentName}");

		var response = await retryPolicy.ExecuteAsync(async ()
			 => await PostAsync(
				 url,
				 JsonDocument.Parse(JsonSerializer.Serialize(request)),
				 nameof(OfflineAsync)).ConfigureAwait(false)).ConfigureAwait(false);

		return response["codeId"]!.GetValue<int>() == 0;
	}

	public async ValueTask<bool> UpdateLimitAsync(
		Uri apiUrl,
		string apiKey,
		string agentName,
		string username,
		string tableLimit,
		CancellationToken cancellationToken = default)
	{
		var random = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		var token = GetHashMD5($"{agentName}{apiKey}{random}");

		var request = new
		{
			token,
			random,
			data = tableLimit,
			member = new { username }
		};

		var url = new Uri(apiUrl, $"/game/updateLimit/{agentName}");

		var jsonNode = await PostAsync(
			url,
			JsonDocument.Parse(JsonSerializer.Serialize(request)),
			nameof(UpdateLimitAsync),
			cancellationToken).ConfigureAwait(false);

		var codeId = jsonNode!["codeId"]!.GetValue<int>();

		return codeId is 0;
	}

	private static string GetHashMD5(string data)
	{
		var dataByte = Encoding.UTF8.GetBytes(data);
		var hash = MD5.HashData(dataByte);
		return hash.ToHexString();
	}

	private async ValueTask<JsonNode> PostAsync(Uri apiUrl, JsonDocument request, string logPrefix, CancellationToken cancellationToken = default)
	{
		HttpContent content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

		var sw = Stopwatch.StartNew();

		var httpResponseMessage = await client.PostAsync(apiUrl, content, cancellationToken).ConfigureAwait(false);

		sw.Stop();

		var responseMessage = default(string);
		using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
		{
			responseMessage = await sr.ReadToEndAsync(cancellationToken: cancellationToken).ConfigureAwait(false) ?? string.Empty;
		}

		var apiLogEvent = new
		{
			Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
			Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
			Prefix = logPrefix,
			ServerName = Environment.MachineName.NoLongerThan(128),
			RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
			RequestContent = (await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
			ResponseHeaders = httpResponseMessage.Headers.ToString(),
			ResponseContent = responseMessage,
			ResponseCode = (short)httpResponseMessage.StatusCode,
			ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
			RecordedOn = DateTimeOffset.UtcNow
		};

		logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

		_ = httpResponseMessage.EnsureSuccessStatusCode();

		return JsonNode.Parse(responseMessage)!;
	}
}
