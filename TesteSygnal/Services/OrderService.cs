using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteSygnal.Constants;
using TesteSygnal.Context;
using TesteSygnal.DTOs;
using TesteSygnal.Models;
using TesteSygnal.ViewModels;

namespace TesteSygnal.Services;


public class OrderService(TSDbContext context) 
{
    private readonly TSDbContext _context = context;

    public Order? GetOrder(int controlNumber)
    {
        return _context.Orders.AsNoTracking().FirstOrDefault(order => order.ControlNumber == controlNumber);
    }
    
    public List<Order> GetOrders(OrderFormDTO? orderFormDTO)
    {
        try
        {
            IQueryable<Order> queryOrder = _context.Orders.AsNoTracking();
            
            if (orderFormDTO.State != null)
                queryOrder = queryOrder.Where(o => o.State == orderFormDTO.State);  
            if (orderFormDTO.ControlNumber != null)
                queryOrder = queryOrder.Where(o => o.ControlNumber >= orderFormDTO.ControlNumber);
            if (orderFormDTO.ControlNumberMax != null)
                queryOrder = queryOrder.Where(o => o.ControlNumber <= orderFormDTO.ControlNumberMax);

            return queryOrder.ToList();
        }
        catch (Exception e)
        {
            return [];
        }
    }
    
    public Order? NewOrder()
    {
        try
        {
            Order newOrder = new() { State = OrderStateConstants.Pending };
            _context.Orders.Add(newOrder);
            
            if (_context.SaveChanges() > 0)
                return newOrder;
            
            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public Order? UpdateOrderState(int controlNumber)
    {
        try
        {
            Order? dbOrder = _context.Orders.AsNoTracking()
                .FirstOrDefault(order => order.ControlNumber == controlNumber);

            if (dbOrder == null)
                return null;
            
            _context.Orders.Attach(dbOrder);

            if (dbOrder.State == OrderStateConstants.Pending)
                dbOrder.State = OrderStateConstants.InProgress;
            else if (dbOrder.State == OrderStateConstants.InProgress)
                dbOrder.State = OrderStateConstants.Completed;

            if (_context.SaveChanges() > 0)
                return dbOrder;

            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public bool DeleteOrder(int controlNumber)
    {
        try
        {
            Order? dbOrder = _context.Orders.AsNoTracking()
                .FirstOrDefault(order => order.ControlNumber == controlNumber);

            if (dbOrder == null)
                return false;
            
            _context.Orders.Remove(dbOrder);

            if (_context.SaveChanges() > 0)
                return true;
            
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}