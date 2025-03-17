using TesteSygnal.Constants;
using TesteSygnal.Models;

namespace TesteSygnal.ViewModels;

public class OrderViewModel(Order order)
{
    public int ControlNumber { get; set; } = order.ControlNumber;
    public string State { get; set; } = order.State;

    public string StateName =>
        State == OrderStateConstants.Pending
            ? "Pending"
            : State == OrderStateConstants.InProgress
                ? "In Progress"
                : "Completed";

    public string StateColor =>
        State == OrderStateConstants.Pending
            ? "secondary"
            : State == OrderStateConstants.InProgress
                ? "primary"
                : "success";
}