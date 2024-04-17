using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.DreamGamingService.Core.UnitTest;

[TestClass]
public class TransferUnitTest
{
	[TestMethod]
	public async Task 會員存款成功()
	{
		var apiUrl = new Uri("https://api.dg0.co/");
		var apiKey = "d39da9da731b427b9349b80d9d97f2e4";
		var agentName = "DGTE0105IH";
		var username = "4ucmqplxb029";
		var refTransactionCode = "7a7147218501000000010000af506b2c";
		var amount = 1000M;
		var url = new Uri(apiUrl, $"/account/transfer/{agentName}");
		var response = "{\"codeId\":0,\"token\":\"64910df0933836dd856e88f226b49363\",\"random\":\"1672910644097\",\"data\":\"7a7147218501000000010000af506b2c\",\"member\":{\"username\":\"4UCMQPLXB029\",\"balance\":1000.0}}";
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, url.ToString())
						.Respond(HttpStatusCode.OK, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		var actual = await sut
			.TransferAsync(
				apiUrl: apiUrl,
				apiKey: apiKey,
				agentName: agentName,
				username: username,
				refTransactionCode: refTransactionCode,
				amount: amount)
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.AreEqual(1000M, actual.Balance);
		Assert.AreEqual("7a7147218501000000010000af506b2c", actual.RefTransactionCode);
		Assert.AreEqual("7a7147218501000000010000af506b2c", actual.TransactionId);
	}

	[TestMethod]
	public async Task 會員取款成功()
	{
		var apiUrl = new Uri("https://api.dg0.co/");
		var apiKey = "d39da9da731b427b9349b80d9d97f2e4";
		var agentName = "DGTE0105IH";
		var username = "4ucmqplxb029";
		var refTransactionCode = "7a3f35bf271247b998f6750d448ed757";
		var amount = -100M;
		var url = new Uri(apiUrl, $"/account/transfer/{agentName}");
		var response = "{\"codeId\":0,\"token\":\"44f9e76dc953ba7d5f3c4ad9f0963a7a\",\"random\":\"1672970221985\",\"data\":\"7a3f35bf271247b998f6750d448ed757\",\"member\":{\"username\":\"4UCMQPLXB029\",\"balance\":900.0}}";
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, url.ToString())
						.Respond(HttpStatusCode.OK, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		var actual = await sut
			.TransferAsync(
				apiUrl: apiUrl,
				apiKey: apiKey,
				agentName: agentName,
				username: username,
				refTransactionCode: refTransactionCode,
				amount: amount)
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.AreEqual(900M, actual.Balance);
		Assert.AreEqual("7a3f35bf271247b998f6750d448ed757", actual.RefTransactionCode);
		Assert.AreEqual("7a3f35bf271247b998f6750d448ed757", actual.TransactionId);
	}
}
