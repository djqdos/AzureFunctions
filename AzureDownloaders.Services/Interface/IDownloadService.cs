using AzureDownloaders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDownloaders.Services.Interface
{
    public interface IDownloadService
    {
        //Task DownloadImages(RabbitMQMessage message);

        Task DownloadImages(DownloadMessage message);
    }
}
