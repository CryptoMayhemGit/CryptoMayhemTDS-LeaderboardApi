using AutoMapper;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Requests;
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
                .ForMember(x => x.CreateDate, y => y.MapFrom(z => z.CreateDate))
                .ForMember(x => x.QuestDetails, y => y.MapFrom(z => z.QuestDetails));

            CreateMap<QuestDetailsDto, QuestDetails>()
                .ForMember(x => x.TournamentType, y => y.MapFrom(z => z.TournamentType))
                .ForMember(x => x.Value, y => y.MapFrom(z => z.Value));

            CreateMap<Tournaments, TournamentDto>()
                .ForMember(dest => dest.TournamentUserStatistics, opt => opt.MapFrom(src => src.TournamentUserStatistics))
                .ForMember(dest => dest.QuestDetails, opt => opt.MapFrom(src => src.QuestDetails));

            CreateMap<TournamentUserStatistics, TournamentUserStatisticsDto>();
            CreateMap<QuestDetails, QuestDetailsDto>();

            CreateMap<QuestDetailsRequest, QuestDetailsDto>();
        }
    }
}