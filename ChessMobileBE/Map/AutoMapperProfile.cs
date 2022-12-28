using AutoMapper;
using ChessMobileBE.Models.DBModels;
using ChessMobileBE.Models.DTOs.Requests;
using ChessMobileBE.Models.DTOs.Responses;

namespace ChessMobileBE.Map
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PendingMatchDTO, PendingMatch>().ReverseMap();
            CreateMap<UserViewModel, User>().ReverseMap();
        }
    }
}
