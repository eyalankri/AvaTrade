using Common.Jwt;
using Common.Settings;
using Common.Sqlite;
using Common.Repositories;
using NewsApi.Services;
using NewsApi.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models; //Required for Swagger JWT
using News.Contracts.Entities;

var builder = WebApplication.CreateBuilder(args);

// Load JWT settings
var jwtConfig = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

// Configure SQLite using local NewsDbContext and migration assembly
builder.Services.AddDbContext<NewsDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetSection(nameof(SqliteSettings)).Get<SqliteSettings>()!.ConnectionString,
        x => x.MigrationsAssembly("NewsApi")
    ));

builder.Services.AddScoped<IRepository<NewsItem>>(sp =>
{
    var db = sp.GetRequiredService<NewsDbContext>();
    return new SqliteRepository<NewsItem>(db);
});

builder.Services.AddScoped<INewsService, NewsService>();

builder.Services.AddJwtAuthentication(jwtConfig!);

 
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "News API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Enter your JWT token in the format **Bearer &lt;token&gt;**"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new() { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new List<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAnyOrigin");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
