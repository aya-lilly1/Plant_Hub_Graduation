
using GoogleTranslateFreeApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Plant_Hub_Core.Managers.Posts;
using Plant_Hub_ModelView;
using System.Globalization;

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
        public IActionResult UpdatePostById([FromForm] PostMV post)
        {
            var res = _post.UpdatePostById( post);
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

        //[AllowAnonymous]
        //[Route("api/Post/GetAllPostssss")]
        //[HttpGet]
        //public IActionResult GetAllPostsss()
        //{
        //    //var res = _post.GetAllPosts();


        //    // استدعاء خدمة المدير
        //    var translationManagerService = ApplicationContext.Current.Services.TranslationManagerService;

        //    // تحديد معلمات النص الذي تريد ترجمته
        //    var sourceText = "Hello, world!";
        //    var sourceCulture = CultureInfo.InvariantCulture; // لغة المصدر
        //    var targetCulture = new CultureInfo("ar-SA");     // اللغة المستهدفة

        //    // إرسال النص للترجمة
        //    var result = translationManagerService.Translate(sourceText, sourceCulture, targetCulture);

        //    return Ok();
        //}
    }
}
