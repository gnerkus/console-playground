﻿using System.Text.Json;
using BenchmarkDotNet.Running;
using Benchmarks;
using Main.parsers;

namespace Main
{
    public record TestNum
    {
        public int First { get; set; }
        public int Second { get; set; }
    }
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
            // Console.WriteLine(NLuaParser.SimpleMathParse());
            var func = NLuaParser.GetLuaFunc<TestNum, long>();
            var records = new List<TestNum>()
            {
                new ()
                {
                    First = 1,
                    Second = 1
                },
                new ()
                {
                    First = 2,
                    Second = 2
                }
            };
            var sums = records.Select(func).ToList();
            Console.WriteLine(JsonSerializer.Serialize(sums));
            
            // Func testing
            Console.WriteLine("FUNC");
            Console.WriteLine(JsonSerializer.Serialize(TestingFuncs.SimpleFunc()));
        }
    }
}