using System.Text.Encodings.Web;
using System.Text.Json;

namespace health.Pages.Weights;

public class IndexModel(
    Db db,
    UserManager<User> userManager) : AuthedPageModel(db, userManager)
{
    public List<WeightMeasurement> Measurements { get; set; } = new();

    // For Chart.js
    public string ChartLabelsJson { get; private set; } = "[]";
    public string ChartDataJson { get; private set; } = "[]";
    public string Unit => "lb";

    public async Task OnGetAsync()
    {
        Measurements = await Db.WeightMeasurements
            .Where(w => w.UserId == CurrentUserId)
            .OrderBy(w => w.MeasuredAt)
            .ToListAsync();

        if (Measurements.Count == 0)
        {
            ChartLabelsJson = "[]";
            ChartDataJson = "[]";
            return;
        }

        var labels = Measurements
            .Select(m => m.MeasuredAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm"))
            .ToList();

        var data = Measurements
            .Select(m => m.Value)
            .ToList();

        var jsonOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        ChartLabelsJson = JsonSerializer.Serialize(labels, jsonOptions);
        ChartDataJson = JsonSerializer.Serialize(data, jsonOptions);
    }

    public async Task<IActionResult> OnPostDeleteAsync(string id)
    {
        var measurement = await Db.WeightMeasurements
            .SingleOrDefaultAsync(m => m.Id == id && m.UserId == CurrentUserId);

        if (measurement == null)
        {
            return NotFound();
        }

        Db.WeightMeasurements.Remove(measurement);
        await Db.SaveChangesAsync();

        return RedirectToPage();
    }
}
