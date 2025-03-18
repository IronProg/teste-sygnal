using Microsoft.AspNetCore.Mvc.Rendering;
using TesteSygnal.Constants;

namespace TesteSygnal.DTOs;

public class OrderFormDTO
{
    public int? ControlNumber { get; set; }
    public int? State { get; set; }

    public List<SelectListItem> StateList =>
    [
        new ("All", ""),
        new ("Pending", OrderStateConstants.Pending),
        new ("In Progress", OrderStateConstants.InProgress),
        new ("Completed", OrderStateConstants.Completed),
    ];
}

