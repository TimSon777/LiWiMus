using AutoMapper;
using LiWiMus.Core.Albums;
using LiWiMus.Core.Playlists;
using LiWiMus.Core.Tracks;
using LiWiMus.Web.MVC.Areas.Search.ViewModels;

namespace LiWiMus.Web.MVC.Areas.Search.Profiles;

// ReSharper disable once UnusedType.Global
public class SearchProfile : Profile
{
    public SearchProfile()
    {
        CreateMap<Playlist, PlaylistForListViewModel>();
        CreateMap<Album, AlbumForListViewModel>();
        CreateMap<Track, TrackForListViewModel>();
    }
}