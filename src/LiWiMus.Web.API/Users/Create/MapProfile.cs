using AutoMapper;
using LiWiMus.Core.Users;

namespace LiWiMus.Web.API.Users.Create;

// ReSharper disable once UnusedType.Global
public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Request, User>();
    }
}