using API_Handler;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace API_Handler
{
    public class APIManager
    {
        private readonly HttpClient _httpClient;
        private readonly APIConfig _config;

        public APIManager(HttpClient httpClient, APIConfig config)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

		/// <summary>
		/// Sends a request to the configured API.
		/// </summary>
		public async Task<string> SendRequest(string endpointKey, HttpMethod method, string jsonPayload = null)
		{
			if (!_config.Endpoints.ContainsKey(endpointKey))
				throw new ArgumentException($"Invalid API endpoint key: {endpointKey}");

			string url = $"{_config.BaseUrl}{_config.Endpoints[endpointKey]}";
			HttpRequestMessage request = new HttpRequestMessage(method, url);

			if (!string.IsNullOrEmpty(_config.AuthToken))
				request.Headers.Add("Authorization", $"Bearer {_config.AuthToken}");

			if (!string.IsNullOrEmpty(jsonPayload))
				request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

			try
			{
				HttpResponseMessage response = await _httpClient.SendAsync(request);
				response.EnsureSuccessStatusCode();

				string responseBody = await response.Content.ReadAsStringAsync();
				JsonConvert.DeserializeObject<dynamic>(responseBody); // Test JSON validity

				return responseBody;
			}
			catch (Exception ex)
			{
				return HandleError(ex); // Error handling
			}
		}

		private string HandleError(Exception ex)
		{
			if (ex is HttpRequestException)
				return $"Error: HTTP request failed - {ex.Message}";
			else if (ex is JsonException)
				return "Error: Invalid JSON response.";
			else
				return $"Error: {ex.GetType().Name} - {ex.Message}";
		}
	}
}
