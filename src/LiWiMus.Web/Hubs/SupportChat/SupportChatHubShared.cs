using AutoMapper;
using LiWiMus.Core.Chats;
using LiWiMus.Core.Chats.Enums;
using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Messages;
using LiWiMus.Core.OnlineConsultants;
using LiWiMus.Core.OnlineConsultants.Specifications;
using LiWiMus.Core.Users;
using LiWiMus.Core.Users.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace LiWiMus.Web.Hubs.SupportChat;

public partial class SupportChatHub
{
    private readonly UserManager<User> _userManager;
    private readonly IRepository<Chat> _repositoryChat;
    private readonly IRepository<OnlineConsultant> _onlineConsultantsRepository;
    private readonly IRepository<Message> _messageRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;
    private readonly IRazorViewRenderer _razorViewRenderer;

    public SupportChatHub(UserManager<User> userManager,
        IRepository<Chat> repositoryChat, IRepository<OnlineConsultant> onlineConsultantsRepository, 
        IRepository<Message> messageRepository, 
        IRepository<User> userRepository,
        IMapper mapper, IRazorViewRenderer razorViewRenderer)
    {
        _userManager = userManager;
        _repositoryChat = repositoryChat;
        _onlineConsultantsRepository = onlineConsultantsRepository;
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _razorViewRenderer = razorViewRenderer;
    }
    
    private async Task EstablishConnection(User user, OnlineConsultant consultant, string? userConnectionId = null)
    {
        var groupName = user.UserName;
        
        if (userConnectionId is null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        else
        {
            await Groups.AddToGroupAsync(userConnectionId, groupName);
        }
        
        await Groups.AddToGroupAsync(consultant.ConnectionId, groupName);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = await _userManager.GetUserAsync(Context.User);
        var userWithChats = await _userRepository.GetBySpecAsync(new UserWithChatsSpec(user));
        var consultant = await _onlineConsultantsRepository.GetBySpecAsync(new ConsultantByUser(user));
        
        if (consultant is not null)
        {
            await DisconnectConsultant();
        }

        var chat = userWithChats!.UserChats.FirstOrDefault(c => c.Status == ChatStatus.Opened);
        
        if (chat is not null)
        {
            await Clients.Group(user.UserName).SendAsync("DeleteChat", user.UserName);
        }

        await base.OnDisconnectedAsync(exception);
    }
}