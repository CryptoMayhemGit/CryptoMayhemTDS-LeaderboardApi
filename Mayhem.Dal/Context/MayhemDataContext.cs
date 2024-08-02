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

        public virtual DbSet<Tournaments> tournaments { get; set; }
        public virtual DbSet<TournamentUserStatistics> tournamentUserStatistics { get; set; }
        public virtual DbSet<ActiveGameCodes> activeGameCodes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MappingConfiguration.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
        }
    }
}
