using System.Text.Json;
using System.Text.Json.Nodes;

namespace Main.parsers
{
    public static class DynamicJsonDeserializer
    {
        public static void TestDeserialize()
        {
            string jsonString = """
                                {
                                  "Date": "2019-08-01T00:00:00",
                                  "Temperature": 25,
                                  "Summary": "Hot",
                                  "DatesAvailable": [
                                    "2019-08-01T00:00:00",
                                    "2019-08-02T00:00:00"
                                  ],
                                  "TemperatureRanges": {
                                      "Cold": {
                                          "High": 20,
                                          "Low": -10
                                      },
                                      "Hot": {
                                          "High": 60,
                                          "Low": 20
                                      }
                                  }
                                }
                                """;
            // Create a JsonNode DOM from a JSON string.
            JsonNode forecastNode = JsonNode.Parse(jsonString)!;
            
            // Get a typed value from a JsonNode.
            int temperatureInt = (int)forecastNode!["Temperature"]!;
            Console.WriteLine($"Value={temperatureInt}");

            // Get a typed value from a JsonNode by using GetValue<T>.
            temperatureInt = forecastNode!["Temperature"]!.GetValue<int>();
            Console.WriteLine($"TemperatureInt={temperatureInt}");
        }
    }
}