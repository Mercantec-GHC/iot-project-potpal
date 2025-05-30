public class PlantPostDTO
{   
    public string Guid { get; set; } = "";
    public string PlantName { get; set; } = "";
    public float IdealSoilMoisture { get; set; }
    public float IdealTemperature { get; set; }
    public float IdealLightLevel { get; set; }
    public float IdealAirHumidity { get; set; }
    public string UserEmail { get; set; } = "";
}