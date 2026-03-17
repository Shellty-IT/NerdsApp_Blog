using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shellty_Blog.Data;
using Shellty_Blog.Models;
using Shellty_Blog.Services;

namespace Shellty_Blog.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlogDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
                               ?? configuration.GetConnectionString("BlogConnection");

        connectionString = ConvertPostgresUri(connectionString);

        services.AddDbContext<BlogContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }

    public static IServiceCollection AddBlogIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<BlogContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection AddBlogServices(this IServiceCollection services)
    {
        services.AddScoped<IBlogPostService, BlogPostService>();

        return services;
    }

    private static string? ConvertPostgresUri(string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString) || !connectionString.StartsWith("postgresql://"))
            return connectionString;

        var uri = new Uri(connectionString);
        var userInfo = uri.UserInfo.Split(':');

        return $"Host={uri.Host};Port={(uri.Port > 0 ? uri.Port : 5432)};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={(userInfo.Length > 1 ? userInfo[1] : "")};SSL Mode=Require;Trust Server Certificate=true";
    }
}