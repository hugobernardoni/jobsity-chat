using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSity.Model.Helpers.InputModels
{
    public class RoomInputModel
    {
        [Required(ErrorMessage = "Room name is required")]
        public string Name { get; set; }
    }
}
