using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;
using Tgs.GameInterface.DreamGamingService.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.DreamGamingService.Controllers;

[ApiController]
[Route("dgtf")]
public class DreamGamingController : ControllerBase
{
    /// <summary>
    /// Funds the in asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.dg0.co/",
    ///             "apiKey": "d39da9da731b427b9349b80d9d97f2e4",
    ///             "agentName": "DGTE0105IH",
    ///             "username": "4ucmqplxb029",
    ///             "refTransactionCode": "fc730000e84770b5d8fc08dab0e9d6cd",
    ///             "amount": -1000
    ///         }
    /// </remarks>
    [HttpPost("fund/in", Name = "FundIn")]
    public async ValueTask<FundResultViewModel> FundInAsync(
        [FromServices] IGetService<FundInQuery, FundResult> service,
        FundViewModel source)
    {
        var query = new FundInQuery(
            ApiUrl: source.ApiUrl,
            ApiKey: source.ApiKey,
            AgentName: source.AgentName,
            Username: source.Username,
            RefTransactionCode: source.RefTransactionCode,
            Amount: source.Amount);

        var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new FundResultViewModel(
            RefTransactionCode: result.RefTransactionCode,
            TransactionId: result.TransactionId,
            Balance: result.Balance);
    }

    /// <summary>
    /// Funds the out asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.dg0.co/",
    ///             "apiKey": "d39da9da731b427b9349b80d9d97f2e4",
    ///             "agentName": "DGTE0105IH",
    ///             "username": "4ucmqplxb029",
    ///             "refTransactionCode": "fc730000e84770b5d8fc08dab0e9d6cd",
    ///             "amount": 1000
    ///         }
    /// </remarks>
    [HttpPost("fund/out", Name = "FundOut")]
    public async ValueTask<FundResultViewModel> FundOutAsync(
            [FromServices] IGetService<FundOutQuery, FundResult> service,
            FundViewModel source)
    {
        var query = new FundOutQuery(
            ApiUrl: source.ApiUrl,
            ApiKey: source.ApiKey,
            AgentName: source.AgentName,
            Username: source.Username,
            RefTransactionCode: source.RefTransactionCode,
            Amount: source.Amount);

        var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new FundResultViewModel(
            RefTransactionCode: result.RefTransactionCode,
            TransactionId: result.TransactionId,
            Balance: result.Balance);
    }

    /// <summary>
    /// Funds the status asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.dg0.co/",
    ///             "apiKey": "d39da9da731b427b9349b80d9d97f2e4",
    ///             "agentName": "DGTE0105IH",
    ///             "refTransactionCode": "4c640000e84770b57e0e08dab0e9face"
    ///         }
    /// </remarks>
    [HttpPost("fund/Status", Name = "FundStatus")]
    public ValueTask<bool> FundStatusAsync(
        [FromServices] IGetService<FundStatusQuery, bool> service,
        FundStatusViewModel source)
    {
        var query = new FundStatusQuery(
                ApiUrl: source.ApiUrl,
                ApiKey: source.ApiKey,
                AgentName: source.AgentName,
                RefTransactionCode: source.RefTransactionCode);

        return service.GetAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Players the create asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.dg0.co/",
    ///             "apiKey": "d39da9da731b427b9349b80d9d97f2e4",
    ///             "agentName": "DGTE0105IH",
    ///             "username": "4ucmqplxb029",
    ///             "password": "DGTFP@ssw0rd",
    ///             "currencyName": "CNY",
    ///             "tableLimit": "A",
    ///             "winLimit": 0
    ///         }
    /// </remarks>
    [HttpPost("player/create", Name = "PlayerCreate")]
    public ValueTask PlayerCreateAsync(
        [FromServices] ICommandService<PlayerCreateCommand> service,
        PlayerCreateViewModel source)
    {
        var command = new PlayerCreateCommand(
            ApiUrl: source.ApiUrl,
            ApiKey: source.ApiKey,
            AgentName: source.AgentName,
            Username: source.Username,
            Password: source.Password,
            CurrencyName: source.CurrencyName,
            TableLimit: source.TableLimit,
            WinLimit: source.WinLimit);
        return service.ExecuteAsync(
                command: command,
                cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Players the balance asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.dg0.co/",
    ///             "apiKey": "d39da9da731b427b9349b80d9d97f2e4",
    ///             "agentName": "DGTE0105IH",
    ///             "username": "4ucmqplxb029"
    ///         }
    /// </remarks>
    [HttpPost("player/balance", Name = "PlayerBalance")]
    public async ValueTask<PlayerBalanceResultViewModel> PlayerBalanceAsync(
        [FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
        PlayerBalanceViewModel source)
    {
        var query = new PlayerBalanceQuery(
            ApiUrl: source.ApiUrl,
            ApiKey: source.ApiKey,
            AgentName: source.AgentName,
            Username: source.Username);

        var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new PlayerBalanceResultViewModel(
            result.IsPlayerExist,
            result.Balance);
    }

    /// <summary>
    /// Games the URL asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.dg0.co/",
    ///             "apiKey": "d39da9da731b427b9349b80d9d97f2e4",
    ///             "agentName": "DGTE0105IH",
    ///             "username": "4ucmqplxb029",
    ///             "password": "DGTFP@ssw0rd",
    ///             "isMobile": false,
    ///             "lang": "en"
    ///         }
    /// </remarks>
    [HttpPost("gameurl")]
    public async ValueTask<GameUrlResultViewModel> GameUrlAsync(
        [FromServices] IGetService<GameUrlQuery, string?> service,
        GameUrlQueryViewModel model)
    {
        var result = await service
            .GetAsync(new GameUrlQuery(
                model.ApiUrl,
                model.ApiKey,
                model.AgentName,
                model.Username,
                model.Password,
                model.IsMobile,
                model.Lang))
            .ConfigureAwait(false);

        return new GameUrlResultViewModel(result);
    }

    /// <summary>
    /// Games the history settled asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.dg0.co/",
    ///             "apiKey": "d39da9da731b427b9349b80d9d97f2e4",
    ///             "agentName": "DGTE0105IH"
    ///         }
    /// </remarks>
    [HttpPost("game/history/settled", Name = "GameHistorySettled")]
    public async IAsyncEnumerable<GameHistorySettledResultViewModel> GameHistorySettledAsync(
        [FromServices] IQueryService<GameHistorySettledQuery, GameHistorySettledResult> service,
        GameHistorySettledViewModel model)
    {
        var query = new GameHistorySettledQuery(
            ApiUrl: model.ApiUrl,
            ApiKey: model.ApiKey,
            AgentName: model.AgentName);

        var gameRecords = await service
                .QueryAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ToArrayAsync(
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        if (!gameRecords.Any())
        {
            yield break;
        }

        foreach (var gameRecord in gameRecords)
        {
            yield return new GameHistorySettledResultViewModel(
                PlayerCode: gameRecord.PlayerCode,
                ExternalRoundId: gameRecord.ExternalRoundId,
                IsClosed: gameRecord.IsClosed,
                ExternalCreatedOn: gameRecord.ExternalCreatedOn,
                ExternalClosedOn: gameRecord.ExternalClosedOn,
                Bet: gameRecord.Bet,
                ValidBet: gameRecord.ValidBet,
                PlayerWinLoss: gameRecord.PlayerWinLoss,
                Turnover: gameRecord.Turnover,
                Settle: gameRecord.Settle,
                AdditionalData: gameRecord.AdditionalData,
                ExternalGameId: gameRecord.ExternalGameId);
        }
    }

    /// <summary>
    /// Players the online asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.dg0.co/",
    ///             "apiKey": "d39da9da731b427b9349b80d9d97f2e4",
    ///             "agentName": "DGTE0105IH"
    ///         }
    /// </remarks>
    [HttpPost("player/online", Name = "PlayerOnline")]
    public async IAsyncEnumerable<PlayerOnlineResultViewModel> PlayerOnlineAsync(
        [FromServices] IQueryService<PlayerOnlineQuery, PlayerOnlineResult> service,
        PlayerOnlineViewModel model)
    {
        var query = new PlayerOnlineQuery(
            ApiUrl: model.ApiUrl,
            ApiKey: model.ApiKey,
            AgentName: model.AgentName);

        var onlineResults = await service
                .QueryAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ToArrayAsync(
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        if (!onlineResults.Any())
        {
            yield break;
        }

        foreach (var online in onlineResults)
        {
            yield return new PlayerOnlineResultViewModel(online.PlayerCode, online.MemberId);
        }
    }

    /// <summary>
    /// Players the kick asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.dg0.co/",
    ///             "apiKey": "d39da9da731b427b9349b80d9d97f2e4",
    ///             "agentName": "DGTE0105IH",
    ///             "memberId": 94030972
    ///         }
    /// </remarks>
    [HttpPost("player/kick", Name = "PlayerKick")]
    public ValueTask PlayerKickAsync(
        [FromServices] ICommandService<PlayerKickCommand> service,
        PlayerKickViewModel source)
        => service
            .ExecuteAsync(
                command: new PlayerKickCommand(
                    source.ApiUrl,
                    source.ApiKey,
                    source.AgentName,
                    source.MemberId),
                cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Players the table limit asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.dg0.co/",
    ///             "apiKey": "d39da9da731b427b9349b80d9d97f2e4",
    ///             "agentName": "DGTE0105IH",
    ///             "username": "4ucmqplxb029",
    ///             "tablelimit": "A"
    ///         }
    /// </remarks>
    [HttpPost("player/tablelimit", Name = "PlayerTableLimit")]
    public ValueTask<bool> PlayerTableLimitAsync(
        [FromServices] IGetService<PlayerTableLimitCommand, bool> service,
        PlayerTableLimitViewModel source)
        => service
            .GetAsync(
                query: new PlayerTableLimitCommand(
                    source.ApiUrl,
                    source.ApiKey,
                    source.AgentName,
                    source.Username,
                    source.TableLimit),
                cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Games the history mark asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://api.dg0.co/",
    ///             "apiKey": "d39da9da731b427b9349b80d9d97f2e4",
    ///             "agentName": "DGTE0105IH",
    ///             "externalRoundIds": ["13562946013"]
    ///         }
    /// </remarks>
    [HttpPost("game/history/mark", Name = "GameHistoryMark")]
    public ValueTask<bool> GameHistoryMarkAsync(
        [FromServices] IGetService<GameHistoryMarkQuery, bool> service,
        GameHistoryMarkViewModel source)
        => service.GetAsync(
            query: new GameHistoryMarkQuery(
                source.ApiUrl,
                source.ApiKey,
                source.AgentName,
                source.ExternalRoundIds),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Games the round detail asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "videoGameApiUrl": "https://ag.dg66.info/",
    ///             "agentName": "DGTE0105IH",
    ///             "language": "en",
    ///             "id": "13540803433"
    ///         }
    /// </remarks>
    [HttpPost("game/history/detail", Name = "GameRoundDetail")]
    public ValueTask<string> GameRoundDetailAsync(
        [FromServices] IGetService<GameRoundDetailQuery, string> service,
        GameRoundDetailViewModel source)
        => service.GetAsync(
            query: new GameRoundDetailQuery(
                source.VideoGameApiUrl,
                source.AgentName,
                source.Language,
                source.Id),
            cancellationToken: HttpContext.RequestAborted);
}
