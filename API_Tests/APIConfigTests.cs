using NUnit.Framework;
using API_Handler;
using Newtonsoft.Json;

namespace API_Tests
{
    [TestFixture]
    public class APIConfigTests
    {
        private const string _validConfigPath = "valid_api_config.json";
        private const string _invalidConfigPath = "invalid_api_config.json";
        private const string MissingConfigPath = "missing_api_config.json";

        [SetUp]
        public void Setup()
        {
            // Create a valid JSON file for testing
            File.WriteAllText(_validConfigPath, @"
        {
            ""BaseUrl"": ""https://test-api.com/"",
            ""AuthToken"": ""test-token"",
            ""Endpoints"": { ""GetFacts"": ""facts/"" }
        }");

            // Create an invalid JSON file
            File.WriteAllText(_invalidConfigPath, "{ Invalid JSON }");
        }

        //[Test]
        //public void LoadConfig_InvalidJson_ThrowsException()
        //{
        //    Assert.Throws<JsonReaderException>(() => APIConfig.LoadConfig(_invalidConfigPath));
        //}

        //[Test]
        //public void LoadConfig_MissingFile_ThrowsFileNotFoundException()
        //{
        //    Assert.Throws<FileNotFoundException>(() => APIConfig.LoadConfig(MissingConfigPath));
        //}
    }
}