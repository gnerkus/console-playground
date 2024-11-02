namespace Main.parsers
{
    public record Base
    {
        public int First { get; set; }
        public int Second { get; set; }
    }
    
    public record Summed
    {
        public int FirstPlusSecond { get; set; }
    }
    
    public static class TestingFuncs
    {
        public static List<Summed> SimpleFunc()
        {
            var mergeBase = new Func<Base, Summed>(a => new Summed
            {
                FirstPlusSecond = a.First + a.Second
            });
            
            var bases = new List<Base>
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

            var result = bases.Select(mergeBase);
            return result.ToList();
        }
    }
}