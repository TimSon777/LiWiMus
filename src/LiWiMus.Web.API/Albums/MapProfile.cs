using AutoMapper;
using LiWiMus.Core.Albums;
namespace LiWiMus.Web.API.Albums;

// ReSharper disable once UnusedType.Global
public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Create.Request, Album>();
    }
}