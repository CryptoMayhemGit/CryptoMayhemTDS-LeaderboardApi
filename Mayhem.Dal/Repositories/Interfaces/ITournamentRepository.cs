using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Tables;

namespace Mayhem.Dal.Repositories.Interfaces
{
    public interface ITournamentRepository
    {
        Task UpdateAsync(Tournaments tournaments);
        Task<Tournaments> GetAsync(int id);
        Task<int> AddAsync(TournamentDto tournamentDto);
        Task<Tournaments> GetAsync(string name);
        Task<Tournaments> GetActiveAsync();
        Task<List<Tournaments>> GetArchivedAsync();
    }
}
