using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TesteSygnal.Context;
using TesteSygnal.Services;

namespace TesteSygnalTesting;

public class TestConfiguration
{
    public static WebApplicationFactory<Program> Get()
    {
            string dbConnectionString = Environment.GetEnvironmentVariable("dbConnectionString") ?? Environment.GetEnvironmentVariable("dbConnectionString", EnvironmentVariableTarget.User);

            var options = new DbContextOptionsBuilder<TSDbContext>()
                .UseNpgsql("");

            var dbContext = new TSDbContext(options.Options);
        
            WebApplicationFactory<Program> application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => {
                builder.ConfigureTestServices(services => {

                    var options = new DbContextOptionsBuilder<TSDbContext>()
                        .UseNpgsql(dbConnectionString)
                        .Options;

                    services.AddSingleton(options);

                    services.AddSingleton<TSDbContext>();

                    services.AddSingleton<OrderService>();

                });
            });

            return application;
    }
}