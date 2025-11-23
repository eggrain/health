var builder = WebApplication.CreateBuilder(args);

string connectionString = null!;

if (builder.Environment.IsDevelopment())
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Is SQLite connection string in appsettings.json missing?");
}
else if (builder.Environment.IsProduction())
{
    connectionString = builder.Configuration["health_DATABASE_PATH"]
        ?? throw new InvalidOperationException(
            "Is health_DATABASE_CONNECTION environment variable present?");
}

builder.Services.AddDbContext<Db>(options =>
{
    options.UseSqlite(connectionString);
});

builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})              .AddEntityFrameworkStores<Db>();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

using IServiceScope scope = app.Services.CreateScope();
Db db = scope.ServiceProvider.GetRequiredService<Db>();
db.Database.Migrate();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
