using AzureDownloaders.Models;
using AzureDownloaders.Services.Interface;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AzureDownloaders.Services
{
    public class DownloadService : IDownloadService
    {        
        //private string filePath = Path.Combine("/downloadoutput");
        private string filePath = Path.Combine("e:\\");


        private string SetDirectories(RabbitMQMessage message)
        {
            string returnDir = string.Empty;
            
            returnDir = Path.Combine(filePath, message.Username, message.SiteName);
            if (!Directory.Exists(returnDir))
            {
                Directory.CreateDirectory(returnDir);
            }


            if (message.PublishDate != DateTime.MinValue)
            {
                var fileWithDate = Path.Combine(returnDir, message.PublishDate.ToString("d").Replace("/", "-"));
                Directory.CreateDirectory(fileWithDate);
                returnDir = fileWithDate;
            }
            else if (!string.IsNullOrWhiteSpace(message.Title))
            {
                var fileWithTitle = Path.Combine(returnDir, message.Title);
                Directory.CreateDirectory(fileWithTitle);
                returnDir = fileWithTitle;
            }

            return returnDir;
        }

        public async Task DownloadImages(RabbitMQMessage message)
        {
            string path = SetDirectories(message);
            int counter = 1;
            foreach (string imageUrl in message.ImageList)
            {                
                using(WebClient client = new WebClient())
                {                                      
                    //client.DownloadFile(new Uri(imageUrl), Path.Combine(path, $"{counter}.jpg"));
                    var data = await client.DownloadDataTaskAsync(new Uri(imageUrl));
                    var contentType = client.ResponseHeaders["Content-Type"];
                    await File.WriteAllBytesAsync(Path.Combine(path, $"{counter}.{GetFileExt(contentType)}"), data);
                }
                counter++;
            }
        }

        private string GetFileExt(string mimeType)
        {
            string ext = string.Empty;

            switch (mimeType)
            {
                case "image/jpeg": ext = "jpg";
                    break;

                case "video/mp4": ext = "mp4";
                    break;
            }

            return ext;
        }

    }
}
