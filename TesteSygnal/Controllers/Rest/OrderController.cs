using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteSygnal.Constants;
using TesteSygnal.Context;
using TesteSygnal.DTOs;
using TesteSygnal.Models;
using TesteSygnal.Services;
using TesteSygnal.ViewModels;

namespace TesteSygnal.Controllers.Rest;

[Route("api/Rest/[controller]")]
public class OrderController(OrderService orderService) : Controller
{
    [HttpGet("{controlNumber:int}")]
    public ActionResult<Order?> GetOrderById([FromRoute] int controlNumber)
    {
        try
        {
            Order? lstOrders = orderService.GetOrder(controlNumber);

            if (lstOrders == null)
                return NotFound();
            
            return Ok(lstOrders);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    [HttpGet]
    public ActionResult<List<Order>> GetOrders([FromQuery(Name = "FormDTO")] OrderFormDTO? orderFormDTO)
    {
        try
        {
            List<Order> lstOrders = orderService.GetOrders(orderFormDTO)
                .ToList();

            return Ok(lstOrders);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    public IActionResult PostOrder()
    {
        try
        {
            Order? newOrder = orderService.NewOrder();
            
            if (newOrder == null)
                return BadRequest("Order not created.");

            return CreatedAtAction(nameof(GetOrderById), new { controlNumber = newOrder.ControlNumber }, newOrder);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);  
        }
    }

    [HttpPut("{controlNumber:int}")]
    public IActionResult UpdateOrder([FromRoute] int controlNumber)
    {
        try
        {
            Order? order = orderService.GetOrder(controlNumber);
            
            if (order == null)
                return NotFound("Order not found.");
            
            if (order.State == OrderStateConstants.Completed)
                return NotFound("Order is already completed.");
            
            Order? dbOrder = orderService.UpdateOrderState(controlNumber);

            if (dbOrder == null)
                return BadRequest("Order not found or not updated.");
            
            return Ok(dbOrder);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); 
        }
    }

    [HttpDelete("{controlNumber:int}")]
    public ActionResult DeleteOrder([FromRoute] int controlNumber)
    {
        try
        {
            Order? dbOrder = orderService.GetOrder(controlNumber);
            if (dbOrder == null)
                return NotFound("Order not found.");
            
            if (dbOrder.State != OrderStateConstants.Pending)
                return NotFound("Order is not pending! Deletion was not possible.");

            bool wasDeleted = orderService.DeleteOrder(controlNumber);

            if (!wasDeleted)
                return BadRequest("Order couldn't be not deleted.");
            
            return Accepted();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);   
        }
    }
}