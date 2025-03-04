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
            optionsBuilder.UseSqlServer();
        }
    }
}
