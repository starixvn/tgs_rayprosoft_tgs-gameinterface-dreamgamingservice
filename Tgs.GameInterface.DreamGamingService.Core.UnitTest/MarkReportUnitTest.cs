using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.DreamGamingService.Core.UnitTest;

[TestClass]
public class MarkReportUnitTest
{
	[TestMethod]
	public async Task 标记已抓取注单成功()
	{
		var apiUrl = new Uri("https://api.dg0.co/");
		var apiKey = "d39da9da731b427b9349b80d9d97f2e4";
		var agentName = "DGTE0105IH";
		var externalRoundIds = new List<string> { "13562946013" };

		var url = new Uri(apiUrl, $"/game/markReport/{agentName}");
		var response = "{\"codeId\":0,\"token\":\"ea382bdef52abc1a2cc0cb7e09423505\",\"random\":\"1673243376362\"}";
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, url.ToString())
						.Respond(HttpStatusCode.OK, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		var actual = await sut
			.MarkReportAsync(
				apiUrl: apiUrl,
				apiKey: apiKey,
				agentName: agentName,
				externalRoundIds: externalRoundIds)
			.ConfigureAwait(false);

		Assert.IsTrue(actual);
	}
}
