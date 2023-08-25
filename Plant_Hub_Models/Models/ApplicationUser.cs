
using Microsoft.AspNetCore.Identity;


namespace Plant_Hub_Models.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Image { get; set; }
        public int UserType { get; set; }

        public string FullName { get; set; }
        public int ConfirmationCode { get; set; }
        public ICollection<SavePlant> SavedPlants { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<LikePost> LikePosts { get; set; }
    }
}
