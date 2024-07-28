using BenchmarkDotNet.Running;
using Benchmarks;

namespace Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var benchmarkSummary = BenchmarkRunner.Run<LinqQueryBenchmark>();
        }
    }    
}
