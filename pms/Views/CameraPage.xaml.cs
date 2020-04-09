using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace pms.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class CameraPage : ContentPage
    {
        public CameraPage()
        {
            InitializeComponent();

            CameraButton.Clicked += CameraButton_Clicked;
        }

        private async void CameraButton_Clicked(object sender, EventArgs e)
        {
            if (! Plugin.Media.CrossMedia.Current.IsCameraAvailable ||
                    !Plugin.Media.CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No camera", ":(No camera available.", "OK");
            }

            var file = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
            {
                return;
            }

            await DisplayAlert("File Location", file.Path, "OK");

            ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();

                return stream;
            });
        }
    }
}