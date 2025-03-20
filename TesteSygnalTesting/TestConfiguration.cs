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
            var options = new DbContextOptionsBuilder<TSDbContext>()
                .UseNpgsql("");

            var dbContext = new TSDbContext(options.Options);
        
            WebApplicationFactory<Program> application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => {
                builder.ConfigureTestServices(services => {

                    var options = new DbContextOptionsBuilder<TSDbContext>()
                        .UseNpgsql("Host=ep-shiny-wave-a5n7a4om-pooler.us-east-2.aws.neon.tech;Database=TesteSygnal;Username=TesteSygnal_owner;Password=npg_FLHKP2gvh6Ay;SSL Mode=Require;Trust Server Certificate=true")
                        .Options;

                    services.AddSingleton(options);

                    services.AddSingleton<TSDbContext>();

                    services.AddSingleton<OrderService>();

                });
            });

            return application;
    }
}