using System.IO;
using System.Text.Json;

public class TestConfiguration {
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
    public string Domain { get; set; }
    public string BadDomain { get; set; }
    public string SubDomain { get; set; }
    public string BadSubDomain { get; set; }

    private const string FilePath = "TestConfiguration.json";

    public static TestConfiguration LoadConfig() {
        if (!File.Exists(FilePath)) {
            return CreateDefaultConfig();
        }

        try {
            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<TestConfiguration>(json) ?? CreateDefaultConfig();
        }
        catch {
            return CreateDefaultConfig();
        }
    }

    private static TestConfiguration CreateDefaultConfig() {
        TestConfiguration defaultConfig = new TestConfiguration {
            ApiKey = "your_api_key",
            ApiSecret = "your_api_secret",
            Domain = "your_domain",
            BadDomain = "your_bad_domain",
            SubDomain = "your_subdomain",
            BadSubDomain = "your_bad_subdomain"
        };

        string json = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
        return defaultConfig;
    }
}
