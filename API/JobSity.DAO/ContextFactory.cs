using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace JobSity.DAO
{
    public class ContextFactory : IDesignTimeDbContextFactory<JobSityContext>
    {
        public JobSityContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<JobSityContext>();
            //this code will be never executed in runtime only in design time
            builder.UseSqlServer(
         "Server=localhost,1433;Initial Catalog=ChatDB;User=sa;Password=#Chat#Job;TrustServerCertificate=True;");
            return new JobSityContext(builder.Options);
        }
    }
}
