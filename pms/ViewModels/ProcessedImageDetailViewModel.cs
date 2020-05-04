using pms.Models;

namespace pms.ViewModels
{
    public class ProcessedImageDetailViewModel : BaseViewModel
    {
        public ProcessedImage ProcessedImage { get; set; }

        public ProcessedImageDetailViewModel(ProcessedImage processedImage = null)
        {
            Title = processedImage.datetime;
            ProcessedImage = processedImage;
        }
    }
}
