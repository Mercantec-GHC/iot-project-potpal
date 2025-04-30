using Models;


namespace Services
{
    public class MetricService
    {
        private readonly MetricRepo _repository;

        public MetricService(MetricRepo repository)
        {
            _repository = repository;
        }

        public async Task AddMetricAsync(Metric metric)
        {
            metric.Timestamp = DateTime.UtcNow;
            await _repository.AddAsync(metric);
        }

        public async Task<List<Metric>> GetMetricsByPlantAsync(string plantGuid)
        {
            return await _repository.GetByPlantGuidAsync(plantGuid);
        }
    }
}