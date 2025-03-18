using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteSygnal.Constants;
using TesteSygnal.Context;
using TesteSygnal.DTOs;
using TesteSygnal.Models;
using TesteSygnal.ViewModels;

namespace TesteSygnal.Controllers;

[Route("api/[controller]")]
public class OrderController(TSDbContext context) : Controller
{
    private readonly TSDbContext _context = context;

    [HttpGet]
    public IActionResult GetOrders([FromQuery] OrderFormDTO? order)
    {
        try
        {
            List<OrderViewModel> lstOrders = _context.Orders.AsNoTracking()
                .Select(order => new OrderViewModel(order))
                .ToList();
            return PartialView("Partials/_table_order", lstOrders);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public IActionResult PostOrder([FromBody] int controlNumber)
    {
        try
        {
            Order newOrder = new() { ControlNumber = controlNumber, State = OrderStateConstants.Pending };
            _context.Orders.Add(newOrder);
            _context.SaveChanges();
            
            return PartialView("Partials/_table_order_row", new OrderViewModel(newOrder));
        }
        catch (Exception ex)
        {
            return BadRequest("An error occurred while creating order: " + ex.Message);   
        }
    }

    [HttpPut("controlNumber:int")]
    public IActionResult UpdateOrder([FromRoute] int controlNumber)
    {
        try
        {
            Order dbOrder = _context.Orders.AsNoTracking()
                .FirstOrDefault(order => order.ControlNumber == controlNumber);

            _context.Orders.Attach(dbOrder);

            if (dbOrder.State == OrderStateConstants.Pending)
                dbOrder.State = OrderStateConstants.InProgress;
            else if (dbOrder.State == OrderStateConstants.InProgress)
                dbOrder.State = OrderStateConstants.Completed;

            _context.SaveChanges();
            
            return Ok(new OrderViewModel(dbOrder));
        }
        catch (Exception ex)
        {
            return BadRequest("An error occurred while updating order: " + ex.Message);   
        }
    }

    [HttpDelete("controlNumber:int")]
    public IActionResult DeleteOrder([FromRoute] int controlNumber)
    {
        try
        {
            Order dbOrder = _context.Orders.AsNoTracking()
                .FirstOrDefault(order => order.ControlNumber == controlNumber);

            _context.Orders.Remove(dbOrder);

            _context.SaveChanges();
            
            return Accepted();
        }
        catch (Exception ex)
        {
            return BadRequest("An error occurred while updating order: " + ex.Message);   
        }
    }
}