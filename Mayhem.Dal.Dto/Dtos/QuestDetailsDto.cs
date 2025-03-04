using Mayhem.Dal.Dto.Request.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mayhem.Dal.Dto.Dtos
{
    public class QuestDetailsDto
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public TournamentType TournamentType { get; set; }
        public int Value { get; set; }
    }
}
