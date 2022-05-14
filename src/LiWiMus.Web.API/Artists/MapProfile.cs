using AutoMapper;
using LiWiMus.Core.Artists;

namespace LiWiMus.Web.API.Artists;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Artist, Dto>();
    }
}