using AzureDownloaders.Models;
using AzureDownloaders.Services.Interface;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AzureDownloaders.Services
{
    public class DownloadService : IDownloadService
    {        
        //private string filePath = Path.Combine("/downloadoutput");
        private string filePath = Path.Combine(@"e:\imgtest2");


        private string SetDirectories(DownloadMessage message)
        {
            string returnDir = string.Empty;
            
            returnDir = Path.Combine(filePath, message.username.Trim());
            if (!Directory.Exists(returnDir))
            {
                Directory.CreateDirectory(returnDir);
            }


            if (!string.IsNullOrWhiteSpace(message.title))
            {
                var sanitizedTitle = Regex.Replace(message.title, "[^a-zA-Z0-9 _]", string.Empty);
                var fileWithTitle = Path.Combine(returnDir, sanitizedTitle.Trim());
                Directory.CreateDirectory(fileWithTitle);
                returnDir = fileWithTitle;
            }

            return returnDir;
        }

        public async Task DownloadImages(DownloadMessage message)
        {
            string path = SetDirectories(message);
            
            int counter = 1;
            foreach(var image in message.media)
            {
                using (WebClient client = new WebClient())
                {
                    //client.DownloadFile(new Uri(imageUrl), Path.Combine(path, $"{counter}.jpg"));
                    var data = await client.DownloadDataTaskAsync(new Uri(image.imageUrl));
                    var contentType = client.ResponseHeaders["Content-Type"];


                    if(image.fileName != null)
                    {
                        string fileName = Regex.Replace(image.fileName, "[^a-zA-Z0-9 _.]", string.Empty);

                        await File.WriteAllBytesAsync(Path.Combine(path, $"{fileName}"), data);                        
                    }
                    else
                    {
                        await File.WriteAllBytesAsync(Path.Combine(path, $"{counter}.jpg"), data);                        
                    }
                    counter++;
                }
            }
        }        
    }
}
