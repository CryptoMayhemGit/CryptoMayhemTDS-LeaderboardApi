using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Request.Models;
using Newtonsoft.Json.Converters;

namespace Mayhem.Dal.Dto.Responses
{
    public class GetActiveTournamentResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string TournamentWalletOwner { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsFinished { get; set; }
        public DateTime CreateDate { get; set; }
        public int HP { get; set; }
        public int SP { get; set; }
        public int MP { get; set; }

        public IList<TournamentUserStatisticsDto> TournamentUserStatistics { get; set; } = null!;
        public IList<QuestDetailsDtoResponse> QuestDetails { get; set; } = null!;
    }


    public class QuestDetailsDtoResponse
    {
        public string TournamentType { get; set; }
        public int Value { get; set; }
    }
}
