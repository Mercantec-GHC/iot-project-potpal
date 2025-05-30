using Backend.Models;
using Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlantController : ControllerBase
{
    private readonly PlantService _plantService;

    public PlantController(PlantService plantService)
    {
        _plantService = plantService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPlants()
    {
        var plants = await _plantService.GetAllAsync();
        if (plants == null || !plants.Any())
        {
            return NotFound("No plants found.");
        }
        return Ok(plants);
    }

    [HttpGet("byGuid")]
    [Authorize]
    public async Task<IActionResult> GetPlantByGuid(string guid)
    {
        var emailFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(string.IsNullOrEmpty(emailFromToken))
        {
            return Unauthorized("User not authenticated or error");
        }


        var plant = await _plantService.GetByGuidAsync(guid, emailFromToken);
        if (plant == null)
        {
            return NotFound("Plant not found.");
        }
        return Ok(plant);
    }

    [HttpGet("byUser/{email}")]
    public async Task<IActionResult> GetPlantByUser(string email)
    {
        var plants = await _plantService.GetByUserAsync(email);
        if (plants == null || !plants.Any())
        {
            return NotFound("No plants found for this user.");
        }
        return Ok(plants);
    }

    [HttpPost]
    public async Task<IActionResult> AddPlantAsync([FromBody] Plant plant)
    {
        if (plant == null)
        {
            return BadRequest("Invalid plant object.");
        }

        // Map email from user object if it's included
        if (plant.User != null && !string.IsNullOrEmpty(plant.User.Email))
        {
            plant.UserEmail = plant.User.Email;
        }

        try
        {
            await _plantService.AddAsync(plant);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("UpdatePlant")]
    [Authorize]
    public async Task<IActionResult> UpdatePlantAsync([FromBody] PlantDTO plant)
    {
        var emailFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(emailFromToken))
        {
            return Unauthorized("User not authenticated or error");
        }

        var existingPlant = await _plantService.GetByGuidAsync(plant.GUID, emailFromToken);
        if (existingPlant == null)
        {
            return NotFound("Plant not found.");
        }

        await _plantService.UpdateAsync(plant);
        return Ok();
    }
}
