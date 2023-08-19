
using Microsoft.AspNetCore.Identity;


namespace Plant_Hub_Models.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Image { get; set; }
        public int UserType { get; set; }

        public string FullName { get; set; }
    }
}
