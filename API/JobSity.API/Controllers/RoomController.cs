using AutoMapper;
using JobSity.API.Services.Abstract;
using JobSity.API.ViewModels;
using JobSity.Model.Helpers.InputModels;
using JobSity.Model.Models;
using JobSity.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobSity.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/room")]
    public class RoomController : Controller
    {
        private readonly IEntityBaseRepositoryAsync<Room> _roomService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public RoomController(IEntityBaseRepositoryAsync<Room> roomService,
            IMapper mapper,
            IConfiguration configuration)
        {
            _roomService = roomService;
            _mapper = mapper;
            _configuration = configuration;
        }       

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RoomInputModel roomInputModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var room = _mapper.Map<Room>(roomInputModel);

                await _roomService.AddAsync(room);
                await _roomService.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var chats = await _roomService.GetAllAsync();

            var chatViewModel = _mapper.Map<List<RoomViewModel>>(chats);

            return Ok(chatViewModel);
        }
    }
}
