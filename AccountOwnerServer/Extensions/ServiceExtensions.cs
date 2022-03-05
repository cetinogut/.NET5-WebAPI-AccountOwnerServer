using Contracts;
using Entities;
using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AccountOwnerServer.Extensions
{
    public static class ServiceExtensions
    {

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options => //we need to configure CORS in our application. CORS (Cross-Origin Resource Sharing) is a mechanism that gives rights to the user to access resources from the server on a different domain. Because we will use Angular as a client-side on a different domain than the server’s domain, configuring CORS is mandatory.
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()  //WithOrigins("http://www.something.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options => //we need to configure an IIS integration which will help us with the IIS deployment
            {
                //We do not initialize any of the properties inside the options because we are just fine with the default values. For more pieces of information about those properties
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration config) //we are going to write the code for configuring the MySQL context.
        {//With the help of the IConfiguration config parameter, we can access the appsettings.json file and take all the data we need from it.
            var connectionString = config["mysqlconnection:connectionString"];
            services.AddDbContext<RepositoryContext>
                (o => o.UseMySql(connectionString, MySqlServerVersion.LatestSupportedServerVersion));
        }
    }
}
