using Mayhem.Dal.MappingsConfiguration;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;

namespace Mayhem.Dal.Context
{
    public class MayhemDataContext : DbContext
    {
        public MayhemDataContext()
        {
        }

        public MayhemDataContext(DbContextOptions<MayhemDataContext> options) : base(options)
        {
        }

        public virtual DbSet<Tournaments> Tournaments { get; set; }
        public virtual DbSet<TournamentUserStatistics> TournamentUserStatistics { get; set; }
        public virtual DbSet<ActiveGameCodes> ActiveGameCodes { get; set; }
        public virtual DbSet<QuestDetails> QuestDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tournaments>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<QuestDetails>()
                .HasKey(q => q.Id);

            modelBuilder.Entity<QuestDetails>()
                .HasOne(q => q.Tournament)
                .WithMany(t => t.QuestDetails)
                .HasForeignKey(q => q.TournamentId);

            MappingConfiguration.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:adriagames-database-server.database.windows.net,1433;Initial Catalog=AdriaGames-LeaderBoardApi-SqlDatabase;Persist Security Info=False;User ID=AdriaGamesAdmin;Password=LZFK*t3F5wgWG21H;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }
    }
}
