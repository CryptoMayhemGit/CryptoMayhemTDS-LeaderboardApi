using System.ComponentModel.DataAnnotations;

namespace Mayhem.Dal.Tables
{
    public class Tournaments
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string TournamentWalletOwner { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsFinished { get; set; }
        public int HP { get; set; }
        public int SP { get; set; }
        public int MP { get; set; }

        public ICollection<TournamentUserStatistics> TournamentUserStatistics { get; } = new List<TournamentUserStatistics>();
        public ICollection<QuestDetails> QuestDetails { get; } = new List<QuestDetails>();
    }
}
