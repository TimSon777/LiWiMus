using AutoMapper;
using LiWiMus.Core.Albums;
using LiWiMus.Core.PlaylistTracks;
using LiWiMus.Core.Tracks;
using LiWiMus.Web.MVC.Areas.Search.ViewModels;
using LiWiMus.Web.MVC.ViewModels;

namespace LiWiMus.Web.MVC.Areas.Search.Profiles;

// ReSharper disable once UnusedType.Global
public class TrackProfile : Profile
{
    public TrackProfile()
    {
        CreateMap<Album, AlbumForTrackViewModel>();
        CreateMap<Core.Artists.Artist, ArtistForTrackListViewModel>();
        CreateMap<PlaylistTrack, PlaylistGeneralInfoViewModel>();
        CreateMap<Track, TrackViewModel>();
    }
}