using AutoMapper;
using LiWiMus.Core.Plans;

namespace LiWiMus.Web.API.Permissions;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Permission, Dto>();
    }
}