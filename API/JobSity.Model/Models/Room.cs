using System.ComponentModel.DataAnnotations;

namespace JobSity.Model.Models
{
    public class Room : EntityBase
    {
        [Required]
        public string Name { get; set; }
    }
}
