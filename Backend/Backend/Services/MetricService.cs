using Database;
using Microsoft.EntityFrameworkCore;
using Models;

public class MetricService(MetricRepo metricRepo)
{
  


    private readonly MetricRepo _metricRepo = metricRepo;


    public async Task<IEnumerable<Metric>> GetByPlantAsync(string guid)
    {
        return await _metricRepo.GetByPlantAsync(guid);
    }


    public async Task<IEnumerable<Metric>> GetAllAsync()
    {
        return await _metricRepo.GetAllAsync();
    }


    public async Task AddAsync(Metric metric)
    {
        await _metricRepo.AddAsync(metric);
    }
}