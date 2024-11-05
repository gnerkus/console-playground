using NLua;

namespace Main.parsers
{
    public static class NLuaParser
    {
        public static int SimpleMathParse()
        {
            var state = new Lua();
            state.DoString("""
                           
                           		function ScriptFunc (val2)
                           		    return val2.First + val2.Second
                           		end
                           		
                           """);
            var scriptFunc = state["ScriptFunc"] as LuaFunction;
            var res = (int)(long)scriptFunc.Call(new TestNum() { First = "3", Second = 4}).First();
            return res;
        }

        public static Func<T, TU> GetLuaFunc<T, TU>() where T : TestNum
        {
            var state = new Lua();
            state.DoString("""
                           
                           		function ScriptFunc (val2)
                           		    return val2.First + val2.Second
                           		end
                           		
                           """);
            var scriptFunc = state["ScriptFunc"] as LuaFunction;
            var newFunc = new Func<T, TU>(a => (TU)scriptFunc.Call(a).First());
            return newFunc;
        }
    }
}