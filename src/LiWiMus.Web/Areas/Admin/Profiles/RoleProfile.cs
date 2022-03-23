using AutoMapper;
using LiWiMus.Core.Entities;
using LiWiMus.Web.Areas.Admin.ViewModels;

namespace LiWiMus.Web.Areas.Admin.Profiles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<RoleViewModel, Role>();
    }
}