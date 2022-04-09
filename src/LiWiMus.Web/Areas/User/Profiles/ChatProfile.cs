using AutoMapper;
using LiWiMus.Core.Entities;
using LiWiMus.Web.Areas.User.ViewModels;

namespace LiWiMus.Web.Areas.User.Profiles;

// ReSharper disable once UnusedType.Global
public class ChatProfile : Profile
{
    public ChatProfile()
    {
        CreateMap<Core.Entities.User, UserChatViewModel>().ReverseMap();
        CreateMap<Chat, ChatViewModel>()
            .ForMember(a => a.Consultant, opt => opt.MapFrom(s => s.Consultant.Consultant))
            .ReverseMap()
            .ForPath(a => a.Consultant.Consultant, opt => opt.MapFrom(s => s.Consultant));
    }
}