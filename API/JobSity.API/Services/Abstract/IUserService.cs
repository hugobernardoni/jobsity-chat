using JobSity.Model.Helpers.InputModels;
using Microsoft.AspNetCore.Identity;

namespace JobSity.API.Services.Abstract
{
    public interface IUserService
    {
        Task<IdentityUser> Login(string username, string password);
        Task<IdentityUser> Create(UserInputModel userInputModel);
        string GenerateToken(string username, string userId);
    }
}
