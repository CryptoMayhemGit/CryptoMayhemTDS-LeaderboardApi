using Mayhem.Dal.Dto.Dtos;

namespace Mayhem.Dal.Dto.Responses
{
    public class GetActiveTournamentResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsFinished { get; set; }
        public DateTime CreateDate { get; set; }
        public int HP { get; set; }
        public int SP { get; set; }
        public int MP { get; set; }

        public IList<TournamentUserStatisticsDto> TournamentUserStatistics { get; set; } = null!;
        public IList<QuestDetailsDto> QuestDetails { get; set; } = null!;
    }
}
