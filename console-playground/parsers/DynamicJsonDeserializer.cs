using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;
using NLua;

namespace Main.parsers
{
    public class ObjectToInferredTypesConverter : JsonConverter<object>
    {
        public override object Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) => reader.TokenType switch
        {
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.Number when reader.TryGetInt64(out long l) => l,
            JsonTokenType.Number => reader.GetDouble(),
            JsonTokenType.String when reader.TryGetDateTime(out DateTime datetime) => datetime,
            JsonTokenType.String => reader.GetString()!,
            _ => JsonDocument.ParseValue(ref reader).RootElement.Clone()
        };

        public override void Write(
            Utf8JsonWriter writer,
            object objectToWrite,
            JsonSerializerOptions options) =>
            JsonSerializer.Serialize(writer, objectToWrite, objectToWrite.GetType(), options);
    }
    public record Leaderboard
    {
        public string LuaScript { get; set; }
    }
    public record Score
    {
        public string JsonValue { get; set; }
    }
    public static class DynamicJsonDeserializer
    {
        private static double GetScore(string scoreJson, Lua state, string luascript)
        {
            // 1. configure serialization options
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            options.Converters.Add(new ObjectToInferredTypesConverter());
            // 2. Get object from score JSON
            dynamic data = JsonSerializer.Deserialize<ExpandoObject>(scoreJson, options);
            // var obj = JsonDocument.Parse(scoreJson);
            // 3. create global value in Lua state from data
            state["score"] = data;
            // 4. execute script
            var res = (double)(long)state.DoString(luascript)[0];
            return res;
        }
        public static List<double> TestDeserialize()
        {
            var scores = new List<Score>
            {
                new ()
                {
                    JsonValue = """
                                {
                                  "First": 2,
                                  "Second": 4
                                }
                                """
                },
                new ()
                {
                    JsonValue = """
                                {
                                  "First": 3,
                                  "Second": 6
                                }
                                """
                },
                new ()
                {
                    JsonValue = """
                                {
                                  "First": 4,
                                  "Second": 8
                                }
                                """
                }
            };

            var leaderboard = new Leaderboard()
            {
                LuaScript = "return score.First + score.Second"
            };

            var state = new Lua();
            state.DoString("import = function () end");

            var results = scores.Select(score => GetScore(score.JsonValue, state, leaderboard.LuaScript));
            return results.ToList();
        }
    }
}