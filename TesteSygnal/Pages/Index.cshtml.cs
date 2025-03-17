using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TesteSygnal.Context;
using TesteSygnal.DTOs;
using TesteSygnal.Models;

namespace TesteSygnal.Pages;

public class IndexModel(
    TSDbContext context, 
    ILogger<IndexModel> logger) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public OrderFormDTO FormDTO { get; set; } = new();
    [BindProperty]
    public int? NewOderControlNumber { get; set; }

    public void OnGet()
    {
    }
}
