using Database;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Backend.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PlantController : Controller
{
   
    private readonly PlantService _plantService;

    public PlantController(PlantService plantService)
    {
        _plantService = plantService;

    }

    [HttpGet]
    public async Task<IActionResult> GetAllPlants()
    {
        var plant = await _plantService.GetAllAsync();
        if (plant == null)
        {
            return NotFound();
        }
        return Ok(plant);
    }

    [HttpGet("{guid}")]
    public async Task<IActionResult> GetPlantByGuid(string guid)
    {
        var plant = await _plantService.GetByGuidAsync(guid);
        if (plant == null)
        {
            return NotFound();
        }
        return Ok(plant);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddPlantAsync(Plant plant)
    {
        await _plantService.AddAsync(plant);
        if (plant == null)
        {
            return NotFound();
        }
        return Ok(plant);
    }
    
    [HttpPut("{guid}")]
    public async Task<IActionResult> UpdatePlantAsync(string GUID, Plant plant)
    {
        var existingPlant = await _plantService.GetByGuidAsync(GUID);
        if (existingPlant == null)
        {
            return NotFound();
        }

        await _plantService.UpdateAsync(plant);
        return Ok(plant);
    }
}