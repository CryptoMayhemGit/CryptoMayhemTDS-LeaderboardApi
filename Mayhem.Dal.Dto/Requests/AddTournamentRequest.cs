namespace Mayhem.Dal.Dto.Requests
{
    public class AddTournamentRequest
    {
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}