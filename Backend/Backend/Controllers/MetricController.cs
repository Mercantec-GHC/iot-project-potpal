using Database;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Backend.Controllers;


[ApiController]
[Route("api/[controller]")]
public class MetricController : Controller
{
   
    private readonly MetricService _metricService;

    public MetricController(MetricService metricService)
    {
        _metricService = metricService;
         
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMetrics()
    {
        var metric = await _metricService.GetAllAsync();
        if (metric == null)
        {
            return NotFound();
        }
        return Ok(metric);
    }

    [HttpGet("{token}")]
    public async Task<IActionResult> GetMetricByPlant(string token)
    {
        var metric = await _metricService.GetByPlantAsync(token);
        if (metric == null)
        {
            return NotFound();
        }
        return Ok(metric);
    }
    
    [HttpPost]public async Task<IActionResult> AddMetricAsync(Metric metric)
    {
        await _metricService.AddAsync(metric);
        if (metric == null)
        {
            return NotFound();
        }
        return Ok(metric);
    }

}