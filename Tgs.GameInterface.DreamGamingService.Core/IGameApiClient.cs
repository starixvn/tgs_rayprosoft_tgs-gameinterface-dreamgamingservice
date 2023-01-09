using Tgs.GameInterface.DreamGamingService.Core.Models;

namespace Tgs.GameInterface.DreamGamingService.Core;

public interface IGameApiClient
{
    /// <summary>
    /// 注册新会员
    /// </summary>
    ValueTask SignUpAsync(
        Uri apiUrl,
        string apiKey,
        string agentName,
        string username,
        string password,
        string currencyName,
        string tableLimit = "A",
        int winLimit = 0,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 会员登入
    /// </summary>
    ValueTask<string?> LoginAsync(
        Uri apiUrl,
        string apiKey,
        string agentName,
        string username,
        string password,
        bool isMobile,
        string lang = "en",
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取会员余额
    /// </summary>
    ValueTask<GetBalanceResult> GetBalanceAsync(
        Uri apiUrl,
        string apiKey,
        string agentName,
        string username,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 会员存取款
    /// </summary>
    /// <param name="apiUrl">The API URL.</param>
    /// <param name="apiKey">The API key.</param>
    /// <param name="agentName">Name of the agent.</param>
    /// <param name="username">The username.</param>
    /// <param name="refTransactionCode">The reference transaction code.</param>
    /// <param name="amount">存取款金额，正数存款负数取款，请确保保留不超过2位小数</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    ValueTask<TransferResult> TransferAsync(
        Uri apiUrl,
        string apiKey,
        string agentName,
        string username,
        string refTransactionCode,
        decimal amount,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 检查存取款操作是否成功
    /// </summary>
    ValueTask<bool> CheckTransferAsync(
        Uri apiUrl,
        string apiKey,
        string agentName,
        string refTransactionCode,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 抓取注单报表
    /// </summary>
    IAsyncEnumerable<GetReportResult> GetReportAsync(
        Uri apiUrl,
        string apiKey,
        string agentName,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 游戏注单解析入口
    /// </summary>
    ValueTask<string> GetGameResultAsync(
        Uri videoGameApiUrl, 
        string agentName,
        string language,
        string id,
        CancellationToken cancellationToken = default);    

    /// <summary>
    /// 标记已抓取注单
    /// </summary>
    ValueTask<bool> MarkReportAsync(
        Uri apiUrl,
        string apiKey,
        string agentName,
        IEnumerable<string> externalRoundIds,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取当前代理下在DG在线会员信息
    /// </summary>
    IAsyncEnumerable<OnlineReportResult> OnlineReportAsync(
        Uri apiUrl,
        string apiKey,
        string agentName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 踢人
    /// </summary>
    ValueTask<bool> OfflineAsync(
        Uri apiUrl,
        string apiKey,
        string agentName,
        long memberId,
        CancellationToken cancellationToken = default);
}