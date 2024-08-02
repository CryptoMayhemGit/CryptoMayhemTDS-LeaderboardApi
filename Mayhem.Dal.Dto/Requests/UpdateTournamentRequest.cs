namespace Mayhem.Dal.Dto.Requests
{
    public class UpdateTournamentRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
