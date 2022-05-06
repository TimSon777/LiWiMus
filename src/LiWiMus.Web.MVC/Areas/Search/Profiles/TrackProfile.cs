using AutoMapper;
using LiWiMus.Core.Albums;
using LiWiMus.Core.Tracks;
using LiWiMus.Web.MVC.Areas.Search.ViewModels;

namespace LiWiMus.Web.MVC.Areas.Search.Profiles;

// ReSharper disable once UnusedType.Global
public class TrackProfile : Profile
{
    public TrackProfile()
    {
        CreateMap<Album, AlbumForTrackViewModel>();
        CreateMap<Core.Artists.Artist, ArtistForTrackListViewModel>();
        CreateMap<Track, TrackListViewModel>();
    }
}