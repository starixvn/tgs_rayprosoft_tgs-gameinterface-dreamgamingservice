using Tgs.GameInterface.DreamGamingService.Core.Models;
using Tgs.GameInterface.DreamGamingService.Core.ApplicationServices;
using NSubstitute;

namespace Tgs.GameInterface.DreamGamingService.Core.UnitTest.ApplicationServices;
[TestClass]
public class GameHistorySettledQueryServiceUnitTest
{
	[TestMethod]
	public async Task GameType_Is_1_Win()
	{
		var query = new GameHistorySettledQuery(
			ApiUrl: new Uri("https://api.dg0.co/"),
			ApiKey: "d39da9da731b427b9349b80d9d97f2e4",
			AgentName: "DGTE0105IH");


		var fakeClient = Substitute.For<IGameApiClient>();

		fakeClient.GetReportAsync(
			Arg.Is(query.ApiUrl),
			Arg.Is(query.ApiKey),
			Arg.Is(query.AgentName),
			Arg.Any<CancellationToken>())
			.Returns(new[]{new GetReportResult {
			PlayerCode="4UNOTEPWIKK2",
			ExternalRoundId="22545327967",
			IsVoid=false,
			IsClosed=true,
			ExternalCreatedOn=new DateTimeOffset(DateTime.Parse("2024-09-17 16:11:53"), TimeSpan.FromHours(8D)),
			ExternalClosedOn=new DateTimeOffset(DateTime.Parse("2024-09-17 16:12:18"), TimeSpan.FromHours(8D)),
			Bet=100.0m,
			ValidBet=100.0m,
			Settle=200.0m,
			AdditionalData="{}",
			ExternalGameId="30102",
			GameType="1",
			GameId="1"
			}}.ToAsyncEnumerable());

		var sut = new GameHistorySettledQueryService(fakeClient);

		var actual = await sut.QueryAsync(query)
			.ToArrayAsync()
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameHistorySettledResult>();

		Assert.IsNotNull(actual);
		Assert.AreEqual("4UNOTEPWIKK2", actual.First().PlayerCode);
		Assert.AreEqual("22545327967", actual.First().ExternalRoundId);
		Assert.IsFalse(actual.First().IsVoid);
		Assert.IsTrue(actual.First().IsClosed);
		Assert.AreEqual(new DateTimeOffset(DateTime.Parse("2024-09-17 16:11:53"), TimeSpan.FromHours(8D)), actual.First().ExternalCreatedOn);
		Assert.AreEqual(new DateTimeOffset(DateTime.Parse("2024-09-17 16:12:18"), TimeSpan.FromHours(8D)), actual.First().ExternalClosedOn);
		Assert.AreEqual(100.0m, actual.First().Bet);
		Assert.AreEqual(100.0m, actual.First().ValidBet);
		Assert.AreEqual(100.0m, actual.First().PlayerWinLoss);
		Assert.AreEqual(100.0m, actual.First().Turnover);
		Assert.AreEqual(200.0m, actual.First().Settle);
		Assert.IsNotNull(actual.First().AdditionalData);
		Assert.AreEqual("30102", actual.First().ExternalGameId);
	}

	[TestMethod]
	public async Task GameType_Is_1_Loss()
	{
		var query = new GameHistorySettledQuery(
			ApiUrl: new Uri("https://api.dg0.co/"),
			ApiKey: "d39da9da731b427b9349b80d9d97f2e4",
			AgentName: "DGTE0105IH");


		var fakeClient = Substitute.For<IGameApiClient>();

		fakeClient.GetReportAsync(
			Arg.Is(query.ApiUrl),
			Arg.Is(query.ApiKey),
			Arg.Is(query.AgentName),
			Arg.Any<CancellationToken>())
			.Returns(new[]{new GetReportResult {
			PlayerCode="4UNOTEPWIKK2",
			ExternalRoundId="22545209117",
			IsVoid=false,
			IsClosed=true,
			ExternalCreatedOn=new DateTimeOffset(DateTime.Parse("2024-09-17 16:04:53"), TimeSpan.FromHours(8D)),
			ExternalClosedOn=new DateTimeOffset(DateTime.Parse("2024-09-17 16:05:12"), TimeSpan.FromHours(8D)),
			Bet=570.0m,
			ValidBet=570.0m,
			Settle=0.0m,
			AdditionalData="{}",
			ExternalGameId="94103",
			GameType="1",
			GameId="41"
			}}.ToAsyncEnumerable());

		var sut = new GameHistorySettledQueryService(fakeClient);

		var actual = await sut.QueryAsync(query)
			.ToArrayAsync()
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameHistorySettledResult>();

		Assert.IsNotNull(actual);
		Assert.AreEqual("4UNOTEPWIKK2", actual.First().PlayerCode);
		Assert.AreEqual("22545209117", actual.First().ExternalRoundId);
		Assert.IsFalse(actual.First().IsVoid);
		Assert.IsTrue(actual.First().IsClosed);
		Assert.AreEqual(new DateTimeOffset(DateTime.Parse("2024-09-17 16:04:53"), TimeSpan.FromHours(8D)), actual.First().ExternalCreatedOn);
		Assert.AreEqual(new DateTimeOffset(DateTime.Parse("2024-09-17 16:05:12"), TimeSpan.FromHours(8D)), actual.First().ExternalClosedOn);
		Assert.AreEqual(570.0m, actual.First().Bet);
		Assert.AreEqual(570.0m, actual.First().ValidBet);
		Assert.AreEqual(-570.0m, actual.First().PlayerWinLoss);
		Assert.AreEqual(570.0m, actual.First().Turnover);
		Assert.AreEqual(0.0m, actual.First().Settle);
		Assert.IsNotNull(actual.First().AdditionalData);
		Assert.AreEqual("94103", actual.First().ExternalGameId);
	}

	[TestMethod]
	public async Task GameType_Is_2_GameId_Is_2_DG红包()
	{
		var query = new GameHistorySettledQuery(
			ApiUrl: new Uri("https://api.dg0.co/"),
			ApiKey: "d39da9da731b427b9349b80d9d97f2e4",
			AgentName: "DGTE0105IH");


		var fakeClient = Substitute.For<IGameApiClient>();

		fakeClient.GetReportAsync(
			Arg.Is(query.ApiUrl),
			Arg.Is(query.ApiKey),
			Arg.Is(query.AgentName),
			Arg.Any<CancellationToken>())
			.Returns(new[]{new GetReportResult {
			PlayerCode="4UNOTEPWIKK2",
			ExternalRoundId="22545211903",
			IsVoid=false,
			IsClosed=true,
			ExternalCreatedOn=new DateTimeOffset(DateTime.Parse("2024-09-17 16:05:03"), TimeSpan.FromHours(8D)),
			ExternalClosedOn=new DateTimeOffset(DateTime.Parse("2024-09-17 16:05:03"), TimeSpan.FromHours(8D)),
			Bet=0.0m,
			ValidBet=0.0m,
			Settle=10.0m,
			AdditionalData="{}",
			ExternalGameId="2_2",
			GameType="2",
			GameId="2"
			}}.ToAsyncEnumerable());

		var sut = new GameHistorySettledQueryService(fakeClient);

		var actual = await sut.QueryAsync(query)
			.ToArrayAsync()
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameHistorySettledResult>();

		Assert.IsNotNull(actual);
		Assert.AreEqual("4UNOTEPWIKK2", actual.First().PlayerCode);
		Assert.AreEqual("22545211903", actual.First().ExternalRoundId);
		Assert.IsFalse(actual.First().IsVoid);
		Assert.IsTrue(actual.First().IsClosed);
		Assert.AreEqual(new DateTimeOffset(DateTime.Parse("2024-09-17 16:05:03"), TimeSpan.FromHours(8D)), actual.First().ExternalCreatedOn);
		Assert.AreEqual(new DateTimeOffset(DateTime.Parse("2024-09-17 16:05:03"), TimeSpan.FromHours(8D)), actual.First().ExternalClosedOn);
		Assert.AreEqual(0.0m, actual.First().Bet);
		Assert.AreEqual(0.0m, actual.First().ValidBet);
		Assert.AreEqual(0.0m, actual.First().PlayerWinLoss);
		Assert.AreEqual(0.0m, actual.First().Turnover);
		Assert.AreEqual(10.0m, actual.First().Settle);
		Assert.IsNotNull(actual.First().AdditionalData);
		Assert.AreEqual("2_2", actual.First().ExternalGameId);
	}

	[TestMethod]
	public async Task GameType_Is_2_GameId_Is_5_博餅活動()
	{
		var query = new GameHistorySettledQuery(
			ApiUrl: new Uri("https://api.dg0.co/"),
			ApiKey: "d39da9da731b427b9349b80d9d97f2e4",
			AgentName: "DGTE0105IH");


		var fakeClient = Substitute.For<IGameApiClient>();

		fakeClient.GetReportAsync(
			Arg.Is(query.ApiUrl),
			Arg.Is(query.ApiKey),
			Arg.Is(query.AgentName),
			Arg.Any<CancellationToken>())
			.Returns(new[]{new GetReportResult {
			PlayerCode="HOIN10587788",
			ExternalRoundId="8873437184",
			IsVoid=false,
			IsClosed=true,
			ExternalCreatedOn=new DateTimeOffset(DateTime.Parse("2021-09-19 11:48:08"), TimeSpan.FromHours(8D)),
			ExternalClosedOn=new DateTimeOffset(DateTime.Parse("2021-09-19 11:48:08"), TimeSpan.FromHours(8D)),
			Bet=0.0m,
			ValidBet=0.0m,
			Settle=37.3m,
			AdditionalData="{}",
			ExternalGameId="2_5",
			GameType="2",
			GameId="5"
			}}.ToAsyncEnumerable());

		var sut = new GameHistorySettledQueryService(fakeClient);

		var actual = await sut.QueryAsync(query)
			.ToArrayAsync()
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameHistorySettledResult>();

		Assert.IsNotNull(actual);
		Assert.AreEqual("HOIN10587788", actual.First().PlayerCode);
		Assert.AreEqual("8873437184", actual.First().ExternalRoundId);
		Assert.IsFalse(actual.First().IsVoid);
		Assert.IsTrue(actual.First().IsClosed);
		Assert.AreEqual(new DateTimeOffset(DateTime.Parse("2021-09-19 11:48:08"), TimeSpan.FromHours(8D)), actual.First().ExternalCreatedOn);
		Assert.AreEqual(new DateTimeOffset(DateTime.Parse("2021-09-19 11:48:08"), TimeSpan.FromHours(8D)), actual.First().ExternalClosedOn);
		Assert.AreEqual(0.0m, actual.First().Bet);
		Assert.AreEqual(0.0m, actual.First().ValidBet);
		Assert.AreEqual(0.0m, actual.First().PlayerWinLoss);
		Assert.AreEqual(0.0m, actual.First().Turnover);
		Assert.AreEqual(37.3m, actual.First().Settle);
		Assert.IsNotNull(actual.First().AdditionalData);
		Assert.AreEqual("2_5", actual.First().ExternalGameId);
	}
}
