using Microsoft.EntityFrameworkCore;
using TesteSygnal.Context;
using TesteSygnal.Services;

const string PermitReactCorsRuleName = "permitReactCors";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: PermitReactCorsRuleName,
        policy  =>
        {
            policy.WithOrigins("https://teste-sygnal.vercel.app")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services
    .AddDbContext<TSDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<OrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors(PermitReactCorsRuleName);

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();

public partial class Program { }
