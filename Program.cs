using Microsoft.EntityFrameworkCore;
using Shellty_Blog.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = GetConnectionString(builder.Configuration);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BlogContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BlogContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

static string GetConnectionString(IConfiguration configuration)
{
    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

    if (!string.IsNullOrEmpty(databaseUrl))
    {
        var url = databaseUrl.Replace("postgres://", "postgresql://");
        var uri = new Uri(url);
        var userInfo = uri.UserInfo.Split(':');
        var port = uri.Port > 0 ? uri.Port : 5432;
        return $"Host={uri.Host};Port={port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
    }

    return configuration.GetConnectionString("BlogConnection")
           ?? throw new InvalidOperationException("Connection string 'BlogConnection' not found.");
}