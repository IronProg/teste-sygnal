using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TesteSygnal.Constants;
using TesteSygnal.Context;
using TesteSygnal.DTOs;
using TesteSygnal.Models;
using TesteSygnal.Services;

namespace TesteSygnalTesting;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class OrderServiceTest
{
    private readonly  TSDbContext _context;
    private readonly OrderService _orderService;
    public OrderServiceTest()
    {
        var application = TestConfiguration.Get();
        _context = application.Services.GetRequiredService<TSDbContext>();
         _orderService = application.Services.GetRequiredService<OrderService>();
    }
    private async Task BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
    private async Task RollbackAsync() => await _context.Database.CurrentTransaction!.RollbackAsync();
    [Fact]
    public async Task GetOrderTest()
    {
        await BeginTransactionAsync();
        
        int orderCount = _context.Orders.Count();

        OrderFormDTO formDto = new () { State = null };
        List<Order> serviceOrders = _orderService.GetOrders(formDto);
        Assert.Equal(serviceOrders.Count(), orderCount);
        
        await RollbackAsync();
    }
    
    [Fact]
    public async Task CreateAndDeleteOrderTest()
    {
        await BeginTransactionAsync();

        Order? newOrder = _orderService.NewOrder();
        Assert.NotNull(newOrder);
        
        bool result = _orderService.DeleteOrder(newOrder.ControlNumber);
        Assert.True(result);
        
        await RollbackAsync();
    }
    
    [Fact]
    public async Task UpdateOrderTest()
    {
        await BeginTransactionAsync();

        Order? newOrder = _orderService.NewOrder();
        Assert.NotNull(newOrder);
        
        string expectedState = OrderStateConstants.InProgress;

        Order? updatedOrder = _orderService.UpdateOrderState(newOrder.ControlNumber);
        Assert.NotNull(updatedOrder);
        Assert.Equal(expectedState, updatedOrder.State);
        
        await RollbackAsync();
    }
    
    [Fact]
    public async Task CreateOrderTest()
    {
        await BeginTransactionAsync();

        Order? newOrder = _orderService.NewOrder();
        Assert.NotNull(newOrder);
        
        await RollbackAsync();
    }
}

 

 

