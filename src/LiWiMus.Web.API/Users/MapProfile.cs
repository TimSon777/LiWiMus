using AutoMapper;
using LiWiMus.Core.Users;
using LiWiMus.Web.API.Users.Create;

namespace LiWiMus.Web.API.Users;

// ReSharper disable once UnusedType.Global
public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Request, User>();
        CreateMap<User, Dto>();
    }
}