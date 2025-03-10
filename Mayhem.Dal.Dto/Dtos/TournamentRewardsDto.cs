namespace Mayhem.Dal.Dto.Dtos
{
    public class TournamentRewardsDto
    {
        public int TournamentId { get; set; }
        public string TournamentName { get; set; } = null!;
        public string TournamentWalletOwnerName { get; set; } = null!;
        public float TournamentWalletOwnerReward { get; set; }
        public string TournamentWalletOwnerTransactionHash { get; set; } = null!;
        public List<WinnerTables> WinnersTable { get; set; } = null!;
    }

    public class WinnerTables
    {
        public string TransactionHash { get; set; } = null!;
        public string Wallet { get; set; } = null!;
        public int KillsSum { get; set; }
        public int Rank { get; set; }
        public int Points { get; set; }
        public float Reward { get; set; }
    }
}
