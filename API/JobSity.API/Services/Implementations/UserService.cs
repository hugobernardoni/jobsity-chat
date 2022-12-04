using JobSity.API.Core.JWT;
using JobSity.API.Services.Abstract;
using JobSity.Model.Helpers.InputModels;
using JobSity.Model.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace JobSity.API.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<User> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IdentityUser> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return null;

            if (!await _userManager.CheckPasswordAsync(user, password))
                return null;

            return user;
        }

        public async Task<IdentityUser> Create(UserInputModel userInputModel)
        {
            var user = new User
            {
                UserName = userInputModel.Username,
                Email = userInputModel.Email
            };

            var result = await _userManager.CreateAsync(user, userInputModel.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                throw new Exception(string.Join(" - ", errors));
            }

            return await _userManager.FindByNameAsync(userInputModel.Username);
        }

        public string GenerateToken(string username, string userId)
        {
            var token = new JwtTokenBuilder(_configuration)
                         .AddClaim(ClaimTypes.Name.ToString(), username)
                         .AddClaim("UserId", userId);

            return token.Build().Value;
        }
    }
}
