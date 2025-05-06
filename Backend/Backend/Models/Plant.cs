
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Models;

public class Plant
{
    [Key]
    public string GUID { get; set; } = "";
    public string PlantName { get; set; } = "";
    public float IdealSoilMoisture { get; set; }
    public float IdealTemperature { get; set; }
    public float IdealLightLevel { get; set; }
    public float IdealAirHumidity { get; set; }
    public string UserEmail { get; set; } = "";
    public List<Metric> Metrics { get; set; } = new();

    [ForeignKey("UserEmail")]
    public User User { get; set; } = null!;
}