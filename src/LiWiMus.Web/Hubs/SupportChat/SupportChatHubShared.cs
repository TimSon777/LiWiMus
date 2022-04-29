using AutoMapper;
using LiWiMus.Core.Chats;
using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Messages;
using LiWiMus.Core.OnlineConsultants;
using LiWiMus.Core.OnlineConsultants.Specifications;
using LiWiMus.Core.Users;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;

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
    
    private async Task EstablishConnection(User user, OnlineConsultant consultant)
    {
        var groupName = user.UserName;
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Groups.AddToGroupAsync(consultant.ConnectionId, groupName);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = await _userManager.GetUserAsync(Context.User);
        var consultant = await _onlineConsultantsRepository.GetBySpecAsync(new ConsultantByUser(user));
        
        if (consultant is not null)
        {
            await DisconnectConsultant();
        }
        
        await base.OnDisconnectedAsync(exception);
    }
}