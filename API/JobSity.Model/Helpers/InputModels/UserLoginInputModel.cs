using System.ComponentModel.DataAnnotations;

namespace JobSity.Model.Helpers.InputModels
{
    public class UserLoginInputModel
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
