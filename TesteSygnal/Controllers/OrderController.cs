using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteSygnal.Constants;
using TesteSygnal.Context;
using TesteSygnal.DTOs;
using TesteSygnal.Models;
using TesteSygnal.Services;
using TesteSygnal.ViewModels;

namespace TesteSygnal.Controllers;

[Route("api/[controller]")]
public class OrderController(OrderService orderService) : Controller
{
    [HttpGet]
    public IActionResult GetOrders([FromQuery(Name = "FormDTO")] OrderFormDTO? orderFormDTO)
    {
        try
        {
            List<OrderViewModel> lstOrders = orderService.GetOrders(orderFormDTO)
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
    public IActionResult PostOrder()
    {
        try
        {
            Order? newOrder = orderService.NewOrder();
            
            if (newOrder == null)
                return BadRequest("Order not created");
            
            return PartialView("Partials/_table_order_row", new OrderViewModel(newOrder));
        }
        catch (Exception ex)
        {
            return BadRequest("An error occurred while creating order: " + ex.Message);   
        }
    }

    [HttpPut("{controlNumber:int}")]
    public IActionResult UpdateOrder([FromRoute] int controlNumber)
    {
        try
        {
            if (orderService.GetOrder(controlNumber) == null)
                return NotFound("Order not found");
            
            Order? dbOrder = orderService.UpdateOrderState(controlNumber);

            if (dbOrder == null)
                return BadRequest("Order not found or not updated");
            
            return Ok(new OrderViewModel(dbOrder));
        }
        catch (Exception ex)
        {
            return BadRequest("An error occurred while updating order: " + ex.Message);   
        }
    }

    [HttpDelete("{controlNumber:int}")]
    public IActionResult DeleteOrder([FromRoute] int controlNumber)
    {
        try
        {
            Order? dbOrder = orderService.GetOrder(controlNumber);
            if (dbOrder == null)
                return NotFound("Order not found");

            bool wasDeleted = orderService.DeleteOrder(controlNumber);

            if (!wasDeleted)
                return BadRequest("Order not found or not deleted");
            
            return Accepted();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);   
        }
    }
}