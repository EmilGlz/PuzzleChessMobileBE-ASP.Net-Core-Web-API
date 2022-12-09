using AutoMapper;
using ChessMobileBE.Models.DBModels;
using ChessMobileBE.Models.DTOs.Requests;

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
