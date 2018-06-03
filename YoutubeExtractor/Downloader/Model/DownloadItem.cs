using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Downloader.Model
{
    public class DownloadItem : INotifyPropertyChanged
    {
        public int Order { get; set; }
        public string Url { get; set; }

        private double downloadPercent;
        public double DownloadedPercent {
            get {
                return this.downloadPercent;
            }
            set {
                this.downloadPercent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DownloadedPercent"));
            }
        }

        public string Status { get; set; }

        private string comment;
        public string Comment
        {
            get
            {
                return this.comment;
            }
            set
            {
                this.comment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Comment"));
            }
        }

        public FileInfo Target { get; set; }
        public Guid Id { get; set; }

        public DownloadItem()
        {
            this.Id = Guid.NewGuid();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}
