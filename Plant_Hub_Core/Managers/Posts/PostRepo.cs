﻿using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Plant_Hub_Models.Models;
using Plant_Hub_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_Core.Managers.Posts
{
    public class PostRepo : IPost
    {
        private Plant_Hub_dbContext _dbContext;
        private IMapper _mapper;
        private IWebHostEnvironment _host;

        public PostRepo(Plant_Hub_dbContext dbContext, IMapper mapper, IWebHostEnvironment host)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _host = host;
        }
        #region Public
            public ResponseApi CreatePost(string userId, PostMV post)
            {
                string folder = "Uploads/PostImage/";
                string imageURL = UploadImage(folder, post.ImageFile);
                var newPost = new Post()
                      {
                         Title = post.Title,
                         Content = post.Content,
                         CreatedDate = DateTime.Now,
                         Image = imageURL,
                         UserId = userId
                };
                _dbContext.Posts.Add(newPost);
                _dbContext.SaveChanges();
                return new ResponseApi()
                    { 
                        IsSuccess = true,
                        Message = "successfully",
                        Data = newPost
                    };

            }


        public List<Post> GetAllPosts()
        {
            var posts = _dbContext.Posts.ToList();
            return posts;

        }
            public ResponseApi GetAllPost(String userId)
            {
                var posts =_dbContext.Posts.Select(x => new
                                                {
                                                    Username = x.User.UserName,
                                                    UserImage = x.User.Image,
                                                    PostId = x.Id,
                                                    CretedDate = x.CreatedDate,
                                                    PostTitel = x.Title,
                                                    PostContent = x.Content,
                                                    PostImage = x.Image,
                                                    IsLiked = x.LikePosts.Any(x => x.UserId == userId) ? x.LikePosts.FirstOrDefault(s => s.UserId == userId).status : false,
                                                    LikeCount = x.LikePosts.Count(p => p.status == true),
                                                    CommentCount = x.Comments.Count(),
                                                    Comments = x.Comments.Select(comment => new
                                                          {
                                                            CommentId = comment.Id,
                                                            CommentContent = comment.Content,
                                                            CommentCreatedDate = comment.CreatedDate,
                                                            CommentUserName = comment.User.FullName ,
                                                            CommentUserImage = comment.User.Image

                                                         }).OrderBy(x => x.CommentCreatedDate).ToList()
                                              }).OrderByDescending(x=>x.CretedDate).ToList();
                if (!posts.Any())
                {
                    return new ResponseApi()
                    {
                        IsSuccess = true,
                        Message = "No Data Available",
                        Data = null
                    };

                }
                return new ResponseApi()
                {
                    IsSuccess = true,
                    Message = "successfully",
                    Data = posts
                };
            }

            public ResponseApi GetPostById(int postId, String userId)
            {
                var posts = _dbContext.Posts.Where(x => x.Id == postId).Select(x => new
                {
                    Username = x.User.UserName,
                    UserImage = x.User.Image,
                    PostId = x.Id,
                    CretedDate = x.CreatedDate,
                    PostTitel = x.Title,
                    PostContent = x.Content,
                    PostImage = x.Image,
                    IsLiked = x.LikePosts.Any(x => x.UserId == userId) ? x.LikePosts.FirstOrDefault(s => s.UserId == userId).status : false,
                    LikeCount = x.LikePosts.Count(p => p.status == true),
                    CommentCount = x.Comments.Count(c => c.PostId == postId),
                    Comments = x.Comments.Select(comment => new
                    {
                        CommentId = comment.Id,
                        CommentContent = comment.Content,
                        CommentCreatedDate = comment.CreatedDate,
                        CommentUserName = comment.User.FullName,
                        CommentUserImage = comment.User.Image

                    }).OrderBy(x => x.CommentCreatedDate).ToList()
                }).OrderByDescending(x => x.CretedDate).ToList();
                if (!posts.Any())
                {
                    return new ResponseApi()
                    {
                        IsSuccess = true,
                        Message = "No Data Available",
                        Data = null
                    };

                }
                return new ResponseApi()
                {
                    IsSuccess = true,
                    Message = "successfully",
                    Data = posts
                };
            }
            public ResponseApi GetAllPostByUserId(string userId)
            {
                var posts = _dbContext.Posts.Where(x => x.UserId == userId).Select(x => new
                {
                    Username = x.User.UserName,
                    UserImage = x.User.Image,
                    PostId = x.Id,
                    CretedDate = x.CreatedDate,
                    PostTitel = x.Title,
                    PostContent = x.Content,
                    PostImage = x.Image,
                    IsLiked = x.LikePosts.Any(x => x.UserId == userId) ? x.LikePosts.FirstOrDefault(s => s.UserId == userId).status : false,
                    LikeCount = x.LikePosts.Count(p => p.status == true),
                    CommentCount = x.Comments.Count(),
                    Comments = x.Comments.Select(comment => new
                    {
                        CommentId = comment.Id,
                        CommentContent = comment.Content,
                        CommentCreatedDate = comment.CreatedDate,
                        CommentUserName = comment.User.FullName,
                        CommentUserImage = comment.User.Image

                    }).ToList()
                }).OrderByDescending(x => x.CretedDate).ToList();
                if (!posts.Any())
                {
                    return new ResponseApi()
                    {
                        IsSuccess = true,
                        Message = "No Data Available",
                        Data = null
                    };

                }
                return new ResponseApi()
                {
                    IsSuccess = true,
                    Message = "successfully",
                    Data = posts
                };
            }

            public ResponseApi UpdatePostById( PostMV post)
            {
                var existPost = _dbContext.Posts.Find(post.PostId);
                if (existPost == null)
                {
                    return new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "Invalid Id",
                        Data = null
                    };
                }
                string folder = "Uploads/PostImage/";
                string imageURL = UploadImage(folder, post.ImageFile);
                existPost.Title = post.Title;
                existPost.Content = post.Content;
                existPost.Image = imageURL;
                _dbContext.SaveChanges();
                return new ResponseApi
                {
                    IsSuccess = true,
                    Message = "successfully",
                    Data = null
                };

            }

            

            public ResponseApi DeletePostById(int PostId)
            {
                var existPost = _dbContext.Posts.Include(p => p.Comments).FirstOrDefault( x =>x.Id == PostId);
                if (existPost == null)
                {
                    return new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "No data available",
                        Data = null
                    };
                }
                _dbContext.Comments.RemoveRange(existPost.Comments);
                _dbContext.Posts.Remove(existPost);
                _dbContext.SaveChanges();
                return new ResponseApi
                    {
                        IsSuccess = true,
                        Message = "successfully",
                        Data = null
                    };
            }

            //comment
            public ResponseApi AddComment(string userId,CommentMV comment)
            {
                var existPost = _dbContext.Posts.Find(comment.PostId);
                if (existPost == null)
                {
                    return new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "Invalid Id",
                        Data = null
                    };
                }
                var newComment = new Plant_Hub_Models.Models.Comment()
                {
                    Content = comment.Content,
                    CreatedDate = DateTime.Now,
                    UserId = userId,
                    PostId = comment.PostId,
                };
                _dbContext.Comments.Add(newComment);
                _dbContext.SaveChanges();
                return new ResponseApi
                {
                    IsSuccess = true,
                    Message = "successfully",
                    Data = newComment
                };

            }

            //Like Post
            public ResponseApi LikePostByUsre(string userId, int postId)
            {
            
                var cheakIfExist = _dbContext.LikePosts.FirstOrDefault(x => x.UserId == userId && x.PostId == postId);

                if (cheakIfExist == null)
                {


                    var info = new LikePost
                    {
                        PostId = postId,
                        UserId = userId,
                        status = true
                    };
                    _dbContext.LikePosts.Add(info);
                    _dbContext.SaveChanges();

                    return new ResponseApi
                    {
                        IsSuccess = true,
                        Message = "successfully Like",
                        Data = null
                    };
                }
                else
                {
                    if (cheakIfExist.status == true)
                    {
                        cheakIfExist.status = false;
                        _dbContext.SaveChanges();
                        return new ResponseApi
                        {
                            IsSuccess = true,
                            Message = "Successfully Unlike",
                            Data = null
                        };
                    }
                    else
                    {
                        cheakIfExist.status = true;
                        _dbContext.SaveChanges();
                        return new ResponseApi
                        {
                            IsSuccess = true,
                            Message = "Successfully like",
                            Data = null
                        };
                    }
                }
            }

            public ResponseApi Deletelike(string userId, int PostId)
            {
                var info = _dbContext.LikePosts.FirstOrDefault(x => x.UserId == userId && x.PostId == PostId);
                if (info == null)
                {
                    return new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "Invalid Id Or Login",
                        Data = null
                    };
                }
                _dbContext.LikePosts.Remove(info);
                _dbContext.SaveChanges();

                return new ResponseApi
                {
                    IsSuccess = true,
                    Message = "successfully",
                    Data = null
                };
            }




        #endregion Public 

        #region private
        private string UploadImage(string folder, IFormFile imageFile)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            string serverFolderPath = Path.Combine(_host.WebRootPath, folder);
            string serverFilePath = Path.Combine(serverFolderPath, uniqueFileName);

            Directory.CreateDirectory(serverFolderPath);

            using (var fileStream = new FileStream(serverFilePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }

            string imageURL = "/" + Path.Combine(folder, uniqueFileName).Replace("\\", "/");
            return imageURL;
        }
        //private string UploadImage(string folder, IFormFile ImgeFile)
        //    {
        //        folder += Guid.NewGuid().ToString() + "_" + ImgeFile.FileName;
        //        string ImageURL = "/" + folder;
        //        string serverFolder = Path.Combine(_host.WebRootPath, folder);
        //        ImgeFile.CopyTo(new FileStream(serverFolder, FileMode.Create));
        //        return ImageURL;
        //    }
        #endregion private
    }
}
