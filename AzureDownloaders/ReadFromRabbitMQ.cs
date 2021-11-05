using System;
using System.Threading.Tasks;
using AzureDownloaders.Models;
using AzureDownloaders.Services.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureDownloaders
{
    public class ReadFromRabbitMQ
    {
        private readonly IDownloadService _service;

        public ReadFromRabbitMQ(IDownloadService service)
        {
            _service = service;
        }

        [Function("ReadFromRabbitMQ")]
        public async Task Run([RabbitMQTrigger("downloads", ConnectionStringSetting = "RabbitMQ")] RabbitMQMessage rabbitMessage, FunctionContext context)
        {
            var logger = context.GetLogger("ReadFromRabbitMQ");
            logger.LogInformation($"C# Queue trigger function processed: {rabbitMessage}");

            //_service.DownloadImages(rabbitMessage);
        }
    }
}
