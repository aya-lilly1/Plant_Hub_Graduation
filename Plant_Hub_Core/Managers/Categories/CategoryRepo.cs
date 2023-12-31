﻿using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Plant_Hub_Core.Helper;
using Plant_Hub_Models.Models;
using Plant_Hub_ModelView;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_Core.Managers.Categories
{
    public class CategoryRepo : ICatregory
    {
        private Plant_Hub_dbContext _dbContext;
        private IMapper _mapper;
        //private IWebHostEnvironment _host;
        private readonly IFileManagement _fileManagement;

        public CategoryRepo(Plant_Hub_dbContext dbContext, IMapper mapper, IFileManagement fileManagement)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _fileManagement = fileManagement;
        }

        #region Public

            public async Task<ResponseApi> CreateCategoty(CreateCategoryMV category)
            {
                var existCatEn = _dbContext.Categories.FirstOrDefault(x => x.CategoryName == category.CategoryName );
                if (existCatEn != null)
                {
                    return new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "category English name already exists",
                        Data = category
                    };

                    }
                var existCatAr = _dbContext.Categories.FirstOrDefault(x => x.CategoryNameAr == category.CategoryNameAr);

                if (existCatAr != null)
                {
                    return new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "category Arabic name already exists",
                        Data = category
                    };

                }
                //    string folder = "/Uploads/CategoryImage/";
                //string imageURL = UploadImage(folder, category.ImageFile);

                string imageURL = await _fileManagement.Upload(category.ImageFile, "Uploads", "Images");

                var newCategory = new Category
                    {
                        CategoryName = category.CategoryName,
                        Description = category.Description,
                        CategoryNameAr = category.CategoryNameAr,
                        DescriptionAr = category.DescriptionAr,
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

            public ResponseApi GetAllCategories(int langId)
        {
            
                var localizedCategories = _dbContext.Categories.Select(s => new
                {
                    Id = s.Id,
                    CategoryName = langId == 1 ? s.CategoryName : s.CategoryNameAr,
                    Description = langId == 1 ? s.Description : s.DescriptionAr,
                    image = s.Image
                }).ToList();
                if (!localizedCategories.Any())
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
                    Data = localizedCategories
                };
            

          
              }
            public ResponseApi GetAllCategories()
            {
                var categories = _dbContext.Categories.Select(s => new
                {
                    Id = s.Id,
                    CategoryName =  s.CategoryName,
                    CategoryNameAr = s.CategoryNameAr,
                    Description =  s.Description,
                    DescriptionAr = s.DescriptionAr,
                    image = s.Image
                }).ToList();
                if (!categories.Any())
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

            public ResponseApi GetCategoryById(int categortId, int langId)
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
                        Data = new
                        {
                            Id = category.Id,
                            CategoryName = langId == 1 ? category.CategoryName : category.CategoryNameAr,
                            Description = langId == 1 ? category.Description : category.DescriptionAr,
                            image = category.Image
                        }
                    };
               
                 
            }

            public ResponseApi SearchForCategory(String categoryName, int langId)
            {
                var matchingCategory = _dbContext.Categories.Where(u => u.CategoryName.Contains(categoryName) || u.CategoryNameAr.Contains(categoryName)).ToList();
                if (!matchingCategory.Any())
                {
                    return new ResponseApi()
                    {
                        IsSuccess = true,
                        Message = "No Data Available",
                        Data = null
                    };
                }
                var localizedCategories = matchingCategory.Select(category => new
                {
                    Id = category.Id,
                    CategoryName = langId == 1 ? category.CategoryName : category.CategoryNameAr,
                    Description = langId == 1 ? category.Description : category.DescriptionAr,
                    image = category.Image
                }).ToList();

            return new ResponseApi()
                {
                    IsSuccess = true,
                    Message = "Successfully",
                    Data = localizedCategories

            };
            
            }
            public async Task<ResponseApi> UpdateCategoryById(UpdateCategoryMV category)
                {
                    var existCategory = _dbContext.Categories.Find(category.CategoryId);
                    if (existCategory == null)
                    {
                        return new ResponseApi
                        {
                            IsSuccess = false,
                            Message = "No data available",
                            Data = null
                        };

                    }
                    var CheackCatName = _dbContext.Categories.FirstOrDefault(x =>( x.CategoryName == category.CategoryName && x.Id !=category.CategoryId) || (x.CategoryNameAr == category.CategoryNameAr && x.Id != category.CategoryId));
                    if (CheackCatName != null)
                    { 
                        return new ResponseApi
                        {
                            IsSuccess = false,
                            Message = "Category name already exists",
                            Data = null
                        };
                    }

                    //string imageURL = "Uploads/CategoryImage/";
                    //string imageURL = UploadImage(folder, category.ImageFile);

                    existCategory.CategoryName = category.CategoryName;
                    existCategory.Description = category.Description;
                    existCategory.CategoryNameAr = category.CategoryNameAr;
                    existCategory.DescriptionAr = category.DescriptionAr;
                    if (category.ImageFile != null)
                    {
                      string imageURL = await _fileManagement.Upload(category.ImageFile, "Uploads", "Images");
                      existCategory.Image = imageURL;
                    }

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



        //private string UploadImage(string folder, IFormFile imageFile)
        //{
        //    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
        //    string serverFolderPath = Path.Combine(_host.WebRootPath, folder);
        //    string serverFilePath = Path.Combine(serverFolderPath, uniqueFileName);

        //    Directory.CreateDirectory(serverFolderPath);

        //    using (var fileStream = new FileStream(serverFilePath, FileMode.Create))
        //    {
        //        imageFile.CopyTo(fileStream);
        //    }

        //    string imageURL = "/" + Path.Combine(folder, uniqueFileName).Replace("\\", "/");
        //    return imageURL;
        //}

        //private string UploadImage(string folder, IFormFile ImgeFile)
        //{
        //    folder += Guid.NewGuid().ToString() + "_" + ImgeFile.FileName;
        //    string ImageURL = "/" + folder;
        //    string serverFolder = Path.Combine(_host.WebRootPath, folder);
        //    Directory.CreateDirectory(serverFolder);
        //    ImgeFile.CopyTo(new FileStream(serverFolder, FileMode.Create));
        //    return ImageURL;
        //}

        #endregion private
    }
}
