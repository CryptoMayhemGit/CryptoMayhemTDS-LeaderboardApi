using AutoMapper;
using Mayhem.Dal.Context;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Repositories.Interfaces;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;

namespace Mayhem.Dal.Repositories.Implementations
{
    public class TournamentRepository(MayhemDataContext mayhemDataContext, IMapper mapper) : ITournamentRepository
    {
        public async Task AddAsync(TournamentDto tournamentDto)
        {
            await mayhemDataContext.tournaments.AddAsync(mapper.Map<Tournaments>(tournamentDto));
            await mayhemDataContext.SaveChangesAsync();
        }

        public async Task<Tournaments> GetAsync(string name)
        {
            return await mayhemDataContext
            .tournaments
            .AsNoTracking()
            .Where(x => x.Name == name)
            .SingleOrDefaultAsync();
        }

        public async Task<Tournaments> GetAsync(int id)
        {
            return await mayhemDataContext
            .tournaments
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(Tournaments tournaments)
        {
            await mayhemDataContext.SaveChangesAsync();
        }

        public async Task<Tournaments> GetActiveAsync()
        {
            return await mayhemDataContext
            .tournaments
            .Include(x => x.TournamentUserStatistics)
            .AsNoTracking()
            .Where(x => x.EndDate > DateTime.UtcNow)
            .SingleOrDefaultAsync();
        }

        public async Task<List<Tournaments>> GetArchivedAsync()
        {
            return await mayhemDataContext
            .tournaments
            .Include(x => x.TournamentUserStatistics)
            .AsNoTracking()
            .Where(x => x.EndDate < DateTime.UtcNow)
            .ToListAsync();
        }
    }
}
