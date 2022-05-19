using AutoMapper;
using LiWiMus.Core.Albums;
using LiWiMus.Web.API.Albums.Create;
using LiWiMus.Web.Shared.Extensions;

namespace LiWiMus.Web.API.Albums;

// ReSharper disable once UnusedType.Global
public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Request, Album>();

        CreateMap<Album, Dto>()
            .ForMember(dto => dto.Artists, expression => expression.Ignore())
            .ForMember(dto => dto.TracksCount, expression => expression.Ignore())
            .ForMember(dto => dto.ListenersCount, expression => expression.Ignore());

        CreateMap<Update.Request, Album>()
            .IgnoreNulls();
    }
}