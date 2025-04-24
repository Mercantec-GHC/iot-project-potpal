
using System.ComponentModel.DataAnnotations;
namespace Models;

public class Plant
{
    [Key]
    public string GUID { get; set; } = "";
    public string Name { get; set; } = "";
    public float IdealSoilMoisture { get; set; }
    public float IdealTemperature { get; set; }
    public float IdealLightLevel { get; set; }
    public float IdealAirHumidity { get; set; }
    public float SoilMoisture { get; set; }
    public float Temperature { get; set; }
    public float LightLevel { get; set; }
    public float AirHumidity { get; set; }
    public string UserEmail { get; set; } = "";
    
}