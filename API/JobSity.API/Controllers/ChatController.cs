using AutoMapper;
using JobSity.API.ViewModels;
using JobSity.Model.Helpers.InputModels;
using JobSity.Model.Models;
using JobSity.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobSity.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/chat")]
    public class ChatController : Controller
    {
        private readonly IEntityBaseRepositoryAsync<Chat> _chatService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ChatController(IEntityBaseRepositoryAsync<Chat> chatService,
            IMapper mapper,
            IConfiguration configuration)
        {
            _chatService = chatService;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string roomId)
        {
            var chats = _chatService.FindBy(x => x.RoomId.ToString() == roomId)
                .OrderByDescending(x => x.TimeStamp)
                .Take(50)
                .ToList();            

            var chatViewModel = _mapper.Map<List<ChatViewModel>>(chats);

            return Ok(chatViewModel);
        }
    }
}
