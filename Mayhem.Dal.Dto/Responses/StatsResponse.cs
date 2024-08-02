namespace Mayhem.Dal.Dto.Responses
{
    public class StatsResponse
    {
        public List<PlayerStatistic> PlayerStatistic { get; set; } = new();
    }

    public class PlayerStatistic
    {
        public int Order { get; set; }
        public int Change { get; set; }
        public string Wallet { get; set; }
        public int Win { get; set; }
        public int Lose { get; set; }
        public int Kills { get; set; }
        public int Points { get; set; }

    }
}
