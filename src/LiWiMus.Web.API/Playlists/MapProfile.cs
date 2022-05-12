using AutoMapper;
using LiWiMus.Core.Playlists;
using LiWiMus.Web.API.Playlists.Update;
using LiWiMus.Web.Shared.Extensions;

namespace LiWiMus.Web.API.Playlists;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Request, Playlist>().IgnoreNulls();

        CreateMap<Playlist, Dto>()
            .ForMember(dto => dto.UserId, expression => expression
                .MapFrom(playlist => playlist.Owner.Id))
            .ForMember(dto => dto.UserName, expression => expression
                .MapFrom(playlist => playlist.Owner.UserName));
    }
}