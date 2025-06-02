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

    [HttpGet("{guid}")]
    public async Task<IActionResult> GetMetricByPlant(string guid)
    {
        var metric = await _metricService.GetByPlantAsync(guid);
        if (metric == null)
        {
            return NotFound();
        }
        return Ok(metric);
    }

    [HttpPost]
    public async Task<IActionResult> AddMetricAsync([FromBody] Metric metric)
    {
        await _metricService.AddAsync(metric);
        if (metric == null)
        {
            return NotFound();
        }
        return Ok(metric);
    }


    [HttpDelete("{guid}")]
    public async Task<IActionResult> DeleteMetricAsync(string guid)
    {
        try
        {
            await _metricService.DeleteAsync(guid);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

}