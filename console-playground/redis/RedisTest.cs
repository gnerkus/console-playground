using System.Text.Json;
using StackExchange.Redis;

namespace Main.redis
{
    public class Participant
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
    }
    
    public class RedisTest
    {
        private readonly IDatabase _db;
        
        public RedisTest()
        {
            var redis = ConnectionMultiplexer.Connect("localhost");
            _db = redis.GetDatabase(10);
        }

        public void AddScores()
        {
            var leaderboardGuid = Guid.NewGuid();
            var leaderboardId = leaderboardGuid.ToString();

            var participantOne = new Participant
            {
                Id = Guid.NewGuid(),
                FullName = "John Doe"
            };
            
            var participantTwo = new Participant
            {
                Id = Guid.NewGuid(),
                FullName = "Jane Doe"
            };

            _db.SortedSetIncrement(leaderboardId, JsonSerializer.Serialize(participantOne), 20);
            _db.SortedSetIncrement(leaderboardId, JsonSerializer.Serialize(participantOne), 20);
            _db.SortedSetIncrement(leaderboardId, JsonSerializer.Serialize(participantTwo), 100);
            _db.SortedSetIncrement(leaderboardId, JsonSerializer.Serialize(participantOne), 10);

            var results = _db.SortedSetScan(leaderboardId);
            var first = results.First();
            var firstParticipant = JsonSerializer.Deserialize<Participant>(first.Element);
            Console.WriteLine(firstParticipant.FullName);
        }
    }
}