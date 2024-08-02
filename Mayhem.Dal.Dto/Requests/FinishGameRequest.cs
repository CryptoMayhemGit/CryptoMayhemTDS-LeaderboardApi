namespace Mayhem.Dal.Dto.Requests
{
    public class FinishGameRequest
    {
        public bool IsWin { get; set; }
        public int Kills { get; set; }
        public string Ticket { get; set; } = null!;
        public Guid GameCode { get; set; }

    }
}
