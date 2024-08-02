using Mayhem.Dal.Dto.Dtos;

namespace Mayhem.Dal.Dto.Responses
{
    public class GetArchivedTournamentsResponse
    {
        public IList<TournamentDto> TournamentDtos { get; set; } = null!;
    }
}
