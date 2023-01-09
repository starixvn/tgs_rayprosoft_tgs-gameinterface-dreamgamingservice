using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.DreamGamingService.Core.UnitTest;

[TestClass]
public class OnlineReportUnitTest
{
    [TestMethod]
    public async Task 获取当前代理下在DG在线会员信息成功()
    {
        var apiUrl = new Uri("https://api.dg0.co/");
        var apiKey = "d39da9da731b427b9349b80d9d97f2e4";
        var agentName = "DGTE0105IH";

        var url = new Uri(apiUrl, $"/user/onlineReport/{agentName}");
        var response = "{\"codeId\":0,\"token\":\"f1e31ad4b41c0141200134fca5892ac8\",\"random\":\"1673076414659\",\"list\":[{\"username\":\"4UCMQPLXB029\",\"nickname\":\"29\",\"currencyName\":\"CNY\",\"ip\":\"114.33.41.237\",\"device\":\"1\",\"login\":\"2023-01-07 11:58:30\",\"memberId\":94030972}]}";
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut
            .OnlineReportAsync(
                apiUrl: apiUrl,
                apiKey: apiKey,
                agentName: agentName)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Length > 0);
    }
}