using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.DreamGamingService.Core.UnitTest;

[TestClass]
public class GetBalanceUnitTest
{
    [TestMethod]
    public async Task 獲取會員餘額成功()
    {
        var apiUrl = new Uri("https://api.dg0.co/");
        var apiKey = "d39da9da731b427b9349b80d9d97f2e4";
        var agentName = "DGTE0105IH";
        var username = "4ucmqplxb029";

        var url = new Uri(apiUrl, $"/user/getBalance/{agentName}");
        var response = "{\"codeId\":0,\"token\":\"379c180485e261343151f409ad7dcc96\",\"random\":\"1672910754230\",\"member\":{\"username\":\"4UCMQPLXB029\",\"balance\":1000.0}}";
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        _ = await sut
            .GetBalanceAsync(
                apiUrl: apiUrl,
                apiKey: apiKey,
                agentName: agentName,
                username: username)
            .ConfigureAwait(false);
    }
}