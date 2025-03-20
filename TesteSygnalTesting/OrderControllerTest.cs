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
    private readonly  TSDbContext _context;
    private readonly  HttpClient _client;
    public OrderControllerTest()
    {
        var application = TestConfiguration.Get();
        _context = application.Services.GetRequiredService<TSDbContext>();
        _client = application.CreateClient();
    }
    private async Task BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
    private async Task RollbackAsync() => await _context.Database.CurrentTransaction!.RollbackAsync();
    [Fact]
    public async Task GetOrderTest()
    {
        await BeginTransactionAsync();
        
        var response = await _client.GetAsync("/api/order");

        int orderCount = _context.Orders.Count();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string? stringContent = await response.Content.ReadAsStringAsync();
        
        Assert.IsType<string>(stringContent);
        
        Regex regex = new("<tr data-control-number", RegexOptions.IgnoreCase);
        var trCount = regex.Matches(stringContent).Count;
        
        Assert.Equal(orderCount, trCount);
        
        await RollbackAsync();
    }
    
    [Fact]
    public async Task CreateAndDeleteOrderTest()
    {
        await BeginTransactionAsync();
        

        var responseCreated = await _client.PostAsync("/api/order", null);

        Assert.Equal(HttpStatusCode.OK, responseCreated.StatusCode);
        
        int orderControlNumber = _context.Orders.Select(x => x.ControlNumber).Max();
        
        var responseThatShouldFail = await _client.DeleteAsync($"/api/order/{orderControlNumber + 1}");
        
        Assert.Equal(HttpStatusCode.NotFound, responseThatShouldFail.StatusCode);
        
        var response = await _client.DeleteAsync($"/api/order/{orderControlNumber}");
        
        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

        Order? orderFromDb = await _context.Orders.FirstOrDefaultAsync(x => x.ControlNumber == orderControlNumber);
        
        Assert.Null(orderFromDb);
        
        await RollbackAsync();
    }
    
    [Fact]
    public async Task UpdateOrderTest()
    {
        await BeginTransactionAsync();
        

        var responseCreated = await _client.PostAsync("/api/order", null);

        Assert.Equal(HttpStatusCode.OK, responseCreated.StatusCode);

        Order newOrder = await _context.Orders.AsNoTracking().OrderBy(x => x.ControlNumber).LastAsync();
        
        var response = await _client.PutAsync($"/api/order/{newOrder.ControlNumber}", null);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        Order? orderFromDb = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.ControlNumber == newOrder.ControlNumber);
        
        Assert.NotEqual(orderFromDb.State, newOrder.State);
        
        await RollbackAsync();
    }
    
    [Fact]
    public async Task CreateOrderTest()
    {
        await BeginTransactionAsync();
        
        int orderCount = _context.Orders.AsNoTracking().Count();
        
        var response = await _client.PostAsync("/api/order", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        int newOrderCount = _context.Orders.Count();
        
        Assert.Equal(orderCount + 1, newOrderCount);
        
        await RollbackAsync();
    }
}

 

 

