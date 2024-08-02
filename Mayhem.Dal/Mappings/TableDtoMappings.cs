using AutoMapper;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Tables;

namespace Mayhem.Dal.Mappings
{
    public class TableDtoMappings : Profile
    {
        public TableDtoMappings()
        {
            CreateMap<ActiveGameCodesDto, ActiveGameCodes>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.Wallet, y => y.MapFrom(z => z.Wallet))
                .ForMember(x => x.GameCode, y => y.MapFrom(z => z.GameCode))
                .ForMember(x => x.TournamentId, y => y.MapFrom(z => z.TournamentId))
                .ForMember(x => x.CreateDate, y => y.MapFrom(z => z.CreateDate));

        CreateMap<TournamentUserStatisticsDto, TournamentUserStatistics> ()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.Wallet, y => y.MapFrom(z => z.Wallet))
                .ForMember(x => x.IsWin, y => y.MapFrom(z => z.IsWin))
                .ForMember(x => x.Kills, y => y.MapFrom(z => z.Kills))
                .ForMember(x => x.TournamentId, y => y.MapFrom(z => z.TournamentId))
                .ForMember(x => x.CreateDate, y => y.MapFrom(z => z.CreateDate));

            CreateMap<TournamentDto, Tournaments>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.Name, y => y.MapFrom(z => z.Name))
                .ForMember(x => x.StartDate, y => y.MapFrom(z => z.StartDate))
                .ForMember(x => x.EndDate, y => y.MapFrom(z => z.EndDate))
                .ForMember(x => x.CreateDate, y => y.MapFrom(z => z.CreateDate));

            CreateMap<Tournaments, TournamentDto>()
                .ForMember(dest => dest.TournamentUserStatistics, opt => opt.MapFrom(src => src.TournamentUserStatistics));

            CreateMap<TournamentUserStatistics, TournamentUserStatisticsDto>();
        }
    }
}