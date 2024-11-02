using System.Text.Json;
using BenchmarkDotNet.Running;
using Benchmarks;
using Main.parsers;

namespace Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Benchmarks
            // var benchmarkSummary = BenchmarkRunner.Run<LinqQueryBenchmark>();
            
            // CSV testing
            Console.WriteLine("CSV");
            Console.WriteLine(JsonSerializer.Serialize(CSVHelperParser.SimpleParse("test.csv")));
            
            // Lua testing
            Console.WriteLine("LUA");
            Console.WriteLine(NLuaParser.SimpleMathParse());
            
            // Func testing
            Console.WriteLine("FUNC");
            Console.WriteLine(JsonSerializer.Serialize(TestingFuncs.SimpleFunc()));
        }
    }
}