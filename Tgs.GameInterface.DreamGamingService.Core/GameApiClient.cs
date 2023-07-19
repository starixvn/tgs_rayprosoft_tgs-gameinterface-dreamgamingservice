using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Polly;
using Tgs.GameInterface.DreamGamingService.Core.Exceptions;
using Tgs.GameInterface.DreamGamingService.Core.Models;

namespace Tgs.GameInterface.DreamGamingService.Core;

internal class GameApiClient : IGameApiClient
{
    private readonly HttpClient _client;
    private readonly ILogger<GameApiClient> _logger;

    private readonly Dictionary<int, string> _codeIdDescription = new()
    {
        {  0, "操作成功" },
        {  1, "参数错误" },
        {  2, "Token验证失败" },
        {  4, "非法操作" },
        { 10,"日期格式错误" },
        { 11,"数据格式错误" },
        { 97,"没有权限" },
        { 98,"操作失败" },
        { 99,"未知错误" },
        { 100,"账号被锁定" },
        { 101,"账号格式错误" },
        { 102,"账号不存在" },
        { 103,"此账号被占用" },
        { 104,"密码格式错误" },
        { 105,"密码错误" },
        { 106,"新旧密码相同" },
        { 107,"会员账号不可用" },
        { 108,"登入失败" },
        { 109,"注册失败" },
        { 113,"传入的代理账号不是代理" },
        { 114,"找不到会员" },
        { 116,"账号已占用" },
        { 117,"找不到会员所属的分公司" },
        { 118,"找不到指定的代理" },
        { 119,"存取款操作时代理点数不足" },
        { 120,"余额不足" },
        { 121,"盈利限制必须大于或等于0" },
        { 150,"免费试玩账号用完" },
        { 300,"系统维护" },
        { 301,"代理账号找不到" },
        { 320,"API Key 错误" },
        { 321,"找不到相应的限红组" },
        { 322,"找不到指定的货币类型" },
        { 323,"转账流水号占用" },
        { 324,"转账失败" },
        { 325,"代理状态不可用" },
        { 326,"会员代理没有视频组" },
        { 328,"API 类型找不到" },
        { 329,"会员代理信息不完整" },
        { 400,"客户端IP 受限" },
        { 401,"网络延迟" },
        { 402,"连接关闭" },
        { 403,"客户端来源受限" },
        { 404,"请求的资源不存在" },
        { 405,"请求太频繁" },
        { 406,"请求超时" },
        { 407,"找不到游戏地址" },
        { 500,"空指针异常" },
        { 501,"系统异常" },
        { 502,"系统忙" },
        { 503,"数据操作异常" },
    };

    public GameApiClient(
        HttpClient client,
        ILogger<GameApiClient> logger)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

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
            "SignUp",
            cancellationToken).ConfigureAwait(false);

        var codeId = jsonNode!["codeId"]!.GetValue<int>();

        if (codeId is not 0)
        {
            _ = _codeIdDescription.TryGetValue(codeId, out var codeIdDescription);
            throw new GameProviderResponseErrorException(codeId, codeIdDescription);
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
            "Login",
            cancellationToken).ConfigureAwait(false);

        var codeId = jsonNode!["codeId"]!.GetValue<int>();

        if (codeId is not 0)
        {
            _ = _codeIdDescription.TryGetValue(codeId, out var codeIdDescription);
            throw new GameProviderResponseErrorException(codeId, codeIdDescription);
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
            "GetBalance",
            cancellationToken).ConfigureAwait(false);

        var codeId = jsonNode!["codeId"]!.GetValue<int>();

        if (codeId is not 0)
        {
            _ = _codeIdDescription.TryGetValue(codeId, out var codeIdDescription);
            _logger.LogWarning("GetBalance Failed, codeId:{codeId}, ErrorMessage:{codeIdDescription}", codeId, codeIdDescription);
            return new(false, 0);
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
            "Transfer",
            cancellationToken).ConfigureAwait(false);

        var codeId = jsonNode!["codeId"]!.GetValue<int>();

        if (codeId is not 0)
        {
            _ = _codeIdDescription.TryGetValue(codeId, out var codeIdDescription);
            throw new GameProviderResponseErrorException(codeId, codeIdDescription);
        }
        var transactionId = jsonNode!["data"]!.ToString();

        var member = jsonNode!["member"]!.AsObject();

        var balance = member["balance"]!.GetValue<decimal>();

        return new TransferResult(refTransactionCode, transactionId, balance);
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
            "CheckTransfer",
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
                 _logger.LogWarning("{JsonSerializer.Serialize(data)}", JsonSerializer.Serialize(data));
             });

        var url = new Uri(apiUrl, $"/game/getReport/{agentName}");

        var jsonNode = await retryPolicy.ExecuteAsync(async ()
            => await PostAsync(
                url,
                JsonDocument.Parse(JsonSerializer.Serialize(request)),
                "GetReport").ConfigureAwait(false)).ConfigureAwait(false);

        var codeId = jsonNode!["codeId"]!.GetValue<int>();

        if (codeId is not 0)
        {
            _ = _codeIdDescription.TryGetValue(codeId, out var codeIdDescription);
            throw new GameProviderResponseErrorException(codeId, codeIdDescription);
        }

        if (jsonNode["list"] == null)
            yield break;

        var listArray = jsonNode["list"]!.AsArray();

        if (listArray is null || !listArray.Any())
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
        }) ?? Enumerable.Empty<GetReportResult>();

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
        var httpResponseMessage = await _client
            .GetAsync($"{videoGameApiUrl?.ToString()}ag/result/getAgentFix.html?username={agentName}", cancellationToken)
            .ConfigureAwait(false);

        using var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result);
        var response = await sr.ReadToEndAsync(cancellationToken: cancellationToken) ?? string.Empty;

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
                 _logger.LogWarning("{JsonSerializer.Serialize(data)}", JsonSerializer.Serialize(data));
             });

        var url = new Uri(apiUrl, $"/game/markReport/{agentName}");

        var jsonNode = await retryPolicy.ExecuteAsync(async ()
             => await PostAsync(
                 url,
                 JsonDocument.Parse(JsonSerializer.Serialize(request)),
                 "MarkReport").ConfigureAwait(false)).ConfigureAwait(false);

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
                 _logger.LogWarning("{JsonSerializer.Serialize(data)}", JsonSerializer.Serialize(data));
             });

        var url = new Uri(apiUrl, $"/user/onlineReport/{agentName}");

        var jsonNode = await retryPolicy.ExecuteAsync(async ()
            => await PostAsync(
                url,
                JsonDocument.Parse(JsonSerializer.Serialize(request)),
                "OnlineReport").ConfigureAwait(false)).ConfigureAwait(false);

        var codeId = jsonNode["codeId"]!.GetValue<int>();

        if (codeId is not 0)
        {
            _ = _codeIdDescription.TryGetValue(codeId, out var codeIdDescription);
            throw new GameProviderResponseErrorException(codeId, codeIdDescription);
        }

        if (jsonNode["list"] == null)
            yield break;

        var jsonList = jsonNode["list"]!.AsArray();

        var onlineList = jsonList.Select(x => new OnlineReportResult
        {
            PlayerCode = x!["username"]!.ToString().ToLower(),
            MemberId = x["memberId"]!.GetValue<long>(),
        }) ?? Enumerable.Empty<OnlineReportResult>();

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
                 _logger.LogWarning("{JsonSerializer.Serialize(data)}", JsonSerializer.Serialize(data));
             });

        var url = new Uri(apiUrl, $"/user/offline/{agentName}");

        var response = await retryPolicy.ExecuteAsync(async ()
             => await PostAsync(
                 url,
                 JsonDocument.Parse(JsonSerializer.Serialize(request)),
                 "Offline").ConfigureAwait(false)).ConfigureAwait(false);

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
            "UpdateLimit",
            cancellationToken).ConfigureAwait(false);

        var codeId = jsonNode!["codeId"]!.GetValue<int>();

        return codeId is 0;
    }

    private static string GetHashMD5(string data)
    {
        using var md5 = MD5.Create();
        var dataByte = Encoding.UTF8.GetBytes(data);
        var hash = md5.ComputeHash(dataByte);
        return hash.ToHexString();
    }

    private async ValueTask<JsonNode> PostAsync(Uri apiUrl, JsonDocument request, string logPrefix, CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        var sw = Stopwatch.StartNew();

        HttpContent content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var httpResponseMessage = await _client.PostAsync(apiUrl, content, cancellationToken);
        sw.Stop();

        var responseMessage = default(string);
        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr.ReadToEndAsync(cancellationToken: cancellationToken).ConfigureAwait(false) ?? string.Empty;
        }

        var apiLogEvent = new GameApiLogEvent
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

        _logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        _ = httpResponseMessage.EnsureSuccessStatusCode();

        return JsonNode.Parse(responseMessage)!;
    }
}
