﻿using Plant_Hub_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_Core.Managers.Plants
{
    public interface IPlant
    {
        ResponseApi CreatePlant(CreatePlantMV plant);
        ResponseApi GetAllPlants(string userId, int langId);
        ResponseApi GetAllPlants();
        ResponseApi GetPlantById(int plantId, int langId);
        ResponseApi GetPlantByCategoryId(int categoryId, int langId);
        ResponseApi SearchForPlantsByCategory(String PlantName, int CategoryId, int langId, String userId);
        ResponseApi SearchForPlants(String PlantName, int langId, String userId);
        ResponseApi OrderToAddNewPlant(string userId, OrderAddNewPlantMV orderMv);
        ResponseApi GetAllOrderToAddNewPlant();
        ResponseApi DeleteOrderToAddNewPlant(int id);
		ResponseApi UpdatePlantById(UpdatePlantMV plant);
        ResponseApi DeletePlantById(int plantId);
        ResponseApi SavePlant(string userId, int plantId);
        ResponseApi SearchForPreservedPlants(string plantName, String userId, int langId);
        ResponseApi GetPreservedPlants(string userId, int langId);
        ResponseApi DeletePreservedPlant( int id);
        public ResponseApi RemovePreservedPlant(String userId, int plantId);

    }
}
