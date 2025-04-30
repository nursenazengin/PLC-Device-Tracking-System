using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TrackingSystemAPI.BGservices;

namespace MyProject.BGServices
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly ILogger<MyBackgroundService> logger;
        private readonly TagDataService tagDataService;

        public MyBackgroundService( ILogger<MyBackgroundService> logger, 
                                    TagDataService tagDataService
                                   )
        {
            this.logger = logger;
            this.tagDataService = tagDataService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var tagTask = tagDataService.AddTagDataAsync();

            await Task.WhenAll(tagTask); 
        }
    }
    }


