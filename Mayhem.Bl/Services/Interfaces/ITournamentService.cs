using Mayhem.Dal.Dto.Request;
using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Dto.Responses;

namespace Mayhem.Bl.Services.Interfaces
{
    public interface ITournamentService
    {
        Task<AddTournamentResponse> AddTournamentAsync(AddTournamentRequest request);
        Task<GetActiveTournamentResponse> GetActiveTournamentAsync();
        Task<GetArchivedTournamentsResponse> GetArchivedTournamentsAsync();
        Task<IsAnyTicketTournamentActiveResponse> IsAnyTicketTournamentActiveAsync(string ticket);
        Task<UpdateTournamentResponse> UpdateTournamentAsync(UpdateTournamentRequest request);
    }
}
