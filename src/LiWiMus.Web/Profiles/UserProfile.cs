using AutoMapper;
using LiWiMus.Core.Entities;
using LiWiMus.Web.Areas.User.ViewModels;

namespace LiWiMus.Web.Profiles;

// ReSharper disable once UnusedType.Global
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ProfileViewModel, User>().ReverseMap();
    }
}