using System.Net;
using Newtonsoft.Json;

namespace APIHandler_Tests.UniTests
{
	public class APIManagerTests: IClassFixture<APIManagerTestSetup>
	{
		private readonly APIManagerTestSetup _setup;

		public APIManagerTests(APIManagerTestSetup setup)
		{
			_setup = setup;
		}

		[Fact]
		public async Task SendRequest_ValidEndpoint_ReturnsExpectedResponse()
		{
			// Arrange
			_setup.SetupMockResponse(HttpStatusCode.OK, "{\"message\": \"Success\"}");

			// Act
			var result = await _setup.ApiManager.SendRequest("GetFacts", HttpMethod.Get);

			// Assert
			Assert.Contains("Success", result);
		}

		[Fact]
		public async Task SendRequest_GetFactsEndPoint_ReturnsFacts()
		{
			// Arrange
			_setup.SetupMockResponse(HttpStatusCode.OK,
				@"
				{
					""data"": [
						{
							""id"": ""f301ae94-3382-4d42-9a37-8a137e21a9ed"",
							""type"": ""fact"",
							""attributes"": {
								""body"": ""Example fact text""
							}
						}
					]
				}");

			// Act
			var result = await _setup.ApiManager.SendRequest("GetFacts", HttpMethod.Get);

			var responseObject = JsonConvert.DeserializeObject<dynamic>(result);

			// Assert root structure
			Assert.NotNull(responseObject);
			Assert.NotEmpty(responseObject.data);

			// Assert each element in the array has required properties
			var firstFact = responseObject.data[0];
			Assert.NotNull(firstFact.id);
			Assert.Equal("fact", (string)firstFact.type);
			Assert.NotNull(firstFact.attributes);
			Assert.NotNull(firstFact.attributes.body);
		}

		[Fact]
		public async Task SendRequest_InvalidEndpointKey_ThrowsArgumentException()
		{
			// Arrange
			_setup.SetupMockResponse(HttpStatusCode.NotFound, "Not Found");

			// Act & Assert
			await Assert.ThrowsAsync<ArgumentException>(() => _setup.ApiManager.SendRequest("InvalidKey", HttpMethod.Get));
		}

		[Fact]
		public async Task SendRequest_ApiReturns404_ReturnsErrorMessage()
		{
			// Arrange
			_setup.SetupMockResponse(HttpStatusCode.NotFound, "Not Found");

			// Act
			var result = await _setup.ApiManager.SendRequest("GetFacts", HttpMethod.Get);

			// Assert
			Assert.Contains("Error:", result);
		}

		[Fact]
		public async Task SendRequest_ApiReturns500_ReturnsErrorMessage()
		{
			// Arrange
			_setup.SetupMockResponse(HttpStatusCode.InternalServerError, "Internal Server Error");

			// Act
			var result = await _setup.ApiManager.SendRequest("GetFacts", HttpMethod.Get);

			// Assert
			Assert.Contains("Error:", result);
		}

		[Fact]
		public async Task SendRequest_ApiReturnsInvalidJson_ThrowsJsonException()
		{
			// Arrange
			_setup.SetupMockResponse(HttpStatusCode.OK, "Invalid JSON");

			// Act
			var result = await _setup.ApiManager.SendRequest("GetFacts", HttpMethod.Get);

			// Assert
			Assert.Contains("Error:", result); // Should return an error message for invalid JSON
		}

		[Fact]
		public async Task SendRequest_IncludesAuthorizationHeader_WhenAuthTokenExists()
		{
			// Arrange
			string expectedToken = _setup.Config.AuthToken; // Get the token from config
			_setup.SetupMockResponse(HttpStatusCode.OK, "{\"message\":\"Success\"}", expectedToken);

			// Act
			var result = await _setup.ApiManager.SendRequest("GetFacts", HttpMethod.Get);

			// Assert
			Assert.Contains("Success", result);
		}

		[Fact]
		public async Task SendRequest_MissingAuthToken_ReturnsUnauthorized()
		{
			// Arrange
			_setup.SetupMockResponse(HttpStatusCode.Unauthorized, "{\"error\":\"Unauthorized\"}", "valid_token");

			// Act
			var result = await _setup.ApiManager.SendRequest("GetFacts", HttpMethod.Get);

			// Assert
			Assert.Contains("Error:", result); // Should return an unauthorized error message
		}

		[Fact]
		public async Task SendRequest_ApiReturnsEmptyResponse_ReturnsErrorMessage()
		{
			// Arrange
			_setup.SetupMockResponse(HttpStatusCode.OK, ""); // Simulate empty response

			// Act
			var result = await _setup.ApiManager.SendRequest("GetFacts", HttpMethod.Get);

			// Assert
			Assert.NotNull(result);
			Assert.Contains("Error:", result); // Should return a meaningful error
		}

		[Fact]
		public async Task SendRequest_ApiReturnsUnexpectedFormat_ReturnsErrorMessage()
		{
			// Arrange
			_setup.SetupMockResponse(HttpStatusCode.OK, @"{ ""message"": ""Unexpected format"" }"); // Incorrect structure

			// Act
			var result = await _setup.ApiManager.SendRequest("GetFacts", HttpMethod.Get);

			// Assert
			Assert.NotNull(result);
			Assert.Contains("Error:", result); // API should recognize it's not the expected structure
		}
	}
}