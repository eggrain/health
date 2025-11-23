namespace health.Data;

public class Db(DbContextOptions<Db> options) : IdentityDbContext<Models.User>(options)
{
    public DbSet<WeightMeasurement> WeightMeasurements { get; set; } = default!;
}