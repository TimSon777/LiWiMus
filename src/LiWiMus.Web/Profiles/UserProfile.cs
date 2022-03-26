using AutoMapper;
using LiWiMus.Core.Entities;
using LiWiMus.Web.Areas.User.ViewModels;

namespace LiWiMus.Web.Profiles;

// ReSharper disable once UnusedType.Global
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ProfileViewModel, User>()
            .ForMember(user => user.Gender, opt => opt.MapFrom(vm => vm.IsMale ? Gender.Male : Gender.Female))
            .ReverseMap()
            .ForPath(vm => vm.IsMale, opt => opt.MapFrom(user => user.Gender == Gender.Male));
    }
}