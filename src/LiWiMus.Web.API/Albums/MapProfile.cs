using AutoMapper;
using LiWiMus.Core.Albums;
using LiWiMus.Web.Shared.Extensions;

namespace LiWiMus.Web.API.Albums;

// ReSharper disable once UnusedType.Global
public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Create.Request, Album>();
        CreateMap<Album, Dto>();
        CreateMap<Update.Request, Album>().IgnoreNulls();
    }
}