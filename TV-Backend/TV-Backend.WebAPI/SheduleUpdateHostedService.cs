using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TV_Backend.WebAPI
{
    public class SheduleUpdateHostedService : BackgroundService
    {
        private readonly ILogger<SheduleUpdateHostedService> logger;
        private readonly IConfiguration configuration;
        private readonly HttpClient httpClient;
        private Task task;

        public SheduleUpdateHostedService(ILogger<SheduleUpdateHostedService> logger,
            IHttpClientFactory httpClient,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.httpClient = httpClient.CreateClient();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Request background service is started");

            task = ExecuteAsync(cancellationToken);
            if (task.IsCompleted)
            {
                return task;
            }

            return Task.CompletedTask;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Log(stoppingToken);
                await Log1(stoppingToken);
            }

        }
        private async Task Log(CancellationToken stoppingToken)
        {
            //await httpClient.GetAsync(new Uri(configuration.GetSection("URL")["Mirea.ru"]));
            logger.LogInformation("WORKER SERVICE");
            await Task.Delay(1000, stoppingToken);
        }
        private async Task Log1(CancellationToken stoppingToken)
        {
            logger.LogInformation("WORKER SERVICE__2");
            await Task.Delay(2000, stoppingToken);
        }
    }
}
