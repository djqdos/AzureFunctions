using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDownloaders.Models
{


    public class DownloadMessage
    {
        public string username { get; set; }
        public string postid { get; set; }
        public string title { get; set; }
        public string[] tags { get; set; }
        public Medium[] media { get; set; }
        public string thumbnail { get; set; }
    }

    public class Medium
    {
        public string imageUrl { get; set; }
        public string fileName { get; set; }
    }


}
