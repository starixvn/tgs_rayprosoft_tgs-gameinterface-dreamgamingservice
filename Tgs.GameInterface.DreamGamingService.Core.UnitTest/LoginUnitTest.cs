using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.DreamGamingService.Core.UnitTest;

[TestClass]
public class LoginUnitTest
{
    [TestMethod]
    public async Task 會員登入成功()
    {
        var apiUrl = new Uri("https://api.dg0.co/");
        var apiKey = "d39da9da731b427b9349b80d9d97f2e4";
        var agentName = "DGTE0105IH";
        var username = "4ucmqplxb029";
        var password = "DGTFP@ssw0rd";

        var url = new Uri(apiUrl, $"/user/login/{agentName}");
        var response = "{\"codeId\":0,\"token\":\"05b4cf398a30421bb2cbfb00f2a817b1\",\"lang\":\"en\",\"random\":\"1672976072649\",\"list\":[\"https://login.maidaosha.com/pc-index/direct1.html?token=\",\"https://login.maidaosha.com/pc-index/direct1.html?token=\",\"http://app.wechat668.com/download/cn.html?t=\"],\"domains\":\"{\\n\\t\\\"file\\\": \\\"https://dg-asia.ruihejade.com/\\\",\\n\\t\\\"tcp\\\": \\\"appa.gztsshuju.com:8860\\\",\\n\\t\\\"game_wss\\\": \\\"wss://appa.gztsshuju.com:8871\\\",\\n\\t\\\"freeToken\\\":\\\"https://login.maidaosha.com/apidata/game/h5/\\\",\\n\\t\\\"login\\\": \\\"https://login.maidaosha.com/apidata/\\\",\\n\\t\\\"chat_wss\\\": \\\"wss://appa.gztsshuju.com:8873\\\",\\n\\t\\\"back_img\\\": \\\"https://tupian-dg.dg0.co/#https://tupian-ct.dg0.co/#https://tupian.nanhai666.com/image/#https://tupian-cp.dg0.co/\\\",\\n\\t\\\"back_video\\\": \\\"https://videoback-dg.dg0.co/#https://videoback-ct.dg0.co/#https://playback.nanhai66.com/#https://videoback-cp.dg0.co/\\\",\\n\\t\\\"back_appvideo\\\": \\\"rtmp://play-ct.dg0.co/vod/mp4#rtmp://play-ct.dg0.co:1936/vod/mp4#rtmp://playback.nanhai66.com/vod/mp4\\\",\\n\\t\\\"dealerFile\\\": \\\"https://dg-asia.ruihejade.com/#https://dg-asia.ruihejade.com/#https://nh-asia.17xiangqu.com/\\\",\\n\\t\\\"bgmVersion\\\": \\\"2\\\",\\n\\t\\\"bobingrobot\\\": \\\"https://bobingrobot.dg75l.com/api/ranking\\\"\\n}\\n\"}";
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut
            .LoginAsync(
                apiUrl: apiUrl,
                apiKey: apiKey,
                agentName: agentName,
                username: username,
                password: password,
                isMobile: false,
                lang: "en")
            .ConfigureAwait(false);

        Assert.AreEqual("https://login.maidaosha.com/pc-index/direct1.html?token=05b4cf398a30421bb2cbfb00f2a817b1&language=en", actual);
    }
}