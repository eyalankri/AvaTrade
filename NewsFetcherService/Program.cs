using News.Contracts.Entities;
 
using Common.Repositories;
using Common.Sqlite;
using Common.Settings;
using Microsoft.EntityFrameworkCore;
using NewsFetcherService.Persistence;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true)
              .AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        services.AddHttpClient();

        // ✅ Register DB context manually (like you do in NewsApi)
        services.AddDbContext<NewsDbContext>(options =>
            options.UseSqlite(
                configuration.GetSection(nameof(SqliteSettings)).Get<SqliteSettings>()!.ConnectionString,
                x => x.MigrationsAssembly("NewsFetcherService")
            ));

        services.AddScoped<IRepository<NewsItem>>(sp =>
        {
            var db = sp.GetRequiredService<NewsDbContext>();
            return new SqliteRepository<NewsItem>(db);
        });


        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
