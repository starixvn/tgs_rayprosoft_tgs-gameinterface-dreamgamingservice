using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.DreamGamingService.Core.Exceptions;

namespace Tgs.GameInterface.DreamGamingService.Core.UnitTest;

[TestClass]
public class SignUpUnitTest
{
    [TestMethod]
    public async Task 創建玩家成功()
    {
        var apiUrl = new Uri("https://api.dg0.co/");
        var apiKey = "d39da9da731b427b9349b80d9d97f2e4";
        var agentName = "DGTE0105IH";
        var username = "4ucmqplxb029";
        var password = "DGTFP@ssw0rd";
        var currencyName = "CNY";

        var url = new Uri(apiUrl, $"/user/signup/{agentName}");
        var response = "{\"codeId\":0,\"token\":\"b2787b1d33ec41cfe289c8b6daeb8b25\",\"random\":\"1672909644020\",\"data\":\"A\"}";
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        await sut.SignUpAsync(
            apiUrl: apiUrl,
            apiKey: apiKey,
            agentName: agentName,
            username: username,
            password: password,
            currencyName: currencyName,
            tableLimit: "A",
            winLimit: 0)
        .ConfigureAwait(false);
    }

    [TestMethod]
    public async Task 玩家已經存在()
    {
        var apiUrl = new Uri("https://api.dg0.co/");
        var apiKey = "d39da9da731b427b9349b80d9d97f2e4";
        var agentName = "DGTE0105IH";
        var username = "4ucmqplxb029";
        var password = "DGTFP@ssw0rd";
        var currencyName = "CNY";

        var url = new Uri(apiUrl, $"/user/signup/{agentName}");
        var response = "{\"codeId\":116,\"token\":\"e649a5cff7ed245207a371ecd3889bfa\",\"random\":\"1673224967156\",\"data\":\"A\"}";
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        _ = await Assert
            .ThrowsExceptionAsync<GameProviderResponseErrorException>(() =>
                sut.SignUpAsync(
                    apiUrl: apiUrl,
                    apiKey: apiKey,
                    agentName: agentName,
                    username: username,
                    password: password,
                    currencyName: currencyName,
                    tableLimit: "A",
                    winLimit: 0)
                .AsTask())
            .ConfigureAwait(false);
    }
}
