using AutoMapper;
using LiWiMus.Core.Chats;
using LiWiMus.Core.Chats.Enums;
using LiWiMus.Core.Chats.Specifications;
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
        
        return PartialView("~/Areas/User/Views/Partials/ChatPartial.cshtml", (chatVm, chatVm.Messages.Count));
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var chat = await _chatRepository.GetBySpecAsync(new OpenChatSpec(user));
        var chatVm = _mapper.Map<ChatViewModel>(chat);
        return View(chatVm);
    }
}