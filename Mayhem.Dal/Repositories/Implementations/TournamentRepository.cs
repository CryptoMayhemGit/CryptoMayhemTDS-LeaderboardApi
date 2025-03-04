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
        public async Task<int> AddAsync(TournamentDto tournamentDto)
        {
            var tournament = mapper.Map<Tournaments>(tournamentDto);
            await mayhemDataContext.Tournaments.AddAsync(tournament);
            await mayhemDataContext.SaveChangesAsync();
            return tournament.Id;
        }

        public async Task<Tournaments> GetAsync(string name)
        {
            return await mayhemDataContext
            .Tournaments
            .AsNoTracking()
            .Where(x => x.Name == name)
            .SingleOrDefaultAsync();
        }

        public async Task<Tournaments> GetAsync(int id)
        {
            return await mayhemDataContext
            .Tournaments
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(Tournaments tournaments)
        {
            mayhemDataContext.Tournaments.Update(tournaments);
            await mayhemDataContext.SaveChangesAsync();
        }

        public async Task<Tournaments> GetActiveAsync()
        {
            return await mayhemDataContext
            .Tournaments
            .Include(x => x.TournamentUserStatistics)
            .Include(x => x.QuestDetails)
            .AsNoTracking()
            .Where(x => x.EndDate > DateTime.UtcNow)
            .Where(x => x.IsFinished == false)
            .SingleOrDefaultAsync();
        }

        public async Task<List<Tournaments>> GetArchivedAsync()
        {
            return await mayhemDataContext
            .Tournaments
            .Include(x => x.TournamentUserStatistics)
            .AsNoTracking()
            .Where(x => x.EndDate < DateTime.UtcNow)
            .ToListAsync();
        }
    }
}
