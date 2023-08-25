using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_Models.Models
{
    public class Post
    {
        public int Id { get; set; }
        [MaxLength(700)]
        public string Title { get; set; }
        [MaxLength(800)]
        public string? Image { get; set; }
        [MaxLength(1000)]
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        // Navigation property for Comments
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<LikePost> LikePosts { get; set; }


    }
}
