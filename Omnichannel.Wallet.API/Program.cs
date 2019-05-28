using System;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.Json;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Omnichannel.Wallet.API
{
    public class Program
    {
        public static IHostingEnvironment HostingEnvironment
        { get; set; }

        public static IConfiguration Configuration
        { get; set; }

        public static void Main(string[] args)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_LAMBDA_FUNCTION_NAME")))
                CreateWebHostBuilder(args).Build().Run();
            else
            {
                var lambdaEntry = new LambdaEntryPoint();
                var functionHandler = (Func<APIGatewayProxyRequest, ILambdaContext, Task<APIGatewayProxyResponse>>)lambdaEntry.FunctionHandlerAsync;

                using (var handlerWrapper = HandlerWrapper.GetHandlerWrapper(functionHandler, new JsonSerializer()))
                using (var bootstrap = new LambdaBootstrap(handlerWrapper))
                {
                    bootstrap.RunAsync().Wait();
                }
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    HostingEnvironment = hostingContext.HostingEnvironment;
                    config.SetBasePath(HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{HostingEnvironment.EnvironmentName}.json", true);
                    config.AddEnvironmentVariables();
                    Configuration = config.Build();
                })
                .UseStartup<Startup>();
    }
}
