using AutoMapper;
using JobSity.API.Services.Abstract;
using JobSity.API.ViewModels;
using JobSity.Model.Helpers.InputModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobSity.API.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IUserService userService,
            IMapper mapper,
            IConfiguration configuration)
        {
            _userService = userService;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginInputModel userLoginInputModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.Login(userLoginInputModel.Username, userLoginInputModel.Password);
            if (user == null)
                return Unauthorized("Invalid username or password");

            var accessToken = _userService.GenerateToken(user.UserName, user.Id);


            var userLoginViewModel = _mapper.Map<UserLoginViewModel>(user);
            userLoginViewModel.AccessToken = accessToken;

            return Ok(userLoginViewModel);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserInputModel userInputModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                IdentityUser user = await _userService.Create(userInputModel);

                if (user == null)
                    return NotFound("User not found");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
