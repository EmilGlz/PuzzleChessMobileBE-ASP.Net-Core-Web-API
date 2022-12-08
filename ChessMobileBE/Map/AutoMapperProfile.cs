using AutoMapper;
using ChessMobileBE.Models.DBModels;
using ChessMobileBE.Models.DTOs;

namespace ChessMobileBE.Map
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PendingMatchDTO, PendingMatch>().ReverseMap();
        }
    }
}
