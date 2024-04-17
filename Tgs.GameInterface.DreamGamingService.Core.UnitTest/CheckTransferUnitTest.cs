using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.DreamGamingService.Core.UnitTest;

[TestClass]
public class CheckTransferUnitTest
{
	[TestMethod]
	public async Task 檢查存取款成功()
	{
		var apiUrl = new Uri("https://api.dg0.co/");
		var apiKey = "d39da9da731b427b9349b80d9d97f2e4";
		var agentName = "DGTE0105IH";
		var refTransactionCode = "7a7147218501000000010000af506b2c";

		var url = new Uri(apiUrl, $"/account/checkTransfer/{agentName}");
		var response = "{\"codeId\":0,\"token\":\"63bafc0b53b45420340b6b75e98fc48f\",\"random\":\"1672970876759\"}";
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, url.ToString())
						.Respond(HttpStatusCode.OK, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		var actual = await sut
			.CheckTransferAsync(
				apiUrl: apiUrl,
				apiKey: apiKey,
				agentName: agentName,
				refTransactionCode: refTransactionCode)
			.ConfigureAwait(false);

		Assert.IsTrue(actual);
	}

	[TestMethod]
	public async Task 檢查存取款失敗()
	{
		var apiUrl = new Uri("https://api.dg0.co/");
		var apiKey = "d39da9da731b427b9349b80d9d97f2e4";
		var agentName = "DGTE0105IH";
		var refTransactionCode = "123";

		var url = new Uri(apiUrl, $"/account/checkTransfer/{agentName}");
		var response = "{\"codeId\":324,\"token\":\"5673521f0ddea7b3d97c8ed95dad6667\",\"random\":\"1673231670437\"}";
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, url.ToString())
						.Respond(HttpStatusCode.OK, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		var actual = await sut
			.CheckTransferAsync(
				apiUrl: apiUrl,
				apiKey: apiKey,
				agentName: agentName,
				refTransactionCode: refTransactionCode)
			.ConfigureAwait(false);

		Assert.IsFalse(actual);
	}
}
