using System.ComponentModel.DataAnnotations;

namespace JobSity.Model.Helpers.InputModels
{
    public class UserLoginInputModel
    {
        [Required(ErrorMessage = "username is required")]
        public string Username { get; set; }        

        [Required(ErrorMessage = "password is required")]
        public string Password { get; set; }
    }
}
