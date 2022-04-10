using LiWiMus.Core.Chats;
using LiWiMus.Core.Chats.Specifications;
using LiWiMus.Core.Constants;
using LiWiMus.Core.Messages;
using LiWiMus.Core.OnlineConsultants;
using LiWiMus.Core.OnlineConsultants.Specifications;
using LiWiMus.Core.Specifications;
using LiWiMus.Core.Users;
using LiWiMus.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace LiWiMus.Web.Hubs;

public class SupportChatUserHub : Hub
{
    private readonly IRepository<OnlineConsultant> _repositoryOnlineConsultants;
    private readonly UserManager<User> _userManager;
    private readonly IRepository<Chat> _repositoryChat;
    private readonly IRepository<OnlineConsultant> _onlineConsultantsRepository;
    private readonly IRepository<Message> _messageRepository;


    public SupportChatUserHub(IRepository<OnlineConsultant> repositoryOnlineConsultants, 
        UserManager<User> userManager,
        IRepository<Chat> repositoryChat, IRepository<OnlineConsultant> onlineConsultantsRepository, IRepository<Message> messageRepository)
    {
        _repositoryOnlineConsultants = repositoryOnlineConsultants;
        _userManager = userManager;
        _repositoryChat = repositoryChat;
        _onlineConsultantsRepository = onlineConsultantsRepository;
        _messageRepository = messageRepository;
    }

    public async Task ConnectConsultant()
    {
        var userClaims = Context.User;
        
        if (userClaims?.IsInRole(Roles.Consultant.Name) == false)
        {
            return;
        }

        var user = await _userManager.GetUserAsync(userClaims);

        var consultant = new OnlineConsultant
        {
            Consultant = user,
            ConnectionId = Context.ConnectionId
        };

        await _onlineConsultantsRepository.AddAsync(consultant);
        await _onlineConsultantsRepository.SaveChangesAsync();
    }
    
    public async Task ConnectUser()
    {
        var consultants = await _repositoryOnlineConsultants.ListAsync();
        var consultant = consultants
            .OrderBy(c => c.Chats.Count)
            .ThenByDescending(c => c.CreatedAt)
            .FirstOrDefault();
        
        if (consultant is null)
        {
            return;
        }

        var user = await _userManager.GetUserAsync(Context.User);

        if (user is null)
        {
            return;
        }

        var chat = new Chat
        {
            Consultant = consultant,
            User = user,
            UserConnectionId = Context.ConnectionId
        };
        
        await _repositoryChat.AddAsync(chat);
        await _repositoryChat.SaveChangesAsync();

        var groupName = user.UserName;
        
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Groups.AddToGroupAsync(consultant.ConnectionId, groupName);
    }

    public async Task SendMessageToConsultant(string text)
    {
        var user = await _userManager.GetUserAsync(Context.User);
        var chat = await _repositoryChat.GetBySpecAsync(new OpenedChatByUserSpec(user));
        
        if (chat is null)
        {
            return;
        }
        
        chat.Messages.Add(new Message
        {
            Text = text,
            Sender = user
        });

        await _repositoryChat.SaveChangesAsync();

        await Clients.Group(user.UserName).SendAsync("SendMessageToConsultant", text, user.UserName);
    }
    
    public async Task SendMessageToUser(string connectionId, string text)
    {
        var user = await _userManager.GetUserAsync(Context.User);
        var onlineConsultant = await _onlineConsultantsRepository.GetBySpecAsync(new OnlineConsultantByConnectionIdSpec(Context.ConnectionId));
        
        var chat = onlineConsultant?.Chats.FirstOrDefault(c => c.UserConnectionId == connectionId);
        
        if (chat is null)
        {
            return;
        }

        await _messageRepository.AddAsync(new Message
        {
            Chat = chat,
            Text = text,
            Sender = user
        });
        
        await _messageRepository.SaveChangesAsync();

        await Clients.Group(chat.User.UserName).SendAsync("SendMessageToConsultant", text);
    }
}