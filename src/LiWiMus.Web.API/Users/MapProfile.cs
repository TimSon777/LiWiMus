using AutoMapper;
using LiWiMus.Core.Users;

namespace LiWiMus.Web.API.Users;

// ReSharper disable once UnusedType.Global
public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Create.Request, User>();
    }
}