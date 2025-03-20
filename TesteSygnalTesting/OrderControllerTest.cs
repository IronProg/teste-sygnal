using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TesteSygnal.Constants;
using TesteSygnal.Context;
using TesteSygnal.Models;
using TesteSygnal.Services;

namespace TesteSygnalTesting;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class OrderControllerTest
{
    [Fact]
    public async Task GetOrderTest()
    {
        var application = ConfigureTestServices();
        var context = application.Services.GetRequiredService<TSDbContext>();
        await context.Database.BeginTransactionAsync();
        
        var client = application.CreateClient();
        var response = await client.GetAsync("/api/order");

        int orderCount = context.Orders.Count();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string? stringContent = await response.Content.ReadAsStringAsync();
        
        Assert.IsType<string>(stringContent);
        
        Regex regex = new("<tr data-control-number", RegexOptions.IgnoreCase);
        var trCount = regex.Matches(stringContent).Count;
        
        Assert.Equal(orderCount, trCount);
        
        await context.Database.CurrentTransaction!.RollbackAsync();
    }
    
    [Fact]
    public async Task CreateAndDeleteOrderTest()
    {
        var application = ConfigureTestServices();
        var context = application.Services.GetRequiredService<TSDbContext>();
        await context.Database.BeginTransactionAsync();
        
        var client = application.CreateClient();

        var responseCreated = await client.PostAsync("/api/order", null);

        Assert.Equal(HttpStatusCode.OK, responseCreated.StatusCode);
        
        int orderControlNumber = context.Orders.Select(x => x.ControlNumber).Max();
        
        var responseThatShouldFail = await client.DeleteAsync($"/api/order/{orderControlNumber + 1}");
        
        Assert.Equal(HttpStatusCode.NotFound, responseThatShouldFail.StatusCode);
        
        var response = await client.DeleteAsync($"/api/order/{orderControlNumber}");
        
        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

        Order? orderFromDb = await context.Orders.FirstOrDefaultAsync(x => x.ControlNumber == orderControlNumber);
        
        Assert.Null(orderFromDb);
        
        await context.Database.CurrentTransaction!.RollbackAsync();
    }
    
    [Fact]
    public async Task UpdateOrderTest()
    {
        var application = ConfigureTestServices();
        var context = application.Services.GetRequiredService<TSDbContext>();
        await context.Database.BeginTransactionAsync();
        
        var client = application.CreateClient();

        var responseCreated = await client.PostAsync("/api/order", null);

        Assert.Equal(HttpStatusCode.OK, responseCreated.StatusCode);

        Order newOrder = await context.Orders.AsNoTracking().OrderBy(x => x.ControlNumber).LastAsync();
        
        var response = await client.PutAsync($"/api/order/{newOrder.ControlNumber}", null);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        Order? orderFromDb = await context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.ControlNumber == newOrder.ControlNumber);
        
        Assert.NotEqual(orderFromDb.State, newOrder.State);
        
        await context.Database.CurrentTransaction!.RollbackAsync();
    }
    
    [Fact]
    public async Task CreateOrderTest()
    {
        var application = ConfigureTestServices();
        var context = application.Services.GetRequiredService<TSDbContext>();
        await context.Database.BeginTransactionAsync();
        
        int orderCount = context.Orders.AsNoTracking().Count();

        var client = application.CreateClient();
        
        var response = await client.PostAsync("/api/order", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        int newOrderCount = context.Orders.Count();
        
        Assert.Equal(orderCount + 1, newOrderCount);
        
        await context.Database.CurrentTransaction!.RollbackAsync();
    }

    private WebApplicationFactory<Program> ConfigureTestServices()
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

 

 

