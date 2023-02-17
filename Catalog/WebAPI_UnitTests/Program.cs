using Infrastructure.Services.Implementations;
using WebAPI_UnitTests.Configurations;
using WebAPI_UnitTests.Data;
using WebAPI_UnitTests.Repositories.Implementations;
using WebAPI_UnitTests.Repositories.Interfaces;
using WebAPI_UnitTests.Services.Implementations;
using WebAPI_UnitTests.Services.Interfaces;
using Infrastructure.Services.Interfaces;

public class Program
{
    public static async Task Main(string[] args)
    {
        var configuration = GetConfiguration();

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.Configure<CatalogConfig>(configuration);
        builder.Services.AddSwaggerGen();
        builder.Services.AddAutoMapper(typeof(Program));

        builder.Services.AddTransient<ICatalogItemRepository, CatalogItemRepository>();
        builder.Services.AddTransient<ICatalogBrandRepository, CatalogBrandRepository>();
        builder.Services.AddTransient<ICatalogTypeRepository, CatalogTypeRepository>();

        builder.Services.AddTransient<ICatalogService, CatalogService>();
        builder.Services.AddTransient<ICatalogItemService, CatalogItemService>();
        builder.Services.AddTransient<ICatalogBrandService, CatalogBrandService>();
        builder.Services.AddTransient<ICatalogTypeService, CatalogTypeService>();

        builder.Services.AddDbContextFactory<ApplicationDbContext>(opts => opts.UseNpgsql(configuration["ConnectionString"]));
        builder.Services.AddScoped<IDbContextWrapper<ApplicationDbContext>, DbContextWrapper<ApplicationDbContext>>();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
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