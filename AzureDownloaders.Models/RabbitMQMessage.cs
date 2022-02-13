using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDownloaders.Models
{
    public class RabbitMQMessage
    {
        public Guid Id { get; set; }

        public string SiteName { get; set; }

        public string Username { get; set; }

        public string? Title { get; set; }

        public List<MediaItem> ImageList { get; set; }

        public DateTime PublishDate { get; set; }
        public string Type { get; set; }
    }

    public class MediaItem
    {
        public string Type { get; set; }

        public string Url { get; set; }
    }
}
