using System.Net;
using API_Handler;
using Moq;
using Moq.Protected;

namespace APIHandler_Tests.UniTests
{
	public class APIManagerTestSetup
	{
		public Mock<HttpMessageHandler> MockHandler { get; private set; }
		public HttpClient HttpClient { get; private set; }
		public APIConfig Config { get; private set; }
		public APIManager ApiManager { get; private set; }

		public APIManagerTestSetup()
		{
			// Arrange
			MockHandler = new Mock<HttpMessageHandler>();
			HttpClient = new HttpClient(MockHandler.Object);
			Config = new APIConfig
			{
				BaseUrl = "https://dogapi.dog/api/v2/",
				Endpoints = new Dictionary<string, string> 
				{ 
					{ "GetFacts", "facts?limit=1" } 
				}
			};
			ApiManager = new APIManager(HttpClient, Config);
		}

		public void SetupMockResponse(HttpStatusCode statusCode, string content)
		{
			MockHandler
				.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = statusCode,
					Content = new StringContent(content)
				});
		}
	}
}