namespace Mayhem.Configuration
{
    public class MayhemConfiguration
    {
        public string SqlConnectionString { get; }
        public string JWTValidIssuer { get; }
        public string JWTValidAudience { get; }
        public string JWTSecret { get; }
        public int JWTDurationInMinutes { get; }
        public string AdminLogin { get; }
        public string AdminPassword { get; }
        public string AlturaTournamentAbi { get; set; }
        public string Web3ProviderEndpoint { get; }
        public string AlturaTournamentAddress { get; set; }
        public string PrivateKeyWallet { get; set; }
        public int AlturaTournamentTicketId { get; set; }

        public MayhemConfiguration(
            string web3ProviderEndpoint,
            string sqlConnectionString,
            string jwtValidIssuer,
            string jwtValidAudience,
            string jwtSecret,
            string jwtDurationInMinutes,
            string adminLogin,
            string adminPassword,
            string alturaTournamentAbi,
            string alturaTournamentAddress,
            string alturaTournamentTicketId,
            string privateKeyWallet)
        {
            this.PrivateKeyWallet = privateKeyWallet;
            this.Web3ProviderEndpoint = web3ProviderEndpoint;
            this.SqlConnectionString = sqlConnectionString;
            this.JWTValidIssuer = jwtValidIssuer;
            this.JWTValidAudience = jwtValidAudience;
            this.JWTSecret = jwtSecret;
            this.JWTDurationInMinutes = Convert.ToInt32(jwtDurationInMinutes);
            this.AdminLogin = adminLogin;
            this.AdminPassword = adminPassword;
            this.AlturaTournamentAbi = alturaTournamentAbi;
            this.AlturaTournamentAddress = alturaTournamentAddress;
            this.AlturaTournamentTicketId = Convert.ToInt32(alturaTournamentTicketId);
        }
    }
}
