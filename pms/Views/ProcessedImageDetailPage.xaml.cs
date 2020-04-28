using System.ComponentModel;
using Xamarin.Forms;

using pms.ViewModels;

namespace pms.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ProcessedImageDetailPage : ContentPage
    {
        ProcessedImageDetailViewModel viewModel;

        public ProcessedImageDetailPage(ProcessedImageDetailViewModel viewModel)
        {
            //InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }
    }
}
