using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using TesteSygnal.Constants;

namespace TesteSygnal.Models;

public class Order
{
    [Key]
    public int ControlNumber { get; set; }
    [Required]
    [AllowedValues([
        OrderStateConstants.Pending, 
        OrderStateConstants.InProgress, 
        OrderStateConstants.Completed])]
    public string State { get; set; } = OrderStateConstants.Pending;
}