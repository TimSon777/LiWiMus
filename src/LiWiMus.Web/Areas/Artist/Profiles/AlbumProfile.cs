using AutoMapper;
using LiWiMus.Core.Albums;
using LiWiMus.Web.Areas.Artist.ViewModels;

namespace LiWiMus.Web.Areas.Artist.Profiles;

public class AlbumProfile : Profile
{
    public AlbumProfile()
    {
        CreateMap<CreateAlbumViewModel, Album>().ReverseMap();
        CreateMap<UpdateAlbumViewModel, Album>().ReverseMap();
    }
}