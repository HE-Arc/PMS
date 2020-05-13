using System.ComponentModel;
using Xamarin.Forms;

using pms.ViewModels;
using Xamarin.Forms.Internals;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;

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
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        async void PlusButton_OnClicked(object sender, EventArgs e)
        {
            viewModel.Count += 1;
            await UpdateCountAsync(viewModel.ProcessedImage.id, viewModel.Count);
        }

        async void MinusButton_OnClicked(object sender, EventArgs e)
        {
            viewModel.Count -= 1;
            await UpdateCountAsync(viewModel.ProcessedImage.id, viewModel.Count);
        }

        async Task UpdateCountAsync(int id, int count)
        {
            var httpClient = new HttpClient();
            var url = "https://pms.srvz-webapp.he-arc.ch/api/update/{id}/{count}";

            var processResponse = await httpClient.GetAsync(url);

            if(processResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Debug.WriteLine("OK");
            }

            Debug.WriteLine("TEST");
        }
    }
}
