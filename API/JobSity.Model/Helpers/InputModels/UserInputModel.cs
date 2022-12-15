using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSity.Model.Helpers.InputModels
{
    public class UserInputModel
    {
        [Required(ErrorMessage = "username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "password is required")]
        public string Password { get; set; }
    }
}
