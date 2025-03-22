using Microsoft.EntityFrameworkCore;
using TesteSygnal.Context;
using TesteSygnal.Services;

const string PermitReactCorsRuleName = "permitReactCors";

string dbConnectionString = Environment.GetEnvironmentVariable("dbConnectionString") ?? Environment.GetEnvironmentVariable("dbConnectionString", EnvironmentVariableTarget.User);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: PermitReactCorsRuleName,
        policy  =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Add services to the container.
builder.Services.AddRazorPages();

builder.Configuration.AddEnvironmentVariables();

builder.Services
    .AddDbContext<TSDbContext>(options =>
        options.UseNpgsql(dbConnectionString));

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
