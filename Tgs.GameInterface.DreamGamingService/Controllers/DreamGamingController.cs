using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;
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
	///             "playerCode": "4ucmqplxb029",
	///             "orderId": "fc730000e84770b5d8fc08dab0e9d6cd",
	///             "amount": -1000
	///         }
	/// </remarks>
	[HttpPost("fund/in", Name = "FundIn")]
	public ValueTask<FundResult> FundInAsync(
		[FromServices] IGetService<FundInQuery, FundResult> service,
		FundInQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

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
	///             "playerCode": "4ucmqplxb029",
	///             "orderId": "fc730000e84770b5d8fc08dab0e9d6cd",
	///             "amount": 1000
	///         }
	/// </remarks>
	[HttpPost("fund/out", Name = "FundOut")]
	public ValueTask<FundResult> FundOutAsync(
		[FromServices] IGetService<FundOutQuery, FundResult> service,
		FundOutQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

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
	///             "orderId": "4c640000e84770b57e0e08dab0e9face"
	///         }
	/// </remarks>
	[HttpPost("fund/Status", Name = "FundStatus")]
	public ValueTask<bool> FundStatusAsync(
		[FromServices] IGetService<FundStatusQuery, bool> service,
		FundStatusQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

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
	///             "playerCode": "4ucmqplxb029",
	///             "password": "DGTFP@ssw0rd",
	///             "currencyName": "CNY",
	///             "tableLimit": "A",
	///             "winLimit": 0
	///         }
	/// </remarks>
	[HttpPost("player/create", Name = "PlayerCreate")]
	public ValueTask PlayerCreateAsync(
		[FromServices] ICommandService<PlayerCreateCommand> service,
		PlayerCreateCommand source)
		=> service.ExecuteAsync(
			command: source,
			cancellationToken: HttpContext.RequestAborted);

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
	///             "playerCode": "4ucmqplxb029"
	///         }
	/// </remarks>
	[HttpPost("player/balance", Name = "PlayerBalance")]
	public ValueTask<PlayerBalanceResult> PlayerBalanceAsync(
		[FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
		PlayerBalanceQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the URL asynchronous.
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
	///             "playerCode": "4ucmqplxb029",
	///             "password": "DGTFP@ssw0rd",
	///             "isMobile": false,
	///             "lang": "en"
	///         }
	/// </remarks>
	[HttpPost("gameurl")]
	public ValueTask<string?> GameUrlAsync(
		[FromServices] IGetService<GameUrlQuery, string?> service,
		GameUrlQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the history settled asynchronous.
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
	///             "agentName": "DGTE0105IH"
	///         }
	/// </remarks>
	[HttpPost("game/history/settled", Name = "GameHistorySettled")]
	public IAsyncEnumerable<GameHistorySettledResult> GameHistorySettledAsync(
		[FromServices] IQueryService<GameHistorySettledQuery, GameHistorySettledResult> service,
		GameHistorySettledQuery source)
		=> service.QueryAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Players the online asynchronous.
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
	///             "agentName": "DGTE0105IH"
	///         }
	/// </remarks>
	[HttpPost("player/online", Name = "PlayerOnline")]
	public IAsyncEnumerable<PlayerOnlineResult> PlayerOnlineAsync(
		[FromServices] IQueryService<PlayerOnlineQuery, PlayerOnlineResult> service,
		PlayerOnlineQuery source)
		=> service.QueryAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Players the logout asynchronous.
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
	[HttpPost("player/logout", Name = "PlayerLogout")]
	public ValueTask PlayerLogoutAsync(
		[FromServices] ICommandService<PlayerLogoutCommand> service,
		PlayerLogoutCommand source)
		=> service.ExecuteAsync(
			command: source,
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
	///             "playerCode": "4ucmqplxb029",
	///             "tablelimit": "A"
	///         }
	/// </remarks>
	[HttpPost("player/tablelimit", Name = "PlayerTableLimit")]
	public ValueTask<bool> PlayerTableLimitAsync(
		[FromServices] IGetService<PlayerTableLimitCommand, bool> service,
		PlayerTableLimitCommand source)
		=> service.GetAsync(
			query: source,
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
		GameHistoryMarkQuery source)
		=> service.GetAsync(
			query: source,
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
	///             "externalRoundId": "13540803433"
	///         }
	/// </remarks>
	[HttpPost("game/history/detail", Name = "GameRoundDetail")]
	public ValueTask<string> GameRoundDetailAsync(
		[FromServices] IGetService<GameRoundDetailQuery, string> service,
		GameRoundDetailQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);
}
