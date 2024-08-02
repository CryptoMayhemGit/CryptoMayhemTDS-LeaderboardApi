using Mayhem.Dal.Dto.Dtos;

namespace Mayhem.Dal.Repositories.Interfaces
{
    public interface IGameCodeRepository
    {
        Task CreateGameCodeAsync(ActiveGameCodesDto gameCodeDto);
        Task<bool> IsGameCodeActiveAsync(string wallet, Guid gameCode, int tournamentId);
        Task RemoveAsync(Guid gameCode);
    }
}
