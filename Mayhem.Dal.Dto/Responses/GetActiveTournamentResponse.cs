using Mayhem.Dal.Dto.Dtos;

namespace Mayhem.Dal.Dto.Responses
{
    public class GetActiveTournamentResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsArchived { get; set; }
        public DateTime CreateDate { get; set; }

        public IList<TournamentUserStatisticsDto> TournamentUserStatistics { get; set; } = null!;
    }
}
