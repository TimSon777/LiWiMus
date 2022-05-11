using AutoMapper;
using LiWiMus.Core.Playlists;
using LiWiMus.Web.API.Playlists.Update;

namespace LiWiMus.Web.API.Playlists;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Request, Playlist>()
            .ForAllMembers(opts => opts
                .Condition((_, _, srcMember) => srcMember is not null));
        CreateMap<Playlist, Dto>()
            .ForMember(dto => dto.UserId, expression => expression
                .MapFrom(playlist => playlist.Owner.Id))
            .ForMember(dto => dto.UserName, expression => expression
                .MapFrom(playlist => playlist.Owner.UserName));
    }
}