using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AzureDownloaders.Models;
using AzureDownloaders.Services.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureDownloaders
{    
    public class PushToRabbitMQ
    {
        private readonly IDownloadService _service;

        public PushToRabbitMQ(IDownloadService service)
        {
            _service = service;
        }


        [Function("PushToRabbitMQ")]
        [RabbitMQOutput(QueueName = "downloads", ConnectionStringSetting = "RabbitMQ")]
        public async Task<object> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,            
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("Function1");
            logger.LogInformation("C# HTTP trigger function processed a request.");


            var response = await new StreamReader(req.Body).ReadToEndAsync();
            RabbitMQMessage message = JsonConvert.DeserializeObject<RabbitMQMessage>(response);
            if(message.Id == Guid.Empty)
            {
                message.Id = Guid.NewGuid();
            }
            
            return message;
        }
    }
}
