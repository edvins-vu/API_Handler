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

        private static APIConfig _instance;

        /// <summary>
        /// Loads API configuration from a JSON file.
        /// </summary>
        /// <returns>Returns APIConfig instance</returns>
        public static APIConfig LoadConfig(string filePath = "api_config.json")
        {
            if (_instance != null) return _instance;

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"API config file not found: {filePath}");

            string json = File.ReadAllText(filePath);
            _instance = JsonConvert.DeserializeObject<APIConfig>(json);
            return _instance;
        }
    }
}
