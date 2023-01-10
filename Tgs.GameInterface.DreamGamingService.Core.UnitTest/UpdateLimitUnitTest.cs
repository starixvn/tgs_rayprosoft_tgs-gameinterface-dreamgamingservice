using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.DreamGamingService.Core.UnitTest;

[TestClass]
public class UpdateLimitUnitTest
{
    [TestMethod]
    public async Task 修改會員限紅組成功()
    {
        var apiUrl = new Uri("https://api.dg0.co/");
        var apiKey = "d39da9da731b427b9349b80d9d97f2e4";
        var agentName = "DGTE0105IH";
        var username = "4ucmqplxb029";

        var url = new Uri(apiUrl, $"/game/updateLimit/{agentName}");
        var response = "{\"codeId\":0,\"token\":\"785010c681807b7da36d592f475a1ea7\",\"random\":\"1673319303595\"}";
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut.UpdateLimitAsync(
            apiUrl: apiUrl,
            apiKey: apiKey,
            agentName: agentName,
            username: username,
            tableLimit: "B")
        .ConfigureAwait(false);

        Assert.IsTrue(actual);
    }
}
