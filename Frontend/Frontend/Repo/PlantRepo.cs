﻿using Frontend.Services;
using Models;

namespace Frontend.Repo
{
    public class PlantRepo 
    {
        PlantServices plantService = new PlantServices();
        public async Task<Plant> PlantData(string plantGuid)
        {
            if (string.IsNullOrEmpty(plantGuid)) return null; 
            return await plantService.PlantData(plantGuid);

        }
    }
}
