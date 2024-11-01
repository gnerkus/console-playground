using BenchmarkDotNet.Running;
using Benchmarks;
using NLua;

namespace Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var benchmarkSummary = BenchmarkRunner.Run<LinqQueryBenchmark>();
            var state = new Lua();
            state.DoString("""
                           
                           		function ScriptFunc (val1, val2)
                           			if val1 > val2 then
                           				return val1 + 1
                           			else
                           				return val2 - 1
                           			end
                           		end
                           		
                           """);
            var scriptFunc = state["ScriptFunc"] as LuaFunction;
            var res = (int)(long)scriptFunc.Call(3, 5).First();
            Console.WriteLine(res);
        }
    }
}