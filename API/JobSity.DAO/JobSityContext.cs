using JobSity.Model.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobSity.DAO
{
    public class JobSityContext : IdentityDbContext<User>
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Chat> Chats { get; set; }

        public JobSityContext(DbContextOptions<JobSityContext> options) : base(options)
        {
        }
    }
}