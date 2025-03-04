using AutoMapper;
using Mayhem.Dal.Context;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Repositories.Interfaces;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;

namespace Mayhem.Dal.Repositories.Implementations
{
    public class TournamentUserStatisticRepository(MayhemDataContext mayhemDataContext, IMapper mapper) : ITournamentUserStatisticRepository
    {
        public async Task<List<TournamentUserStatistics>> GetGameUsersAsync(int tournamentId)
        {
            return await mayhemDataContext
                .TournamentUserStatistics
                .Where(x => x.TournamentId == tournamentId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<TournamentUserStatistics>> GetActiveTournamentGameUsersAsync()
        {
            return await mayhemDataContext
                .TournamentUserStatistics
                .Include(x => x.Tournament)
                .Where(x => x.Tournament.EndDate > DateTime.UtcNow)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(TournamentUserStatisticsDto tournamentUserStatisticsDto)
        {
            await mayhemDataContext.TournamentUserStatistics.AddAsync(mapper.Map<TournamentUserStatistics>(tournamentUserStatisticsDto));
            await mayhemDataContext.SaveChangesAsync();
        }
    }
}
