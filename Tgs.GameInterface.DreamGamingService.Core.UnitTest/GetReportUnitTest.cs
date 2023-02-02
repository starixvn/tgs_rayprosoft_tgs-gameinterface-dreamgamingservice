using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.DreamGamingService.Core.UnitTest;

[TestClass]
public class GetReportUnitTest
{
    [TestMethod]
    public async Task 抓取注單報表成功()
    {
        var apiUrl = new Uri("https://api.dg0.co/");
        var apiKey = "d39da9da731b427b9349b80d9d97f2e4";
        var agentName = "DGTE0105IH";

        var url = new Uri(apiUrl, $"/game/getReport/{agentName}");
        var response = "{\"codeId\":0,\"token\":\"e79947b66bc740be5261299d81286a20\",\"random\":\"1673063768996\",\"list\":[{\"id\":13531234050,\"tableId\":10106,\"shoeId\":20,\"playId\":41,\"lobbyId\":1,\"gameType\":1,\"gameId\":1,\"memberId\":94030972,\"parentId\":23741,\"betTime\":\"2023-01-06 15:04:06\",\"calTime\":\"2023-01-06 15:04:34\",\"winOrLoss\":0.0,\"balanceBefore\":900.0,\"betPoints\":25.0,\"betPointsz\":0.0,\"availableBet\":25.0,\"userName\":\"4UCMQPLXB029\",\"result\":\"{\\\"result\\\":\\\"1,1,9\\\",\\\"poker\\\":{\\\"banker\\\":\\\"9-11-0\\\",\\\"player\\\":\\\"19-50-0\\\"}}\",\"betDetail\":\"{\\\"player\\\":25.0}\",\"ip\":\"114.33.41.237\",\"ext\":\"230106B061050\",\"isRevocation\":1,\"currencyId\":1,\"deviceType\":1,\"roadid\":0,\"pluginid\":0},{\"id\":13531241080,\"tableId\":10106,\"shoeId\":20,\"playId\":42,\"lobbyId\":1,\"gameType\":1,\"gameId\":1,\"memberId\":94030972,\"parentId\":23741,\"betTime\":\"2023-01-06 15:04:44\",\"calTime\":\"2023-01-06 15:05:15\",\"winOrLoss\":48.75,\"balanceBefore\":875.0,\"betPoints\":25.0,\"betPointsz\":0.0,\"availableBet\":23.75,\"userName\":\"4UCMQPLXB029\",\"result\":\"{\\\"result\\\":\\\"1,2,3\\\",\\\"poker\\\":{\\\"banker\\\":\\\"24-29-0\\\",\\\"player\\\":\\\"7-44-47\\\"}}\",\"betDetail\":\"{\\\"banker\\\":25.0,\\\"bankerW\\\":48.75}\",\"ip\":\"114.33.41.237\",\"ext\":\"230106B061051\",\"isRevocation\":1,\"currencyId\":1,\"deviceType\":1,\"roadid\":0,\"pluginid\":0},{\"id\":13540788882,\"tableId\":10103,\"shoeId\":18,\"playId\":17,\"lobbyId\":1,\"gameType\":1,\"gameId\":1,\"memberId\":94030972,\"parentId\":23741,\"betTime\":\"2023-01-07 11:28:05\",\"calTime\":\"2023-01-07 11:28:24\",\"winOrLoss\":0.0,\"balanceBefore\":898.75,\"betPoints\":25.0,\"betPointsz\":0.0,\"availableBet\":25.0,\"userName\":\"4UCMQPLXB029\",\"result\":\"{\\\"result\\\":\\\"5,1,9\\\",\\\"poker\\\":{\\\"banker\\\":\\\"47-7-0\\\",\\\"player\\\":\\\"27-21-0\\\"}}\",\"betDetail\":\"{\\\"banker\\\":25.0}\",\"ip\":\"114.33.41.237\",\"ext\":\"230107B030915\",\"isRevocation\":1,\"currencyId\":1,\"deviceType\":1,\"roadid\":0,\"pluginid\":0},{\"id\":13540793340,\"tableId\":10103,\"shoeId\":18,\"playId\":18,\"lobbyId\":1,\"gameType\":1,\"gameId\":1,\"memberId\":94030972,\"parentId\":23741,\"betTime\":\"2023-01-07 11:28:36\",\"calTime\":\"2023-01-07 11:28:53\",\"winOrLoss\":48.75,\"balanceBefore\":873.75,\"betPoints\":25.0,\"betPointsz\":0.0,\"availableBet\":23.75,\"userName\":\"4UCMQPLXB029\",\"result\":\"{\\\"result\\\":\\\"1,1,7\\\",\\\"poker\\\":{\\\"banker\\\":\\\"46-50-0\\\",\\\"player\\\":\\\"19-51-0\\\"}}\",\"betDetail\":\"{\\\"banker\\\":25.0,\\\"bankerW\\\":48.75}\",\"ip\":\"114.33.41.237\",\"ext\":\"230107B030916\",\"isRevocation\":1,\"currencyId\":1,\"deviceType\":1,\"roadid\":0,\"pluginid\":0},{\"id\":13540798348,\"tableId\":10103,\"shoeId\":18,\"playId\":19,\"lobbyId\":1,\"gameType\":1,\"gameId\":1,\"memberId\":94030972,\"parentId\":23741,\"betTime\":\"2023-01-07 11:29:10\",\"calTime\":\"2023-01-07 11:29:25\",\"winOrLoss\":48.75,\"balanceBefore\":897.5,\"betPoints\":25.0,\"betPointsz\":0.0,\"availableBet\":23.75,\"userName\":\"4UCMQPLXB029\",\"result\":\"{\\\"result\\\":\\\"2,2,6\\\",\\\"poker\\\":{\\\"banker\\\":\\\"34-47-0\\\",\\\"player\\\":\\\"25-37-42\\\"}}\",\"betDetail\":\"{\\\"banker\\\":25.0,\\\"bankerW\\\":48.75}\",\"ip\":\"114.33.41.237\",\"ext\":\"230107B030917\",\"isRevocation\":1,\"currencyId\":1,\"deviceType\":1,\"roadid\":0,\"pluginid\":0},{\"id\":13540803433,\"tableId\":10103,\"shoeId\":18,\"playId\":20,\"lobbyId\":1,\"gameType\":1,\"gameId\":1,\"memberId\":94030972,\"parentId\":23741,\"betTime\":\"2023-01-07 11:29:45\",\"calTime\":\"2023-01-07 11:30:00\",\"winOrLoss\":0.0,\"balanceBefore\":921.25,\"betPoints\":25.0,\"betPointsz\":0.0,\"availableBet\":25.0,\"userName\":\"4UCMQPLXB029\",\"result\":\"{\\\"result\\\":\\\"5,2,6\\\",\\\"poker\\\":{\\\"banker\\\":\\\"4-46-22\\\",\\\"player\\\":\\\"40-44-0\\\"}}\",\"betDetail\":\"{\\\"banker\\\":25.0}\",\"ip\":\"114.33.41.237\",\"ext\":\"230107B030918\",\"isRevocation\":1,\"currencyId\":1,\"deviceType\":1,\"roadid\":0,\"pluginid\":0}]}";
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut
            .GetReportAsync(
                apiUrl: apiUrl,
                apiKey: apiKey,
                agentName: agentName)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Length > 0);
    }
}
