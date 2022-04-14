using AutoMapper;
using LiWiMus.Core.Chats;
using LiWiMus.Core.Chats.Enums;
using LiWiMus.Core.Chats.Specifications;
using LiWiMus.Core.Constants;
using LiWiMus.Core.Interfaces;
using LiWiMus.Core.Messages;
using LiWiMus.Core.OnlineConsultants;
using LiWiMus.Core.OnlineConsultants.Specifications;
using LiWiMus.Core.Users;
using LiWiMus.Core.Users.Specifications;
using LiWiMus.Infrastructure.Identity;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.Areas.User.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LiWiMus.Web.Hubs.SupportChat;

public class SupportChatUserHub : Hub
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly IRepository<Chat> _repositoryChat;
    private readonly IRepository<OnlineConsultant> _onlineConsultantsRepository;
    private readonly IRepository<Message> _messageRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;
    private readonly IRazorViewRenderer _razorViewRenderer;

    public SupportChatUserHub(UserManager<UserIdentity> userManager,
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

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = await _userManager.GetUserAsync(Context.User);
        var consultant = await _onlineConsultantsRepository.GetByIdAsync(new ConsultantByUser(user));
        
        if (consultant is not null)
        {
            await _onlineConsultantsRepository.DeleteAsync(consultant);
        }

        await base.OnDisconnectedAsync(exception);
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

    private async Task<Chat> ConnectUserWithExistChat(User user, Chat chat)
    {
        OnlineConsultant? consultant;
        try
        {
            consultant = await _onlineConsultantsRepository
                .GetByIdAsync(new OnlineConsultantByConnectionIdSpec(chat.Consultant.ConnectionId));
        }
        catch (Exception)
        {
            consultant = null;
        }

        if (consultant is null)
        {
            consultant = await _onlineConsultantsRepository.GetBySpecAsync(new ConsultantWithMinimalWorkload());
            chat.Consultant = consultant!;
            consultant!.Chats.Add(chat);
        }

        await EstablishConnection(user, consultant);
        return chat;
    }

    private async Task EstablishConnection(User user, OnlineConsultant consultant)
    {
        var groupName = user.UserName;
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Groups.AddToGroupAsync(consultant.ConnectionId, groupName);
    }

    private async Task<Chat> ConnectUserWithoutExistChat(User user)
    {
        var consultant = await _onlineConsultantsRepository.GetBySpecAsync(new ConsultantWithMinimalWorkload());

        var chat = new Chat
        {
            Consultant = consultant!,
            User = user,
            UserConnectionId = Context.ConnectionId
        };
        
        await _repositoryChat.AddAsync(chat);
        await _repositoryChat.SaveChangesAsync();

        await EstablishConnection(user, consultant!);
        return chat;
    }
    
    public async Task<IActionResult> ConnectUser()
    {
        var userWithoutChats = await _userManager.GetUserAsync(Context.User);
        
        if (userWithoutChats is null)
        {
            return new BadRequestObjectResult("You aren't authorize");
        }
        
        var user = await _userRepository.GetBySpecAsync(new UserWithChatsSpec(userWithoutChats));
        
        if (user is null)
        {
            return new BadRequestObjectResult("You aren't authorize");
        }
        
        var chatOld = user.UserChats.FirstOrDefault(c => c.Status == ChatStatus.Opened);

        var chat = await (chatOld is null
            ? ConnectUserWithoutExistChat(user)
            : ConnectUserWithExistChat(user, chatOld));
        
        var chatVm = _mapper.Map<ChatViewModel>(chat);
        
        var html = await _razorViewRenderer.RenderViewAsync("/Areas/User/Views/Partials/ChatUserPartial.cshtml", chatVm);

        return new OkObjectResult(html);
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
        
        var onlineConsultant = await _onlineConsultantsRepository
            .GetBySpecAsync(new OnlineConsultantByConnectionIdSpec(Context.ConnectionId));
        
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

        await Clients.Group(chat.User.UserName).SendAsync("SendMessageToUser", text);
    }

    public async Task DisconnectConsultant()
    {
        var user = await _userManager.GetUserAsync(Context.User);
        var consultant = await _onlineConsultantsRepository.GetBySpecAsync(new ConsultantByUser(user));
        
        if (consultant is null)
        {
            return;
        }
        
        var chats = consultant.Chats.Where(c => c.Status == ChatStatus.Opened);
        foreach (var chat in chats)
        {
            var newConsultant = await _onlineConsultantsRepository.GetBySpecAsync(new ConsultantWithMinimalWorkload(consultant));

            if (newConsultant is null)
            {
                return;
            }

            chat.Consultant = newConsultant;
        }
    } 
}