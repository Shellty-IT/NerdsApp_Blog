using Shellty_Blog.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddBlogDatabase(builder.Configuration);
builder.Services.AddBlogIdentity();
builder.Services.AddBlogServices();

var app = builder.Build();

app.MigrateDatabase();
app.ConfigurePipeline();

app.Run();