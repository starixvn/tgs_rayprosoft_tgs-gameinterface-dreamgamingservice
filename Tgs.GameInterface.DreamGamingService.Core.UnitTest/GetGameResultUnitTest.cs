using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.DreamGamingService.Core.UnitTest;

[TestClass]
public class GetGameResultUnitTest
{
	[TestMethod]
	public async Task 游戏注单解析入口成功()
	{
		var videoGameApiUrl = new Uri("https://ag.dg66.info/");
		var agentName = "DGTE0105IH";
		var language = "en";
		var id = "13540803433";

		var url = new Uri(videoGameApiUrl, $"/ag/result/getAgentFix.html?username={agentName}");
		var response = "\"3213\"";
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Get, url.ToString())
						.Respond(HttpStatusCode.OK, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		var actual = await sut
			.GetGameResultAsync(
				videoGameApiUrl: videoGameApiUrl,
				agentName: agentName,
				language: language,
				id: id)
			.ConfigureAwait(false);

		Assert.IsFalse(string.IsNullOrWhiteSpace(actual));
		Assert.AreEqual("https://ag.dg66.info/ag/result/result.html?language=en&agentFix=3213&id=13540803433", actual);
	}
}
