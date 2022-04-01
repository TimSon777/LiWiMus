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
        CreateMap<Chat, ChatViewModel>().ReverseMap();
    }
}