using NLua;

namespace Main.parsers
{
    public static class NLuaParser
    {
        public static int SimpleMathParse()
        {
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
            return res;
        }

        public static void SimpleObjectParse()
        {
	        
        }
    }
}