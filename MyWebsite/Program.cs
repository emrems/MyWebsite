using Microsoft.EntityFrameworkCore;
using MyWebsite.Repository.Concrate;
using MyWebsite.Repository.config;
using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.Concrate;
using MyWebsite.Service.İnterfaces;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// OpenAPI metadata (endpointler Scalar’ın göreceği JSON’u buradan alacak)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// DbContext
builder.Services.AddDbContext<MyWebSiteData>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository & Service DI
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService,CategoryManager>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService,ProjectManager>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IArticleService, ArticleManager>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // OpenAPI dokümanı üret
    app.MapOpenApi();
}

// Scalar UI
app.MapScalarApiReference(
   opt =>
   {
       opt.Title = "KafeApi API Reference";
       opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
       opt.Theme = ScalarTheme.BluePlanet;
   }
);
app.UseMiddleware<MyWebsite.Middleware.ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
