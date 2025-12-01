using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // Swagger için eklendi
using MyWebsite.Dtos.CategoryDtos;
using MyWebsite.Dtos.Comment;
using MyWebsite.Dtos.Experince;
using MyWebsite.Dtos.MessageDtos;
using MyWebsite.Dtos.ProjectDtos;
using MyWebsite.Dtos.UserDtos;
using MyWebsite.Exceptions;
using MyWebsite.Middleware;
using MyWebsite.Repository.Concrate;
using MyWebsite.Repository.config;
using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.Concrate;
using MyWebsite.Service.İnterfaces;
using MyWebsite.Validator.Categories;
using MyWebsite.Validator.Comment;
using MyWebsite.Validator.Experience;
using MyWebsite.Validator.Message;
using MyWebsite.Validator.Project;
using MyWebsite.Validator.User;
using System.Text;
using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);
// CORS servisini ekle
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            // Frontend adresinizi buraya ekleyin
            builder.WithOrigins("http://localhost:5173")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddControllers();

// Swagger/OpenAPI Servisleri Eklendi (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyWebsite API", Version = "v1" });

    // JWT desteğini Swagger UI'a ekle
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});


// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddProblemDetails();

// DbContext
builder.Services.AddDbContext<MyWebSiteData>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository & Service DI
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectManager>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IArticleService, ArticleManager>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IMessageService, MessageManager>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentManager>();
builder.Services.AddScoped<ISkillsRepository, SkillRepository>();
builder.Services.AddScoped<ISkilService, SkillManager>();
builder.Services.AddScoped<IExperinceRepository, ExperinceRepository>();
builder.Services.AddScoped<IExperinceServices, ExperinceManager>();
builder.Services.AddScoped<IArticleLikeRepository, ArticleLikeRepository>();
builder.Services.AddScoped<IArticleLikeService, ArticleLikeManager>();
builder.Services.AddScoped<IMediaRepository, MediaRepository>();
builder.Services.AddScoped<IMediService, MediaManager>();


// validators
//builder.Services.AddScoped<IValidator<CreateCategoryValidator>>();
builder.Services.AddScoped<IValidator<CreateCategoryDtos>, CreateCategoryValidator>();
builder.Services.AddScoped<IValidator<MessageDtos>, MessageDtosValidator>();
builder.Services.AddScoped<IValidator<CreateProjectDtos>, CreateProjectDtoValidator>();

builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserValidator>();

builder.Services.AddScoped<IValidator<UpdateUserDto>, UpdateUserValidator>();
builder.Services.AddScoped<IValidator<createCommentDto>, CreateCommentValidator>();

builder.Services.AddScoped<IValidator<CreateExperinceDtos>, CreateExperinceValidator>();
builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<IValidator<MessageDtos>, MessageDtosValidator>();

var app = builder.Build();
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (exceptionHandlerFeature?.Error != null)
        {
            await CustomExceptionHandler.HandleException(context, exceptionHandlerFeature.Error);
        }
    });
});


if (app.Environment.IsDevelopment())
{
    // Swagger belgesini kullan
    app.UseSwagger();
    // Swagger UI arayüzünü kullan
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyWebsite API V1");
    });
}

app.UseHttpsRedirection();
// CORS middleware'ini kullan
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
// ----------------------------
// Static Files Middleware
// ----------------------------
app.UseStaticFiles(); // wwwroot için
// wwwroot altındaki uploads klasörünü static olarak sun
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")
    ),
    RequestPath = "/uploads"
});

app.MapControllers();
app.Run();