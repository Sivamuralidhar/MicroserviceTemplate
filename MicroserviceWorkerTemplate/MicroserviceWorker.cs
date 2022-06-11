using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceWorkerTemplate
{
    public class MicroserviceWorker : BackgroundService
    {
        public IHostEnvironment HostEnvironment { get; set; }
        public IConfiguration Configuration { get; set; }

        public MicroserviceWorker(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            HostEnvironment = hostEnvironment;
            Configuration = configuration;
        }

        public static string GetEmailId(string environmentName)
        {
            string emailId = string.Empty;
            switch (environmentName)
            {
                case "Development":
                    emailId = "dev@abc.com";
                    break;
                case "Staging":
                    emailId = "staging@abc.com";
                    break;
            }
            return emailId;
        }

        private static void ConfigureFileLogging()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCOREENVIRONMENT");
            string alertsEmailId = GetEmailId(environment);
            var emailInfo = new EmailConnectionInfo
            {
                EmailSubject = $"Error Occurred in 24Seven Bridge TranslateDigitalCheckStatus Background Service in {environment} Server",
                FromEmail = alertsEmailId,
                ToEmail = alertsEmailId,
                MailServer = "smtp.mailserverhere.com"
            };
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.File(@$"D:\{environment}\Microservices\ProjectName\ProjectName.txt",
            fileSizeLimitBytes: 1_000_000,
            rollingInterval: RollingInterval.Day,
            rollOnFileSizeLimit: true,
            shared: true,
            flushToDiskInterval: TimeSpan.FromSeconds(5))
            .WriteTo.Email(emailInfo, restrictedToMinimumLevel: LogEventLevel.Error)
            .CreateLogger();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Log.Information("MicroserviceWorker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task DeleteMessageFromQueue()
        {

        }
    }
}
