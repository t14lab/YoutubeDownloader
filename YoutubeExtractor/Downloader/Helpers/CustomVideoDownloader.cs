using Downloader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace Downloader.Helpers
{
    public class CustomVideoDownloader : VideoDownloader
    {
        public DownloadItem DownloadItem { get; set; }
        public CustomVideoDownloader(DownloadItem downloadItem, 
            VideoInfo video, string savePath, int? bytesToDownload = default(int?)) 
            : base(video, savePath, bytesToDownload)
        {
            this.DownloadItem = downloadItem;
        }
    }
}
