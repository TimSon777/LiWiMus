using AutoMapper;
using LiWiMus.Core.Roles;

namespace LiWiMus.Web.API.SystemPermissions;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<SystemPermission, Dto>();
    }
}