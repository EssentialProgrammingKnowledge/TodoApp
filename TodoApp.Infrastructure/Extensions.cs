using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoApp.Infrastructure.Database;
using TodoApp.Infrastructure.Exceptions;

namespace TodoApp.Infrastructure
{
    public static class Extensions
    {
        #region WebApi

        private const string CORS_POLICY = "TodoAppCorsPolicy";

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppOptions>(configuration.GetSection("app"));
            services.AddDatabase(configuration);
            services.AddErrorHandling(); 
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors(cors => cors.AddPolicy(CORS_POLICY, policy =>
            {
                policy.WithOrigins(configuration.GetValue<string>("frontend") ?? throw new InvalidOperationException("frontend url should be provided"));
                policy.WithMethods("POST", "PUT", "PATCH", "DELETE");
                policy.WithHeaders("content-type");
            }));
            return services;
        }

        public static WebApplication UseInfrastructure(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(CORS_POLICY);
            app.UseAuthorization();
            app.UseErrorHandling();
            app.MapControllers();
            return app;
        }

        #endregion

        #region ConsoleApp

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, Dictionary<string, object> appsettings)
        {
            services.AddDatabase(appsettings);
            return services;
        }

        public static IServiceProvider UseInfrastructure(this IServiceProvider serviceProvider)
        {
            var dbInitializer = serviceProvider.GetRequiredService<IDbInitializer>();
            dbInitializer.Start().GetAwaiter().GetResult();
            return serviceProvider;
        }

        #endregion
    }
}