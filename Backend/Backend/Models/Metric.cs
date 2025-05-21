namespace Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Metric
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string PlantGUID { get; set; } = "";
    [ForeignKey("PlantGUID")]
    public Plant Plant { get; set; } = null!;
    public float SoilMoisture { get; set; }
    public float Temperature { get; set; }
    public float LightLevel { get; set; }
    public float AirHumidity { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
}