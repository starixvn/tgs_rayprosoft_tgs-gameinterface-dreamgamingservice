using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.DreamGamingService.Core.UnitTest;

[TestClass]
public class OfflineUnitTest
{
    [TestMethod]
    public async Task 踢人成功()
    {
        var apiUrl = new Uri("https://api.dg0.co/");
        var apiKey = "d39da9da731b427b9349b80d9d97f2e4";
        var agentName = "DGTE0105IH";
        var memberId = 94030972L;

        var url = new Uri(apiUrl, $"/user/offline/{agentName}");
        var response = "{\"codeId\":0,\"token\":\"49d9bbc0dd5a5a419cb8579f1c075754\",\"random\":\"1673079242315\",\"list\":[94030972]}";
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut
            .OfflineAsync(
                apiUrl: apiUrl,
                apiKey: apiKey,
                agentName: agentName,
                memberId: memberId)
            .ConfigureAwait(false);

        Assert.IsTrue(actual);
    }
}
