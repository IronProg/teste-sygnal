using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TesteSygnal.Constants;
using TesteSygnal.Context;
using TesteSygnal.Models;
using TesteSygnal.Services;

namespace TesteSygnalTesting.Rest;

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
    public async Task GetAllOrderTest()
    {
        await BeginTransactionAsync();
        
        var response = await _client.GetAsync("/api/rest/order");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        List<Order>? lstOrder = await response.Content.ReadFromJsonAsync<List<Order>>() ?? null;
        
        Assert.NotNull(lstOrder);
        
        await RollbackAsync();
    }
    [Fact]
    public async Task GetOrderTest()
    {
        await BeginTransactionAsync();

        Order order = new() { State = OrderStateConstants.Pending };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        Assert.NotEqual(0, order.ControlNumber);
        
        var response = await _client.GetAsync($"/api/rest/order/{order.ControlNumber}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        Order? orderFromController = await response.Content.ReadFromJsonAsync<Order>() ?? null;
        
        Assert.NotNull(orderFromController);
        
        await RollbackAsync();
    }
    
    [Fact]
    public async Task DeleteOrderTest()
    {
        await BeginTransactionAsync();

        Order order = new() { State = OrderStateConstants.Pending };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        
        Assert.NotEqual(0, order.ControlNumber);
        
        var responseThatShouldFail = await _client.DeleteAsync($"/api/order/rest/{order.ControlNumber + 1}");
        
        Assert.Equal(HttpStatusCode.NotFound, responseThatShouldFail.StatusCode);
        
        var response = await _client.DeleteAsync($"/api/rest/order/{order.ControlNumber}");
        
        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

        Order? orderFromDb = await _context.Orders.FirstOrDefaultAsync(x => x.ControlNumber == order.ControlNumber);
        
        Assert.Null(orderFromDb);
        
        await RollbackAsync();
    }
    
    [Fact]
    public async Task UpdateOrderTest()
    {
        await BeginTransactionAsync();

        Order order = new() { State = OrderStateConstants.Pending };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        
        var response = await _client.PutAsync($"/api/rest/order/{order.ControlNumber}", null);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        Order? responseOrder = await response.Content.ReadFromJsonAsync<Order>() ?? null;
        
        Assert.NotNull(responseOrder);
        Assert.Equal(OrderStateConstants.InProgress, responseOrder.State);
        
        var secondResponse = await _client.PutAsync($"/api/rest/order/{order.ControlNumber}", null);
        
        Assert.Equal(HttpStatusCode.OK, secondResponse.StatusCode);
        
        Order? secondResponseOrder = await secondResponse.Content.ReadFromJsonAsync<Order>() ?? null;
        
        Assert.NotNull(secondResponseOrder);
        Assert.Equal(OrderStateConstants.Completed, secondResponseOrder.State);
        
        var invalidResponse = await _client.PutAsync($"/api/rest/order/{order.ControlNumber}", null);
        
        Assert.Equal(HttpStatusCode.BadRequest, invalidResponse.StatusCode);
        
        await RollbackAsync();
    }
    
    [Fact]
    public async Task CreateOrderTest()
    {
        await BeginTransactionAsync();
        
        int orderCount = _context.Orders.AsNoTracking().Count();
        
        var response = await _client.PostAsync("/api/rest/order", null);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        Order? responseOrder = await response.Content.ReadFromJsonAsync<Order>() ?? null;
        
        Assert.NotNull(responseOrder);
        Assert.Equal(OrderStateConstants.Pending, responseOrder.State);
        
        int newOrderCount = _context.Orders.Count();
        
        Assert.Equal(orderCount + 1, newOrderCount);
        
        await RollbackAsync();
    }
}

 

 

