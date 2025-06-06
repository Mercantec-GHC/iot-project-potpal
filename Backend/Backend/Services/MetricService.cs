using Models;

public class MetricService
{
    private readonly MetricRepo _metricRepo;
    private readonly EmailService _emailService;

    public MetricService(MetricRepo metricRepo, EmailService emailService)
    {
        _metricRepo = metricRepo;
        _emailService = emailService;
    }

    // Tolerance values for alerts (can be moved to config or Plant entity)
    private const double SoilMoistureTolerance = 5.0;
    private const double LightLevelTolerance = 5.0;
    private const double AirHumidityTolerance = 5.0;

    public async Task<List<Metric>> GetByPlantAsync(string plantGuid)
    {

        return await _metricRepo.GetByPlantGUIDAsync(plantGuid);

    }


    public async Task<IEnumerable<Metric>> GetAllAsync()
    {
        return await _metricRepo.GetAllAsync();
    }




    public async Task AddAsync(Metric metric)
    {
        var plant = await _metricRepo.GetPlantWithUserAsync(metric.PlantGUID);
        if (plant == null)
        {
            throw new Exception($"Plant with GUID '{metric.PlantGUID}' not found.");
        }
        var user = plant.User;
        if (plant.User == null)
        {
            throw new Exception($"User with email '{plant.UserEmail}' not found.");
        }

        await _metricRepo.AddAsync(metric);

        // Send alerts if values deviate beyond ideal ± tolerance
        // Soil moisture - low
        if (metric.SoilMoisture < plant.IdealSoilMoisture - SoilMoistureTolerance)
        {
            var body = EmailFactory.GetLowMoistureEmail(
                user.UserName ?? user.Email,
                plant.PlantName,
                metric.SoilMoisture.ToString(),
                plant.IdealSoilMoisture.ToString());

            var email = new Email
            {
                Recipient = user.Email,
                Subject = $"PotPal: Water your plant '{plant.PlantName}'",
                Body = body
            };
            await _emailService.SendAsync(email);
        }


        // Light level - low
        if (metric.LightLevel < plant.IdealLightLevel - LightLevelTolerance)
        {
            var body = EmailFactory.GetLightEmailAlert(
                user.UserName ?? user.Email,
                plant.PlantName,
                metric.LightLevel.ToString(),
                plant.IdealLightLevel.ToString());

            var email = new Email
            {
                Recipient = user.Email,
                Subject = $"PotPal: Light level too low for '{plant.PlantName}'",
                Body = body
            };
            await _emailService.SendAsync(email);
        }
        // Light level - high
        else if (metric.LightLevel > plant.IdealLightLevel + LightLevelTolerance)
        {
            var body = EmailFactory.GetLightEmailAlert(
                user.UserName ?? user.Email,
                plant.PlantName,
                metric.LightLevel.ToString(),
                plant.IdealLightLevel.ToString());

            var email = new Email
            {
                Recipient = user.Email,
                Subject = $"PotPal: Light level too high for '{plant.PlantName}'",
                Body = body
            };
            await _emailService.SendAsync(email);
        }

        // Air humidity - low
        if (metric.AirHumidity < plant.IdealAirHumidity - AirHumidityTolerance)
        {
            var body = EmailFactory.GetHumidityAlertEmail(
                user.UserName ?? user.Email,
                plant.PlantName,
                metric.AirHumidity.ToString(),
                plant.IdealAirHumidity.ToString());

            var email = new Email
            {
                Recipient = user.Email,
                Subject = $"PotPal: Air humidity too low for '{plant.PlantName}'",
                Body = body
            };
            await _emailService.SendAsync(email);
        }
        // Air humidity - high
        else if (metric.AirHumidity > plant.IdealAirHumidity + AirHumidityTolerance)
        {
            var body = EmailFactory.GetHumidityAlertEmail(
                user.UserName ?? user.Email,
                plant.PlantName,
                metric.AirHumidity.ToString(),
                plant.IdealAirHumidity.ToString());

            var email = new Email
            {
                Recipient = user.Email,
                Subject = $"PotPal: Air humidity too high for '{plant.PlantName}'",
                Body = body
            };
            await _emailService.SendAsync(email);
        }
    }



    public async Task DeleteAsync(string guid)
    {
        var metrics = await _metricRepo.GetByPlantGUIDAsync(guid);
        var latest = metrics?.OrderByDescending(m => m.Timestamp).FirstOrDefault();

        if (latest == null)
        {
            throw new Exception($"No metric found for GUID '{guid}'.");
        }

        await _metricRepo.DeleteAsync(latest);
    }

}


