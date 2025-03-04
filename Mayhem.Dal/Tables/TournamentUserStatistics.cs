using System.ComponentModel.DataAnnotations;

namespace Mayhem.Dal.Tables
{
    public class TournamentUserStatistics
    {
        [Key]
        public int Id { get; set; }
        public string Wallet { get; set; } = null!;
        public bool IsWin { get; set; }
        public int Kills { get; set; }
        public DateTime CreateDate { get; set; }

        public int TournamentId { get; set; }
        public Tournaments Tournament { get; set; } = null!;
    }
}
