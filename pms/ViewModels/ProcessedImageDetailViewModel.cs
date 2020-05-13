using pms.Models;
using System.Diagnostics;

namespace pms.ViewModels
{
    public class ProcessedImageDetailViewModel : BaseViewModel
    {
        public ProcessedImage ProcessedImage { get; set; }

        public int Count
        {
            get
            {
                return ProcessedImage.count;
            }
            set
            {
                ProcessedImage.count = value;
                OnPropertyChanged("ProcessedImage");
            }
        }

        public ProcessedImageDetailViewModel(ProcessedImage processedImage = null)
        {
            Title = processedImage.datetime;
            ProcessedImage = processedImage;
        }
    }
}
