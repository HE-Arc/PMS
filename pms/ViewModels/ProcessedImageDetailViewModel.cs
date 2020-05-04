using pms.Models;
using System.Diagnostics;

namespace pms.ViewModels
{
    public class ProcessedImageDetailViewModel : BaseViewModel
    {
        private int count;

        public string ProcessedImage { get; set; }

        public string BaseImage { get; set; }

        public int Count 
        {
            get { return count; }
            set { count = value; OnPropertyChanged("Count"); }
        }

        public ProcessedImageDetailViewModel(ProcessedImage processedImage = null)
        {
            Title = processedImage.datetime;
            ProcessedImage = processedImage.processed_image;
            BaseImage = processedImage.base_image;
            Count = processedImage.count;
        }
    }
}
