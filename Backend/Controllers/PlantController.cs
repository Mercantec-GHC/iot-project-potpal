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

    [HttpGet("byGuid/{guid}")]
    public async Task<IActionResult> GetPlantByGuid(string guid)
    {
        var plant = await _plantService.GetByGuidAsync(guid);
        if (plant == null)
        {
            return NotFound();
        }
        return Ok(plant);
    }

    [HttpGet("byUser/{email}")]
    public async Task<IActionResult> GetPlantByUser(string email)
    {
        var plant = await _plantService.GetByUserAsync(email);
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
    public async Task<IActionResult> UpdatePlantAsync(string guid, Plant plant)
    {
        var existingPlant = await _plantService.GetByGuidAsync(guid);
        if (existingPlant == null)
        {
            return NotFound();
        }

        await _plantService.UpdateAsync(plant);
        return Ok(plant);
    }
}