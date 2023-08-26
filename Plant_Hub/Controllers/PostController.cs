using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Plant_Hub_Core.Managers.Posts;
using Plant_Hub_ModelView;

namespace Plant_Hub.Controllers
{
    [Authorize]
    [ApiController]
    public class PostController : BaseController
    {
        private readonly IPost _post;
        private readonly IHttpContextAccessor __httpContextAccessor;
        public PostController(IPost post, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _post = post;
            __httpContextAccessor = httpContextAccessor;
        }
       
        [Route("api/Post/CreatePost")]
        [HttpPost]
        public IActionResult CreatePost([FromForm]PostMV post)
        {
            var res = _post.CreatePost(_UserId, post);
            return Ok(res);
        }

        [AllowAnonymous]
        [Route("api/Post/GetAllPost")]
        [HttpGet]
        public IActionResult GetAllPost()
        {
            var res = _post.GetAllPost(_UserId);
            return Ok(res);
        }


        [Route("api/Post/GetAllPostByUserId")]
        [HttpGet]
        public IActionResult GetAllPostByUserId()
        {
            var res = _post.GetAllPostByUserId(_UserId);
            return Ok(res);
        }

        [AllowAnonymous]
        [Route("api/Post/GetPostById")]
        [HttpGet]
        public IActionResult GetPostById(int postId)
        {
            var res = _post.GetPostById(postId);
            return Ok(res);
        }

        [Route("api/Post/UpdatePostById")]
        [HttpPost]
        public IActionResult UpdatePostById(int postid,[FromForm] PostMV post)
        {
            var res = _post.UpdatePostById(postid, post);
            return Ok(res);
        }

        [Route("api/Post/DeletePostById")]
        [HttpDelete]
        public IActionResult DeletePostById(int postId)
        {
            var res = _post.DeletePostById(postId);
            return Ok(res);
        }

        [Route("api/Post/AddComment")]
        [HttpPost]
        public IActionResult AddComment(CommentMV comment)
        {
            var res = _post.AddComment(_UserId,comment);
            return Ok(res);
        }
        [Route("api/Post/AddLike")]
        [HttpPost]
        public IActionResult LikePostByUsre(int postid)
        {
            var res = _post.LikePostByUsre(_UserId, postid);
            return Ok(res);
        }

        [Route("api/Post/Deletelike")]
        [HttpDelete]
        public IActionResult Deletelike(int postid)
        {
            var res = _post.Deletelike(_UserId, postid);
            return Ok(res);
        }
    }
}
