using health.ViewModels;

namespace health.Pages.Dashboard
{
    public class IndexModel(
        Db db,
        UserManager<User> userManager) : AuthedPageModel(db, userManager)
    {
        [BindProperty]
        public WeightCardViewModel WeightCard { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadWeightCardAsync();
        }

        public async Task<IActionResult> OnPostAddWeightAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadWeightCardAsync();
                WeightCard.ErrorMessage = "Please fix the errors and try again.";
                return Page();
            }

            var entry = WeightCard.NewEntry;

            var measurement = new WeightMeasurement
            {
                UserId = CurrentUserId,
                Value = entry.Weight,
                MeasuredAt = entry.MeasuredAt
            };

            Db.WeightMeasurements.Add(measurement);
            await Db.SaveChangesAsync();

            return RedirectToPage();
        }

        private async Task LoadWeightCardAsync()
        {
            var weights = await Db.WeightMeasurements
                .Where(w => w.UserId == CurrentUserId)
                .OrderByDescending(w => w.MeasuredAt)
                .Take(2)
                .ToListAsync();

            if (weights.Count > 0)
            {
                WeightCard.LatestWeight = weights[0];

                if (weights.Count > 1)
                {
                    WeightCard.WeightDelta = weights[0].Value - weights[1].Value;
                }
            }
        }
    }
}
