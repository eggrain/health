namespace health.Models;

public class WeightMeasurement : Entity
{
    public DateTime MeasuredAt { get; set; }

    public decimal Value { get; set; }
}
