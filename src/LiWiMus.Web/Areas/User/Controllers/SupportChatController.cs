using AutoMapper;
using LiWiMus.Core.Constants;
using LiWiMus.Core.Entities;
using LiWiMus.Core.Specifications;
using LiWiMus.SharedKernel.Interfaces;
using LiWiMus.Web.Areas.User.ViewModels;
using LiWiMus.Web.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LiWiMus.Web.Areas.User.Controllers;


[Area("User")]
public class SupportChatController : Controller
{
    private readonly IRepository<OnlineConsultant> _repositoryConsultant;
    private readonly IRepository<Chat> _chatRepository;
    private readonly UserManager<Core.Entities.User> _userManager;
    private readonly IMapper _mapper;

    public SupportChatController(IRepository<OnlineConsultant> repositoryConsultant,
        IRepository<Chat> chatRepository, 
        UserManager<Core.Entities.User> userManager,
        IMapper mapper)
    {
        _repositoryConsultant = repositoryConsultant;
        _chatRepository = chatRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Roles = "Consultant")]
    public async Task<IActionResult> Chats()
    {
        var user = await _userManager.GetUserAsync(User);
        var consultant = await _repositoryConsultant.GetBySpecAsync(new ConsultantByUser(user));
        var chats = await _chatRepository.ListAsync(new ConsultantChatsSpec(consultant!));
        var chatVms = _mapper.Map<List<ChatViewModel>>(chats);
        return View(chatVms);
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
        var chat = await _chatRepository.GetBySpecAsync(new OpenChatSpec(user));
        var chatVm = _mapper.Map<ChatViewModel>(chat);
        return PartialView("~/Areas/User/Views/Partials/ChatPartial.cshtml", chatVm);
    }
}