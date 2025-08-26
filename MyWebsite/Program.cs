using Microsoft.EntityFrameworkCore;
using MyWebsite.Repository.Concrate;
using MyWebsite.Repository.config;
using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.Concrate;
using MyWebsite.Service.İnterfaces;
using MyWebsite.Service.İnterfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Controller'ları servislere ekler
builder.Services.AddControllers();

// Swagger/OpenAPI servislerini ekler
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext'i servislere ekler ve veritabanı bağlantısını yapılandırır
builder.Services.AddDbContext<MyWebSiteData>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository ve Servis katmanlarını DI konteynerine kaydeder
// Scope: İstek başına tek bir örnek oluşturulur
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IUserService, UserManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Geliştirme ortamındaysa Swagger UI'ı etkinleştirir
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();