using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.DreamGamingService.Core.Models;

namespace Tgs.GameInterface.DreamGamingService.Core.UnitTest;

[TestClass]
public class GetReportUnitTest
{
	[TestMethod]
	public async Task GameType_Is_1_Win()

	{
		var apiUrl = new Uri("https://api.dg0.co/");
		var apiKey = "d39da9da731b427b9349b80d9d97f2e4";
		var agentName = "DGTE0105IH";

		var url = new Uri(apiUrl, $"/game/getReport/{agentName}");
		var response = JsonNode.Parse("""
		{
			"codeId": 0,
			"token": "a90274e0909e8ff3500962668677416d",
			"random": "1726584127372",
			"list": [
				{
					"id": 22545327967,
					"tableId": 30102,
					"shoeId": 18,
					"playId": 58,
					"lobbyId": 3,
					"gameType": 1,
					"gameId": 1,
					"memberId": 151899849,
					"parentId": 30281,
					"betTime": "2024-09-17 16:11:53",
					"calTime": "2024-09-17 16:12:18",
					"winOrLoss": 200.0,
					"balanceBefore": 5300.05,
					"betPoints": 100.0,
					"betPointsz": 0.0,
					"availableBet": 100.0,
					"userName": "4UNOTEPWIKK2",
					"result": "{\"result\":\"1,1,9\",\"poker\":{\"banker\":\"32-29-0\",\"player\":\"18-23-0\"}}",
					"betDetail": "{\"info\":\"{\"roadType\":10,\"table\":3}\",\"banker6\":100.0,\"banker6W\":200.0}",
					"ip": "125.59.7.225",
					"ext": "240917B121119",
					"isRevocation": 1,
					"currencyId": 4,
					"deviceType": 5,
					"roadid": 0,
					"pluginid": 0
				}
			]
		}
		""");

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, url.ToString())
						.Respond(HttpStatusCode.OK, JsonContent.Create(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		var actual = await sut.GetReportAsync(
			apiUrl: apiUrl,
			apiKey: apiKey,
			agentName: agentName)
			.ToArrayAsync()
			.ConfigureAwait(false)
			?? Enumerable.Empty<GetReportResult>();

		Assert.IsNotNull(actual);
		Assert.AreEqual("4UNOTEPWIKK2", actual.First().PlayerCode);
		Assert.AreEqual("22545327967", actual.First().ExternalRoundId);
		Assert.IsFalse(actual.First().IsVoid);
		Assert.IsTrue(actual.First().IsClosed);
		Assert.AreEqual(new DateTimeOffset(DateTime.Parse("2024-09-17 16:11:53"), TimeSpan.FromHours(8D)), actual.First().ExternalCreatedOn);
		Assert.AreEqual(new DateTimeOffset(DateTime.Parse("2024-09-17 16:12:18"), TimeSpan.FromHours(8D)), actual.First().ExternalClosedOn);
		Assert.AreEqual(100.0m, actual.First().Bet);
		Assert.AreEqual(100.0m, actual.First().ValidBet);
		Assert.AreEqual(200.0m, actual.First().Settle);
		Assert.IsNotNull(actual.First().AdditionalData);
		Assert.AreEqual("30102", actual.First().ExternalGameId);
	}

	[TestMethod]
	public async Task GameType_Is_1_Loss()

	{
		var apiUrl = new Uri("https://api.dg0.co/");
		var apiKey = "d39da9da731b427b9349b80d9d97f2e4";
		var agentName = "DGTE0105IH";

		var url = new Uri(apiUrl, $"/game/getReport/{agentName}");
		var response = JsonNode.Parse("""
		{
			"codeId": 0,
			"token": "a90274e0909e8ff3500962668677416d",
			"random": "1726584127372",
			"list": [
				{
					"id": 22545209117,
					"tableId": 94103,
					"shoeId": 35,
					"playId": 24,
					"lobbyId": 9,
					"gameType": 1,
					"gameId": 41,
					"memberId": 151899849,
					"parentId": 30281,
					"betTime": "2024-09-17 16:04:53",
					"calTime": "2024-09-17 16:05:12",
					"winOrLoss": 0.0,
					"balanceBefore": 5470.05,
					"betPoints": 570.0,
					"betPointsz": 0.0,
					"availableBet": 570.0,
					"userName": "4UNOTEPWIKK2",
					"result": "{\"result\":\"1,2,3\",\"poker\":{\"banker\":\"20-19-25\",\"player\":\"5-6-26\"}}",
					"betDetail": "{\"info\":\"{\"roadType\":11,\"table\":3}\",\"player\":460.0,\"tie\":70,\"pPair\":20,\"bPair\":20}",
					"ip": "125.59.7.225",
					"ext": "2409175131829",
					"isRevocation": 1,
					"currencyId": 4,
					"deviceType": 5,
					"roadid": 0,
					"pluginid": 0
				}
			]
		}
		""");

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, url.ToString())
						.Respond(HttpStatusCode.OK, JsonContent.Create(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		var actual = await sut.GetReportAsync(
			apiUrl: apiUrl,
			apiKey: apiKey,
			agentName: agentName)
			.ToArrayAsync()
			.ConfigureAwait(false)
			?? Enumerable.Empty<GetReportResult>();

		Assert.IsNotNull(actual);
		Assert.AreEqual("4UNOTEPWIKK2", actual.First().PlayerCode);
		Assert.AreEqual("22545209117", actual.First().ExternalRoundId);
		Assert.IsFalse(actual.First().IsVoid);
		Assert.IsTrue(actual.First().IsClosed);
		Assert.AreEqual(new DateTimeOffset(DateTime.Parse("2024-09-17 16:04:53"), TimeSpan.FromHours(8D)), actual.First().ExternalCreatedOn);
		Assert.AreEqual(new DateTimeOffset(DateTime.Parse("2024-09-17 16:05:12"), TimeSpan.FromHours(8D)), actual.First().ExternalClosedOn);
		Assert.AreEqual(570.0m, actual.First().Bet);
		Assert.AreEqual(570.0m, actual.First().ValidBet);
		Assert.AreEqual(0.0m, actual.First().Settle);
		Assert.IsNotNull(actual.First().AdditionalData);
		Assert.AreEqual("94103", actual.First().ExternalGameId);
	}

	[TestMethod]
	public async Task GameType_Is_2_GameId_Is_2_DG红包()
	{
		var apiUrl = new Uri("https://api.dg0.co/");
		var apiKey = "d39da9da731b427b9349b80d9d97f2e4";
		var agentName = "DGTE0105IH";

		var url = new Uri(apiUrl, $"/game/getReport/{agentName}");
		var response = JsonNode.Parse("""
		{
			"codeId": 0,
			"token": "a90274e0909e8ff3500962668677416d",
			"random": "1726584127372",
			"list": [
				{
					"id": 22545211903,
					"gameType": 2,
					"gameId": 2,
					"memberId": 151899849,
					"parentId": 30281,
					"betTime": "2024-09-17 16:05:03",
					"balanceBefore": 4900.05,
					"betPoints": 10.0,
					"userName": "4UNOTEPWIKK2",
					"betDetail": "{\"userName\":\"DG红包\",\"type\":1,\"apiId\":1,\"points\":1424.0,\"count\":25,\"currencyId\":4,\"currencyRate\":1.0879,\"message\":\"庆中秋佳节\",\"minAmount\":5.0,\"maxAmount\":1000.0}",
					"ip": "125.59.7.225",
					"ext": "22545211442",
					"isRevocation": 1,
					"currencyId": 4
				}
			]
		}
		""");

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, url.ToString())
						.Respond(HttpStatusCode.OK, JsonContent.Create(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		var actual = await sut.GetReportAsync(
			apiUrl: apiUrl,
			apiKey: apiKey,
			agentName: agentName)
			.ToArrayAsync()
			.ConfigureAwait(false)
			?? Enumerable.Empty<GetReportResult>();

		Assert.IsNotNull(actual);
		Assert.AreEqual("4UNOTEPWIKK2", actual.First().PlayerCode);
		Assert.AreEqual("22545211903", actual.First().ExternalRoundId);
		Assert.IsFalse(actual.First().IsVoid);
		Assert.IsTrue(actual.First().IsClosed);
		Assert.AreEqual(new DateTimeOffset(DateTime.Parse("2024-09-17 16:05:03"), TimeSpan.FromHours(8D)), actual.First().ExternalCreatedOn);
		Assert.AreEqual(new DateTimeOffset(DateTime.Parse("2024-09-17 16:05:03"), TimeSpan.FromHours(8D)), actual.First().ExternalClosedOn);
		Assert.AreEqual(0.0m, actual.First().Bet);
		Assert.AreEqual(0.0m, actual.First().ValidBet);
		Assert.AreEqual(10.0m, actual.First().Settle);
		Assert.IsNotNull(actual.First().AdditionalData);
		Assert.AreEqual("2_2", actual.First().ExternalGameId);
	}

	[TestMethod]
	public async Task GameType_Is_2_GameId_Is_5_博餅活動()
	{
		var apiUrl = new Uri("https://api.dg0.co/");
		var apiKey = "d39da9da731b427b9349b80d9d97f2e4";
		var agentName = "DGTE0105IH";

		var url = new Uri(apiUrl, $"/game/getReport/{agentName}");
		var response = JsonNode.Parse("""
		{
			"codeId": 0,
			"token": "a90274e0909e8ff3500962668677416d",
			"random": "1726584127372",
			"list": [
				{
					"id": 8873437184,
					"gameType": 2,
					"gameId": 5,
					"memberId": 56519780,
					"parentId": 12827,
					"betTime": "2021-09-19 11:48:08",
					"balanceBefore": 2193.5,
					"betPoints": 37.3,
					"userName": "HOIN10587788",
					"betDetail": "{\"result\":\"435525\",\"username\":\"HOIN10587788\",\"type\":1,\"amount\":37.3}",
					"ip": "114.42.57.70",
					"ext": "20210919",
					"isRevocation": 1,
					"currencyId": 8
				}
			]
		}
		""");

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, url.ToString())
						.Respond(HttpStatusCode.OK, JsonContent.Create(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		var actual = await sut.GetReportAsync(
			apiUrl: apiUrl,
			apiKey: apiKey,
			agentName: agentName)
			.ToArrayAsync()
			.ConfigureAwait(false)
			?? Enumerable.Empty<GetReportResult>();

		Assert.IsNotNull(actual);
		Assert.AreEqual("HOIN10587788", actual.First().PlayerCode);
		Assert.AreEqual("8873437184", actual.First().ExternalRoundId);
		Assert.IsFalse(actual.First().IsVoid);
		Assert.IsTrue(actual.First().IsClosed);
		Assert.AreEqual(new DateTimeOffset(DateTime.Parse("2021-09-19 11:48:08"), TimeSpan.FromHours(8D)), actual.First().ExternalCreatedOn);
		Assert.AreEqual(new DateTimeOffset(DateTime.Parse("2021-09-19 11:48:08"), TimeSpan.FromHours(8D)), actual.First().ExternalClosedOn);
		Assert.AreEqual(0.0m, actual.First().Bet);
		Assert.AreEqual(0.0m, actual.First().ValidBet);
		Assert.AreEqual(37.3m, actual.First().Settle);
		Assert.IsNotNull(actual.First().AdditionalData);
		Assert.AreEqual("2_5", actual.First().ExternalGameId);
	}
}