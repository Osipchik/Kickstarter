using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace MailService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            // return Host.CreateDefaultBuilder(args)
            //     .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
            
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((context, configuration) =>
                {
                    configuration.ReadFrom.Configuration(context.Configuration);
                });
        }


       
        // public static IHostBuilder CreateHostBuilder(string[] args)
        // {
        //     var hostBuilder = Host.CreateDefaultBuilder(args)
        //         .ConfigureHostConfiguration(configHost =>
        //         {
        //             configHost.SetBasePath(Directory.GetCurrentDirectory());
        //             configHost.AddJsonFile($"appsettings.json", optional: true);
        //             configHost.AddEnvironmentVariables();
        //             configHost.AddCommandLine(args);
        //         })
        //         .ConfigureAppConfiguration((hostContext, config) =>
        //         {
        //             config.AddJsonFile($"appsettings.json", optional: true);
        //         })
        //         .ConfigureServices((hostContext, services) =>
        //         {
        //             //services.AddRazorPages();
        //     
        //             services.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
        //     
        //             services.AddMassTransit(x =>
        //             {
        //                 x.AddConsumer<EmailConsumer>();
        //                 
        //                 x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
        //                 {
        //                     cfg.Host("rabbitmq://localhost");
        //             
        //                     cfg.ReceiveEndpoint("MailService", ep =>
        //                     {
        //                         ep.PrefetchCount = 16;
        //                         ep.UseMessageRetry(r => r.Interval(2, 100));
        //                         ep.ConfigureConsumer<EmailConsumer>(provider);
        //                         ep.Bind("MailService");
        //                     });
        //                 }));
        //             });
        //     
        //             services.AddMassTransitHostedService();
        //         })
        //         .UseSerilog((hostContext, loggerConfiguration) =>
        //         {
        //             loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
        //         })
        //         .UseConsoleLifetime();
        //     
        //     return hostBuilder; 
        // }
    }
}