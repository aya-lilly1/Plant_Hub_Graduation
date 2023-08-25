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

            public ResponseApi CreatePlant(PlantMV plant)
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
                    Description = plant.Description,
                    Image = imageURL,
                    CategoryId = plant.CategoryId,
                    CareDetails = plant.CareDetails,
                    Season = plant.Season,
                    MedicalBenefit = plant.MedicalBenefit
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

            public ResponseApi GetAllPlants(string userId)
            {
                var plants = _dbContext.Plants.Select(x => new {
                                                            plantId = x.Id,
                                                            PlantName = x.PlantName,
                                                            PlantDescription = x.Description,
                                                            PlantSeason = x.Season,
                                                            PlantCareDetails = x.CareDetails,
                                                            PlantMedicalBenefit = x.MedicalBenefit,
                                                            PlantCategory = x.Category.CategoryName,
                                                            PlantImage = x.Image,
                                                            IsSaved = x.SavedPlants.Any(s => s.UserId == userId)
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
            public ResponseApi GetPlantById(int plantId)
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
                                                                    PlantName = x.PlantName,
                                                                    PlantDescription = x.Description,
                                                                    PlantSeason = x.Season,
                                                                    PlantCareDetails = x.CareDetails,
                                                                    PlantMedicalBenefit = x.MedicalBenefit,
                                                                    PlantCategory = x.Category.CategoryName,
                                                                    PlantImage = x.Image
                                                                }).FirstOrDefault(z => z.PlantId == plantId);
                return new ResponseApi
                {
                    IsSuccess = true,
                    Message = "successfully",
                    Data = plantInfo
                };
            }

            public ResponseApi GetPlantByCategoryId(int categoryId)
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
                    PlantName = x.PlantName,
                    PlantDescription = x.Description,
                    PlantSeason = x.Season,
                    PlantCareDetails = x.CareDetails,
                    PlantMedicalBenefit = x.MedicalBenefit,
                    PlantCategory = x.Category.CategoryName,
                    PlantImage = x.Image
                }).ToList();
                return new ResponseApi
                {
                    IsSuccess = true,
                    Message = "successfully",
                    Data = plants
                };
            }


            public ResponseApi UpdatePlantById(int plantId, PlantMV plant)
            {
                var existPlant= _dbContext.Plants.Find(plantId);
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
                var infoPlant = new SavePlant
                {
                    PlantId = plantId,
                    UserId = userId
                };
                _dbContext.SavePlants.Add(infoPlant);
                _dbContext.SaveChanges();

                return new ResponseApi
                {
                    IsSuccess = true,
                    Message = "successfully",
                    Data = null
                };
            }

            public ResponseApi GetPreservedPlants(string userId)
            {
                var Plants = _dbContext.SavePlants.Where(x => x.UserId == userId)
                                                .Select(x => new {
                                                                        IdToControl = x.Id,
                                                                        plantId = x.Plant.Id,
                                                                        PlantName = x.Plant.PlantName,
                                                                        PlantDescription =x.Plant.Description,
                                                                        PlantSeason = x.Plant.Season,
                                                                        PlantCareDetails = x.Plant.CareDetails,
                                                                        PlantMedicalBenefit = x.Plant.MedicalBenefit,
                                                                        PlantCategory = x.Plant.Category.CategoryName,
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
                return new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Successfully",
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
                ImgeFile.CopyTo(new FileStream(serverFolder, FileMode.Create));
                return ImageURL;
            }
        #endregion private
    }
}
