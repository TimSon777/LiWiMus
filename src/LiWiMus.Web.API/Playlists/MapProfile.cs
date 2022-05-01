using AutoMapper;
using LiWiMus.Core.Playlists;
using LiWiMus.Web.API.Playlists.Update;

namespace LiWiMus.Web.API.Playlists;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Request, Playlist>();
    }
}