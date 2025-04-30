using API_Handler;
using System;
using System.Net.Http;
using System.Threading.Tasks;

public class APIManager
{
    private static readonly HttpClient client = new HttpClient();
    private static APIConfig config = APIConfig.LoadConfig();

	/// <summary>
	/// Sends a request to the configured API.
	/// </summary>
	/// <param name="endpointKey">Key name of the API endpoint in config</param>
	/// <param name="method">HTTP method (GET, POST, etc.)</param>
	/// <param name="jsonPayload">Optional JSON payload</param>
	/// <returns>API response as a string</returns>
	public static async Task<string> SendRequest(string endpointKey, HttpMethod method, string jsonPayload = null)
    {
        if (!config.Endpoints.ContainsKey(endpointKey))
            throw new ArgumentException($"Invalid API endpoint key: {endpointKey}");

        string url = $"{config.BaseUrl}{config.Endpoints[endpointKey]}";

        HttpRequestMessage request = new HttpRequestMessage(method, url);

        // Attach authentication token if available
        if (!string.IsNullOrEmpty(config.AuthToken))
            request.Headers.Add("Authorization", $"Bearer {config.AuthToken}");

        if (!string.IsNullOrEmpty(jsonPayload))
            request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}