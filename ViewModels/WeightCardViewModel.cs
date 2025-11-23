using System.ComponentModel.DataAnnotations;

namespace health.ViewModels;

public class WeightCardViewModel
{
    public WeightMeasurement? LatestWeight { get; set; }
    public decimal? WeightDelta { get; set; }

    public string Unit { get; set; } = "lb";

    public NewWeightInput NewEntry { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public class NewWeightInput
    {
        public decimal Weight { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime MeasuredAt { get; set; } = DateTime.Today;
    }
}
