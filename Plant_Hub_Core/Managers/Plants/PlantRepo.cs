using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Plant_Hub_Models.Models;
using Plant_Hub_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_Core.Managers.Plants
{
    public class PlantRepo : IPlant
    {
        private Plant_Hub_dbContext _dbContext;
        private IMapper _mapper;
        private IWebHostEnvironment _host;

        public PlantRepo(Plant_Hub_dbContext dbContext, IMapper mapper, IWebHostEnvironment host)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _host = host;
        }

        #region Public

            public ResponseApi CreatePlant(CreatePlantMV plant)
            {
                var existPlant = _dbContext.Plants.FirstOrDefault(x => x.PlantName == plant.PlantName);
                if (existPlant != null)
                {
                    return new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "plant name already exists",
                        Data = plant
                    };

                }
                string folder = "Uploads/PlantImage/";
                string imageURL = UploadImage(folder, plant.ImageFile);
                var newPlant = new Plant
                {
                    PlantName = plant.PlantName,
                    PlantNameAr = plant.PlantNameAr,
                    Description = plant.Description,
                    DescriptionAr = plant.DescriptionAr,
                    Image = imageURL,
                    CategoryId = plant.CategoryId,
                    CareDetails = plant.CareDetails,
                    CareDetailsAr = plant.CareDetailsAr,
                    Season = plant.Season,
                    SeasonAr = plant.SeasonAr,
                    MedicalBenefit = plant.MedicalBenefit,
                    MedicalBenefitAr = plant.MedicalBenefitAr
                };
                _dbContext.Plants.Add(newPlant);
                _dbContext.SaveChanges();
                return new ResponseApi
                {
                    IsSuccess = true,
                    Message = "successfully",
                    Data = newPlant
                };


            }

            public ResponseApi GetAllPlants(string userId , int langId)
            {
                var plants = _dbContext.Plants.Select(x => new {
                                                            plantId = x.Id,
                                                            PlantName = langId == 1 ? x.PlantName : x.PlantNameAr,
                                                            PlantDescription = langId == 1 ? x.Description : x.DescriptionAr,
                                                            PlantSeason = langId == 1 ? x.Season : x.SeasonAr,
                                                            PlantCareDetails = langId == 1 ? x.CareDetails : x.CareDetailsAr,
                                                            PlantMedicalBenefit = langId == 1 ? x.MedicalBenefit : x.MedicalBenefitAr,
                                                            PlantCategory = langId == 1 ? x.Category.CategoryName : x.Category.CategoryNameAr,
                                                            PlantImage = x.Image,
                                                            IsSaved = x.SavedPlants.Any(s => s.UserId == userId) ? x.SavedPlants.FirstOrDefault(s => s.UserId == userId).status : false
                }).ToList();
                if (!plants.Any())
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
                    Data = plants
                };


            }
            public ResponseApi GetPlantById(int plantId , int langId)
            {
                var plant = _dbContext.Plants.Find(plantId);
                if (plant == null)
                {
                    return new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "No data available",
                        Data = null
                    };
                }
                var plantInfo = _dbContext.Plants.Select(x => new
                                                                {
                                                                    PlantId = x.Id,
                                                                    PlantName = langId == 1 ? x.PlantName : x.PlantNameAr,
                                                                    PlantDescription = langId == 1 ? x.Description : x.DescriptionAr,
                                                                    PlantSeason = langId == 1 ? x.Season : x.SeasonAr,
                                                                    PlantCareDetails = langId == 1 ? x.CareDetails : x.CareDetailsAr,
                                                                    PlantMedicalBenefit = langId == 1 ? x.MedicalBenefit : x.MedicalBenefitAr,
                                                                    PlantCategory = langId == 1 ? x.Category.CategoryName : x.Category.CategoryNameAr,
                    PlantImage = x.Image
                                                                }).FirstOrDefault(z => z.PlantId == plantId);
                return new ResponseApi
                {
                    IsSuccess = true,
                    Message = "successfully",
                    Data = plantInfo
                };
            }

            public ResponseApi GetPlantByCategoryId(int categoryId, int langId)
            {
                var plant = _dbContext.Plants.ToList();
                if (!plant.Any() )
                {
                    return new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "No data available",
                        Data = null
                    };
                }
                var plants = _dbContext.Plants.Where(x =>x.CategoryId == categoryId).Select(x => new
                {
                    PlantId = x.Id,
                    PlantName = langId == 1 ? x.PlantName : x.PlantNameAr,
                    PlantDescription = langId == 1 ? x.Description : x.DescriptionAr,
                    PlantSeason = langId == 1 ? x.Season : x.SeasonAr,
                    PlantCareDetails = langId == 1 ? x.CareDetails : x.CareDetailsAr,
                    PlantMedicalBenefit = langId == 1 ? x.MedicalBenefit : x.MedicalBenefitAr,
                    PlantCategory = langId == 1 ? x.Category.CategoryName : x.Category.CategoryNameAr,
                    PlantImage = x.Image
                }).ToList();
                return new ResponseApi
                {
                    IsSuccess = true,
                    Message = "successfully",
                    Data = plants
                };
            }

           public ResponseApi SearchForPlants(String PlantName, int CategoryId, int langId, String userId )
            {
                var matchingUPlants= _dbContext.Plants.Where(u =>( u.PlantName.Contains(PlantName) || u.PlantNameAr.Contains(PlantName)) && u.CategoryId == CategoryId).Select(x => new {
                    plantId = x.Id,
                    PlantName = langId == 1 ? x.PlantName : x.PlantNameAr,
                    PlantDescription = langId == 1 ? x.Description : x.DescriptionAr,
                    PlantSeason = langId == 1 ? x.Season : x.SeasonAr,
                    PlantCareDetails = langId == 1 ? x.CareDetails : x.CareDetailsAr,
                    PlantMedicalBenefit = langId == 1 ? x.MedicalBenefit : x.MedicalBenefitAr,
                    PlantCategory = langId == 1 ? x.Category.CategoryName : x.Category.CategoryNameAr,
                    PlantImage = x.Image,
                    IsSaved = x.SavedPlants.Any(s => s.UserId == userId) ? x.SavedPlants.FirstOrDefault(s => s.UserId == userId).status : false
                }).ToList();
            if (!matchingUPlants.Any())
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
                    Message = "Successfully",
                    Data = matchingUPlants
                };
            }
            public ResponseApi UpdatePlantById( PlantMV plant)
            {
                var existPlant= _dbContext.Plants.Find(plant.PlantId);
                if (existPlant == null)
                {
                    return new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "No data available",
                        Data = null
                    };

                }
                var CheackPlantName = _dbContext.Plants.FirstOrDefault(x => x.PlantName == plant.PlantName);
                if (CheackPlantName != null)
                {
                    return new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "plant name already exists",
                        Data = null
                    };
                }

                string folder = "Uploads/PlantImage/";
                string imageURL = UploadImage(folder, plant.ImageFile);

                existPlant.PlantName = plant.PlantName;
                existPlant.Description = plant.Description;
                existPlant.Image = imageURL;
                existPlant.CareDetails = plant.CareDetails;
                existPlant.Season = plant.Season;
                existPlant.MedicalBenefit = plant.MedicalBenefit;
                existPlant.CategoryId = plant.CategoryId;

                _dbContext.SaveChanges();

                return new ResponseApi
                {
                    IsSuccess = true,
                    Message = "successfully",
                    Data = existPlant
                };
            }


            public ResponseApi DeletePlantById(int plantId)
            {
                var existPlant = _dbContext.Plants.Find(plantId);
                if (existPlant == null)
                {
                    return new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "No data available",
                        Data = null
                    };
                }
                _dbContext.Plants.Remove(existPlant);
                _dbContext.SaveChanges();
            return new ResponseApi
                {
                    IsSuccess = true,
                    Message = "successfully",
                    Data = null
                };
            }


        //Save Plant
            public ResponseApi SavePlant(string userId, int plantId)
            {
                var cheakIfExist = _dbContext.SavePlants.FirstOrDefault(x => x.UserId == userId && x.PlantId == plantId);

                if (cheakIfExist == null)
                {
                    var infoPlant = new SavePlant
                    {
                        PlantId = plantId,
                        UserId = userId,
                        status = true
                    };
                    _dbContext.SavePlants.Add(infoPlant);
                    _dbContext.SaveChanges();

                    return new ResponseApi
                    {
                        IsSuccess = true,
                        Message = "Successfully Saved",
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
                            Message = "Successfully Unsaved",
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
                            Message = "Successfully Saved",
                            Data = null
                        };
                    }
                }
            }
            public ResponseApi SearchForPreservedPlants(string plantName, String userId, int langId)
            {
                var matchingUPlants = _dbContext.SavePlants.Where(u => u.Plant.PlantName.Contains(plantName) || u.Plant.PlantNameAr.Contains(plantName)).Select(x => new {
                    plantId = x.Plant.Id,
                    PlantName = langId == 1 ? x.Plant.PlantName : x.Plant.PlantNameAr,
                    PlantDescription = langId == 1 ? x.Plant.Description : x.Plant.DescriptionAr,
                    PlantSeason = langId == 1 ? x.Plant.Season : x.Plant.SeasonAr,
                    PlantCareDetails = langId == 1 ? x.Plant.CareDetails : x.Plant.CareDetailsAr,
                    PlantMedicalBenefit = langId == 1 ? x.Plant.MedicalBenefit : x.Plant.MedicalBenefitAr,
                    PlantCategory = langId == 1 ? x.Plant.Category.CategoryName : x.Plant.Category.CategoryNameAr,
                    PlantImage = x.Plant.Image,
                }).ToList();
                if (!matchingUPlants.Any())
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
                    Message = "Successfully",
                    Data = matchingUPlants
                };
            }


        public ResponseApi GetPreservedPlants(string userId, int langId)
            {
                var Plants = _dbContext.SavePlants.Where(x => x.UserId == userId && x.status == true)
                                                .Select(x => new {
                                                                        IdToControl = x.Id,
                                                                        plantId = x.Plant.Id,
                                                                        PlantName = langId == 1 ? x.Plant.PlantName : x.Plant.PlantNameAr,
                                                                        PlantDescription = langId == 1 ? x.Plant.Description : x.Plant.DescriptionAr,
                                                                        PlantSeason = langId == 1 ? x.Plant.Season : x.Plant.SeasonAr,
                                                                        PlantCareDetails = langId == 1 ? x.Plant.CareDetails : x.Plant.CareDetailsAr,
                                                                        PlantMedicalBenefit = langId == 1 ? x.Plant.MedicalBenefit : x.Plant.MedicalBenefitAr,
                                                                        PlantCategory = langId == 1 ? x.Plant.Category.CategoryName : x.Plant.Category.CategoryNameAr,
                                                                        PlantImage = x.Plant.Image
                                                }).ToList();
                if (!Plants.Any())
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
                    Data = Plants
                };
            }

            public ResponseApi DeletePreservedPlant( int id)
            {
                var existPlant = _dbContext.SavePlants.Find(id);
                if (existPlant == null)
                {
                    return new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "Wrong Id",
                        Data = null
                    };
                }
                _dbContext.SavePlants.Remove(existPlant);
            _dbContext.SaveChanges();
            return new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Successfully",
                    Data = null
                };
            }
            public ResponseApi RemovePreservedPlant(String userId, int plantId)
            {
                var existPlant = _dbContext.SavePlants.FirstOrDefault(x => x.UserId == userId && x.PlantId == plantId);
                if (existPlant == null)
                {
                    return new ResponseApi
                    {
                        IsSuccess = false,
                        Message = "Wrong Id",
                        Data = null
                    };
                }
                var plant = _dbContext.SavePlants.Find(existPlant.Id);
                _dbContext.SavePlants.Remove(plant);
            _dbContext.SaveChanges();
            return new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Successfully",
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
        //{
        //        folder += Guid.NewGuid().ToString() + "_" + ImgeFile.FileName;
        //        string ImageURL = "/" + folder;
        //        string serverFolder = Path.Combine(_host.WebRootPath, folder);
        //        ImgeFile.CopyTo(new FileStream(serverFolder, FileMode.Create));
        //        return ImageURL;
        //    }
        #endregion private
    }
}
