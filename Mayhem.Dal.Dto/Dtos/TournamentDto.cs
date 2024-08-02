namespace Mayhem.Dal.Dto.Dtos
{
    public class TournamentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsArchived { get; set; }
        public DateTime CreateDate { get; set; }
        public List<TournamentUserStatisticsDto> TournamentUserStatistics { get; set; }
    }
}
