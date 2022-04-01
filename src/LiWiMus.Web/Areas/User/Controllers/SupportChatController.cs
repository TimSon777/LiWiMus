using AutoMapper;
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
    private readonly IHubContext<SupportChatHub> _hubContext;
    private readonly IRepository<Chat> _chatRepository;
    private readonly UserManager<Core.Entities.User> _userManager;
    private readonly IMapper _mapper;

    public SupportChatController(IHubContext<SupportChatHub> hubContext, 
        IRepository<Chat> chatRepository, 
        UserManager<Core.Entities.User> userManager,
        IMapper mapper)
    {
        _hubContext = hubContext;
        _chatRepository = chatRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Roles = "Consultant")]
    public async Task<IActionResult> Chats()
    {
        var user = await _userManager.GetUserAsync(User);
        var chats = await _chatRepository.ListAsync(new ConsultantChatsWhenAccessSpec(user));
        return View(chats);
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