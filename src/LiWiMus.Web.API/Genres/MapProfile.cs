using AutoMapper;
using LiWiMus.Core.Genres;
using LiWiMus.Web.API.Genres.Create;

namespace LiWiMus.Web.API.Genres;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Request, Genre>();
        CreateMap<Genre, Dto>()
            .ForMember(dto => dto.TracksCount, expression => expression
                .MapFrom(genre => 0));
    }
}