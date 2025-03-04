namespace Mayhem.Dal.Dto.Dtos
{
    public class TournamentDto
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
        public List<TournamentUserStatisticsDto> TournamentUserStatistics { get; set; }
        public List<QuestDetailsDto> QuestDetails { get; set; }
    }
}
