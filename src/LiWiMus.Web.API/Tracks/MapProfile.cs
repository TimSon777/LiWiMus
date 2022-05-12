using AutoMapper;
using LiWiMus.Core.Tracks;

namespace LiWiMus.Web.API.Tracks;

// ReSharper disable once UnusedType.Global
public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Create.Request, Track>();
    }
}