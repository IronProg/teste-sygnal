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
        var options = new DbContextOptionsBuilder<TSDbContext>()
            .UseNpgsql("Host=ep-shiny-wave-a5n7a4om-pooler.us-east-2.aws.neon.tech;Database=TesteSygnal;Username=TesteSygnal_owner;Password=npg_FLHKP2gvh6Ay;SSL Mode=Require;Trust Server Certificate=true");
        
        var dbContext = new TSDbContext(options.Options);
    }
}