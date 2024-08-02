using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Dto.Responses;

namespace Mayhem.Bl.Services.Interfaces
{
    public interface IUserStatisticsService
    {
        Task<FinishGameResponse> FinishGameAsync(FinishGameRequest addGameRequest);
        Task<StatsResponse> GetGameStatsAsync();
        Task<PlayerPointsResponse> GetPlayerPointsAsync(PlayerPointsRequest playerPointsRequest);
        Task<StartGameResponse> StartGameAsync(StartGameRequest request);
    }
}
