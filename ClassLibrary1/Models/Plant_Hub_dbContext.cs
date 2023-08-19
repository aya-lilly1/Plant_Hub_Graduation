using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Plant_Hub_Models.Models
{
   public partial class Plant_Hub_dbContext : IdentityDbContext<ApplicationUser>
    {
        public Plant_Hub_dbContext()
        {
        }

        public Plant_Hub_dbContext(DbContextOptions<Plant_Hub_dbContext> options)
            : base(options)
        {

        }
    }
}
