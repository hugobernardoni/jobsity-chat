using System.ComponentModel.DataAnnotations;

namespace JobSity.Model.Models
{
    public class Chat : EntityBase
    {
        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        [Required]
        public Guid RoomId { get; set; }
        public virtual Room Room { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }

    }
}
