using Infrastructure.Filters;
using Infrastructure.Services.Implementations;
using Infrastructure.Services.Interfaces;
using Catalog.Host.Configurations;
using Catalog.Host.Data;
using Catalog.Host.Repositories.Implementations;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Implementations;
using Catalog.Host.Services.Interfaces;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var configuration = GetConfiguration();

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
            .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

        builder.Services.Configure<CatalogConfig>(configuration);
        builder.Services.AddSwaggerGen();
        builder.Services.AddAutoMapper(typeof(Program));

        builder.Services.AddTransient<ICatalogItemRepository, CatalogItemRepository>();
        builder.Services.AddTransient<ICatalogService, CatalogService>();
        builder.Services.AddTransient<ICatalogItemService, CatalogItemService>();

        builder.Services.AddDbContextFactory<ApplicationDbContext>(opts => opts.UseNpgsql(configuration["ConnectionString"]));
        builder.Services.AddScoped<IDbContextWrapper<ApplicationDbContext>, DbContextWrapper<ApplicationDbContext>>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "CorsPolicy",
                builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapControllers();
        });

        await CreateDbIfNotExistsAsync(app);
        app.Run();
    }

    private static IConfiguration GetConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        return builder.Build();
    }

    private static async Task CreateDbIfNotExistsAsync(IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();

                await DbInitializer.InitializeAsync(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while creating the DB");
            }
        }
    }
}