using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Tables;

namespace Mayhem.Dal.Repositories.Interfaces
{
    public interface ITournamentUserStatisticRepository
    {
        Task<List<TournamentUserStatistics>> GetActiveTournamentGameUsersAsync();
        Task AddAsync(TournamentUserStatisticsDto tournamentUserStatisticsDto);
        Task<List<TournamentUserStatistics>> GetGameUsersAsync(int tournamentId);
    }
}
