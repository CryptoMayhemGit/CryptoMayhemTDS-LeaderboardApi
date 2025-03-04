using System.ComponentModel.DataAnnotations;

namespace Mayhem.Dal.Tables
{
    public class ActiveGameCodes
    {
        [Key]
        public int Id { get; set; }
        public string Wallet { get; set; } = null!;
        public Guid GameCode { get; set; }
        public int TournamentId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
