using Mayhem.Dal.Dto.Request.Models;

namespace Mayhem.Dal.Dto.Requests
{
    public class AddTournamentRequest
    {
        public string Name { get; set; } = null!;
        public int DurationHours { get; set; }
        public int HP { get; set; }
        public int SP { get; set; }
        public int MP { get; set; }
        public List<QuestDetailsRequest> QuestDetails { get; set; } = new List<QuestDetailsRequest>();

    }

    public class QuestDetailsRequest
    {
        public string TournamentType { get; set; }
        public int Value { get; set; }
    }
}