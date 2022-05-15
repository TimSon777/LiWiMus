using AutoMapper;
using LiWiMus.Core.Tracks;
using LiWiMus.Web.API.Tracks.Create;

namespace LiWiMus.Web.API.Tracks;

// ReSharper disable once UnusedType.Global
public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Request, Track>();

        CreateMap<Track, Dto>()
            .ForMember(dto => dto.Artists, expression => expression
                .MapFrom(track => track.Owners));
    }
}