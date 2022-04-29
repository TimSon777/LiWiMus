using AutoMapper;
using FormHelper;
using LiWiMus.Core.Chats;
using LiWiMus.Core.Chats.Enums;
using LiWiMus.Core.OnlineConsultants;
using LiWiMus.Core.OnlineConsultants.Specifications;
using LiWiMus.Core.Users.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.Areas.User.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LiWiMus.Web.Areas.User.Controllers;

[Area("User")]
public class SupportChatController : Controller
{
    private readonly IRepository<OnlineConsultant> _repositoryConsultant;
    private readonly IRepository<Chat> _chatRepository;
    private readonly UserManager<Core.Users.User> _userManager;
    private readonly IMapper _mapper;
    private readonly IRepository<Core.Users.User> _userRepository;

    public SupportChatController(IRepository<OnlineConsultant> repositoryConsultant,
        IRepository<Chat> chatRepository, 
        UserManager<Core.Users.User> userManager,
        IMapper mapper, IRepository<Core.Users.User> userRepository)
    {
        _repositoryConsultant = repositoryConsultant;
        _chatRepository = chatRepository;
        _userManager = userManager;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    [HttpGet]
    [Authorize(Roles = "Consultant")]
    public IActionResult Chats()
    {
        return View();
    }

    [HttpGet]
    [Authorize(Roles = "Consultant")]
    public async Task<IActionResult> GetTextingUsersChats()
    {
        var user = await _userManager.GetUserAsync(User);
        
        var onlineConsultant = await _repositoryConsultant
            .GetBySpecAsync(new ConsultantByUser(user));
        
        var userNames = onlineConsultant?.Chats
            .Where(ch => ch.Status == ChatStatus.Opened)
            .Select(ch => ch.User.UserName);

        return Json(userNames);
    }

    [HttpGet]
    [Authorize(Roles = "Consultant")]
    public async Task<IActionResult> Chat(string userName)
    {
        var user = await _userManager.GetUserAsync(User);
        var consultant = await _repositoryConsultant.GetBySpecAsync(new ConsultantByUser(user));

        var chat = consultant?.Chats.FirstOrDefault(ch => ch.User.UserName == userName);

        if (chat is null)
        {
            return BadRequest();
        }

        var chatVm = _mapper.Map<ChatViewModel>(chat);
        
        return PartialView("~/Areas/User/Views/Partials/ChatPartial.cshtml", chatVm);
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var userWithChats = await _userRepository.GetBySpecAsync(new UserWithChatsSpec(user));
        var chat = userWithChats!.UserChats.FirstOrDefault(c => c.Status == ChatStatus.Opened);
        var chatVm = _mapper.Map<ChatViewModel>(chat);
        return View(chatVm);
    }

    [HttpPost, Authorize(Roles = "Consultant"), FormValidator]
    public async Task<IActionResult> CloseChatByConsultant(string userName)
    {
        var user = await _userManager.GetUserAsync(User);
        var consultant = await _repositoryConsultant.GetBySpecAsync(new ConsultantByUser(user));
        var chat = consultant!.Chats.FirstOrDefault(c => c.User.UserName == userName);
        
        if (chat is null)
        {
            return FormResult.CreateErrorResult("Chat is not exists or it is not your client");
        }
        
        if (chat.Status != ChatStatus.Opened)
        {
            return FormResult.CreateWarningResult($"Chat's status is not Opened: {chat.Status}");
        }

        chat.Status = ChatStatus.ClosedByConsultant;
        
        await _chatRepository.SaveChangesAsync();
        
        return FormResult.CreateSuccessResult("Chat' status was changed");
    }

    [HttpPost, FormValidator]
    public async Task<IActionResult> CloseChatByUser()
    {
        var user = await _userManager.GetUserAsync(User);
        var userWithChat = await _userRepository.GetBySpecAsync(new UserWithChatsSpec(user));
        var chat = userWithChat!.UserChats.FirstOrDefault(c => c.Status == ChatStatus.Opened);
        
        if (chat is null)
        {
            return FormResult.CreateErrorResult("Chat is not exists");
        }

        chat.Status = ChatStatus.ClosedByUser;
        await _chatRepository.SaveChangesAsync();
        return FormResult.CreateSuccessResult("Chat was closed", "/User/Profile/", 4000);
    }
}