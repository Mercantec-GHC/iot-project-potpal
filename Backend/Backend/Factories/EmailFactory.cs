public static class EmailFactory
{


    public static string GetWelcomeEmail(string UserName)
    {
        var template = LoadEmailTemplate("Welcome.html");
        return template.Replace("{UserName}", UserName);
    }

    public static string GetLowMoistureEmail(string UserName, string PlantName, string SoilMoisture, string IdealSoilMoisture)
    {
        var template = LoadEmailTemplate("LowMoistureEmail.html");
        return template.Replace("{UserName}", UserName).Replace("{PlantName}", PlantName).Replace("{SoilMoisture}", SoilMoisture).Replace("{IdealSoilMoisture}", IdealSoilMoisture);
    }

    public static string GetLightEmailAlert(string UserName, string PlantName, string LightLevel, string IdealLightLevel)
    {
        var template = LoadEmailTemplate("LightAlert.html");
        return template.Replace("{UserName}", UserName).Replace("{PlantName}", PlantName).Replace("{LightLevel}", LightLevel).Replace("{IdealLightLevel}", IdealLightLevel);
    }
    public static string GetHumidityAlertEmail(string UserName, string PlantName, string AirHumidity, string IdealAirHumidity)
    {
        var template = LoadEmailTemplate("HumidityAlert.html");
        return template.Replace("{UserName}", UserName).Replace("{plant}", PlantName).Replace("{AirHumidity}", AirHumidity).Replace("{IdealAirHumidity}", IdealAirHumidity);
    }
    private static string LoadEmailTemplate(string fileName)
    {
        var path = Path.Combine(Config.EMAIL_TEMPLATES_PATH, fileName);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Email template {fileName} not found at {path}");
        }
        // Read the file content
        return File.ReadAllText(path);
    }

}