using TesteSygnal.Context;


using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using Xunit.Sdk;

 
[assembly: Xunit.TestFramework("AspNetCoreTesting.Api.Tests.TestRunStart", "AspNetCoreTesting.Api.Tests")]
namespace TesteSygnalTesting;

public class TestRunStart : XunitTestFramework
{
    public TestRunStart(IMessageSink messageSink) : base(messageSink)
    {
            string dbConnectionString = Environment.GetEnvironmentVariable("dbConnectionString") ?? Environment.GetEnvironmentVariable("dbConnectionString", EnvironmentVariableTarget.User);
        var options = new DbContextOptionsBuilder<TSDbContext>()
            .UseNpgsql(dbConnectionString);
        
        var dbContext = new TSDbContext(options.Options);
    }
}