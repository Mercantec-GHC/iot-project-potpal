using Microsoft.AspNetCore.Mvc;
using Models;
using Services;


namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetricsController : ControllerBase
    {
        private readonly MetricService _service;

        public MetricsController(MetricService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> PostMetric([FromBody] Metric metric)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.AddMetricAsync(metric);
            return Ok(new { message = "Metric saved successfully" });
        }

        [HttpGet("{plantGuid}")]
        public async Task<IActionResult> GetMetrics(string plantGuid)
        {
            var metrics = await _service.GetMetricsByPlantAsync(plantGuid);
            return Ok(metrics);
        }
    }
}