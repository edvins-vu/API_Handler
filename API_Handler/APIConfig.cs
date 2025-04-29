using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace API_Handler
{
    public class APIConfig
    {
        public string BaseUrl { get; set; }
        public string AuthToken { get; set; }
        public Dictionary<string, string> Endpoints { get; set; }

        /// <summary>
        /// Loads API configuration from a JSON file.
        /// </summary>
        public static APIConfig LoadConfig(string filePath = "api_config.json")
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"API config file not found: {filePath}");

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<APIConfig>(json);
        }
    }
}