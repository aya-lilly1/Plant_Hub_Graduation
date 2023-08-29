using Plant_Hub_Models.Models;
using Plant_Hub_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_Core.Managers.Posts
{
    public interface IPost
    {
        ResponseApi CreatePost(string userId, PostMV post);
        ResponseApi GetAllPost(String userId);
        ResponseApi GetPostById(int postId, String userId);
        ResponseApi GetAllPostByUserId(string userId);
        ResponseApi UpdatePostById( PostMV post);
        ResponseApi DeletePostById(int PostId);

        ResponseApi AddComment(string userId, CommentMV comment);
        ResponseApi LikePostByUsre(string userId, int PostId);
        ResponseApi Deletelike(string userId, int PostId);

        List<Post> GetAllPosts();

    }
}
