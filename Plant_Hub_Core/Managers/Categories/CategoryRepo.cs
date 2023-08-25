using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Plant_Hub_Models.Models;
using Plant_Hub_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_Core.Managers.Categories
{
    public class CategoryRepo : ICatregory
    {
        private Plant_Hub_dbContext _dbContext;
        private IMapper _mapper;
        private IWebHostEnvironment _host;

        public CategoryRepo(Plant_Hub_dbContext dbContext, IMapper mapper, IWebHostEnvironment host)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _host = host;
        }

        #region Public

            public ResponseApi CreateCategoty(CategoryMV category)
            {
                var existCat = _dbContext.Categories.FirstOrDefault(x => x.CategoryName == category.CategoryName);
                if (existCat != null)
                {
                    return new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "category name already exists",
                        Data = category
                    };

                }
                string folder = "/Uploads/CategoryImage/";
                string imageURL = UploadImage(folder, category.ImageFile);
                var newCategory = new Category
                {
                    CategoryName = category.CategoryName,
                    Description = category.Description,
                    Image = imageURL
                };
                _dbContext.Categories.Add(newCategory);
                _dbContext.SaveChanges();
                return new ResponseApi
                {
                    IsSuccess = true,
                    Message = "successfully",
                    Data = category
                };


            }

            public ResponseApi GetAllCategories( )
            {
                var categories = _dbContext.Categories.ToList();
                if ( !categories.Any() )
                {
                    return new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "No data available",
                        Data = null
                    };

                }
              
                return new ResponseApi
                {
                    IsSuccess = true,
                    Message = "successfully",
                    Data = categories
                };


            }
            public ResponseApi GetCategoryById(int categortId)
            {
                var category = _dbContext.Categories.Find(categortId);
                if(category == null)
                {
                    return new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "No data available",
                        Data = null
                    };
                }
                return new ResponseApi
                {
                    IsSuccess = true,
                    Message = "successfully",
                    Data = category
                };
            }
            public ResponseApi UpdateCategoryById(int categortId, CategoryMV category)
        {
            var existCategory = _dbContext.Categories.Find(categortId);
            if (existCategory == null)
            {
                return new ResponseApi
                {
                    IsSuccess = false,
                    Message = "No data available",
                    Data = null
                };

            }
            var CheackCatName = _dbContext.Categories.FirstOrDefault(x => x.CategoryName == category.CategoryName);
            if (CheackCatName != null)
            {
                return new ResponseApi
                {
                    IsSuccess = false,
                    Message = "Category name already exists",
                    Data = null
                };
            }

            string folder = "Uploads/CategoryImage/";
            string imageURL = UploadImage(folder, category.ImageFile);

            existCategory.CategoryName = category.CategoryName;
            existCategory.Description = category.Description;
            existCategory.Image = imageURL;

            _dbContext.SaveChanges();

            return new ResponseApi
            {
                IsSuccess = true,
                Message = "successfully",
                Data = existCategory
            };

            }

            public ResponseApi DeleteCategoryById(int categortId)
            {
                var existCategory = _dbContext.Categories.Find(categortId);
                if (existCategory == null)
                {
                    return new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "No data available",
                        Data = null
                    };
                }
                _dbContext.Categories.Remove(existCategory);
                return new ResponseApi
                {
                    IsSuccess = true,
                    Message = "successfully",
                    Data = null
                };
            }

        #endregion Public 

        #region private
        private string UploadImage(string folder, IFormFile ImgeFile)
        {
            folder += Guid.NewGuid().ToString() + "_" + ImgeFile.FileName;
            string ImageURL = "/" + folder;
            string serverFolder = Path.Combine(_host.WebRootPath, folder);
            Directory.CreateDirectory(serverFolder);
            ImgeFile.CopyTo(new FileStream(serverFolder, FileMode.Create));
            return ImageURL;
        }

        #endregion private
    }
}
