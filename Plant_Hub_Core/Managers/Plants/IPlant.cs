using Plant_Hub_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_Core.Managers.Plants
{
    public interface IPlant
    {
        ResponseApi CreatePlant(PlantMV plant);
        ResponseApi GetAllPlants(string userId);
        ResponseApi GetPlantById(int plantId);
        ResponseApi GetPlantByCategoryId(int categoryId);
        ResponseApi UpdatePlantById(int plantId, PlantMV plant);
        ResponseApi DeletePlantById(int plantId);
        ResponseApi SavePlant(string userId, int plantId);
        ResponseApi GetPreservedPlants(string userId);
        ResponseApi DeletePreservedPlant( int id);
        public ResponseApi RemovePreservedPlant(String userId, int plantId);

    }
}
