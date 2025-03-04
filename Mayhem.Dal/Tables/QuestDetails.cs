using Mayhem.Dal.Dto.Request.Models;
using System.ComponentModel.DataAnnotations;

namespace Mayhem.Dal.Tables
{
    public class QuestDetails
    {
        [Key]
        public int Id { get; set; }
        public string TournamentType { get; set; } = null!;
        public int Value { get; set; }

        public int TournamentId { get; set; }
        public Tournaments Tournament { get; set; } = null!;
    }
}
