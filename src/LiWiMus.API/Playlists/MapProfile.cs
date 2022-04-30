using AutoMapper;
using LiWiMus.API.Playlists.Update;
using LiWiMus.Core.Playlists;

namespace LiWiMus.API.Playlists;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Request, Playlist>();
    }
}