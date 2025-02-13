using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.EventBroker;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Ambev.DeveloperEvaluation.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Log.Information("Starting web application");

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.AddDefaultLogging();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(environment == "Development" ? $"appsettings.{environment}.json" : "appsettings.json")
                .AddEnvironmentVariables();

            builder.AddBasicHealthChecks();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DefaultContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
                )
            );

            // Inicializar o banco de dados
            builder.Services.InitializeDatabase();

            builder.Services.AddJwtAuthentication(builder.Configuration);

            // IUserService
            builder.Services.AddTransient<IUserService, UserService>();

            // ISaleRepository
            builder.Services.AddTransient<ISaleRepository, SaleRepository>();

            // ISaleService
            builder.Services.AddTransient<ISaleService, SaleService>();

            // ISaleManagerService
            builder.Services.AddTransient<ISaleManagerService, SaleManagerService>();

            // IProductRepository
            builder.Services.AddTransient<IProductRepository, ProductRepository>();

            // ICustomerRepository
            builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();

            builder.Services.AddTransient<IUserService, UserService>();



            builder.Services.AddTransient<IBranchRepository, BranchRepository>();
            builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
            builder.Services.AddTransient<IProductRepository, ProductRepository>();
            builder.Services.AddTransient<ISaleRepository, SaleRepository>();
            builder.Services.AddTransient<IStockRepository, StockRepository>();

            builder.Services.AddTransient<IEventBroker, EventBroker>();
            builder.Services.AddTransient<IStockService, StockService>();

            builder.RegisterDependencies();

            builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(Program).Assembly
                );
            });

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            var app = builder.Build();
            app.UseMiddleware<ValidationExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseBasicHealthChecks();

            var scope = app.Services.CreateScope();

            scope.ServiceProvider.GetService<DefaultContext>()!.Database.Migrate();

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}