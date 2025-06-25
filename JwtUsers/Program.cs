using Members.Entities;
using Common.Sqlite;
using Common.Jwt;
using Common.Settings;
using JwtUsers.Persistence;
using Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;  

var builder = WebApplication.CreateBuilder(args);

// Load JWT config
var jwtConfig = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetSection(nameof(SqliteSettings)).Get<SqliteSettings>()!.ConnectionString,
        x => x.MigrationsAssembly("JwtUsers")
    ));

// Register repository and JWT
builder.Services.AddScoped<IRepository<User>>(sp =>
{
    var db = sp.GetRequiredService<UsersDbContext>();
    return new SqliteRepository<User>(db);
});

builder.Services.AddJwtAuthentication(jwtConfig!);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "JwtUsers API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
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
