namespace Mayhem.Dal.Tables
{
    public class Tournaments
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreateDate { get; set; }

        public ICollection<TournamentUserStatistics> TournamentUserStatistics { get; } = new List<TournamentUserStatistics>();
    }
}
