using AutoMapper;
using LiWiMus.Core.Entities;
using LiWiMus.Web.ViewModels;

namespace LiWiMus.Web.Profiles;

// ReSharper disable once UnusedType.Global
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, ProfileViewModel>();
    }
}