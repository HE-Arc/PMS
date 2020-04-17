using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Plugin.Media;
using Plugin.Media.Abstractions;

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
        }

        // TODO: transfer the photo for analyze
        void ReadPhoto(MediaFile photo)
        {
            if (photo != null)
            {
                PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
            }
        }

        // Checks wether the application has the camera permission
        async Task<bool> CheckCameraPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();

                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Camera is required", "You need to add camera permission.", "OK");

                    return false;
                }
            }

            return true;
        }

        // Checks wether the application has the storage read permission
        async Task<bool> CheckStorageReadPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageRead>();

                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Storage Read is required", "You need to add storage read permission.", "OK");

                    return false;
                }
            }

            return true;
        }

        // Checks wether the camera plugin is available and supported
        async Task<bool> CheckCameraPlugin()
        {
            if (! CrossMedia.Current.IsCameraAvailable || ! CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Camera not supported", "No camera available.", "OK");
                CameraButton.IsEnabled = false;

                return false;
            }

            return true;
        }

        // Checks wether the pick photo plugin is available and supported
        async Task<bool> CheckPickPhotoPlugin()
        {
            if (! CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Pick photo not supported", "No media picker available.", "OK");
                MediaPickButton.IsEnabled = false;

                return false;
            }

            return true;
        }

        // Click on Take Photo button
        async void CameraButton_OnClicked(object sender, EventArgs e)
		{
            if (await CheckCameraPermission() && await CheckCameraPlugin())
            {
                // Supplies media options for saving the photo after it is taken
                var mediaOptions = new StoreCameraMediaOptions
                {
                    Directory = "Samples",
                    Name = $"{DateTime.UtcNow}.jpg"
                };

                // Takes a photo
                var photo = await CrossMedia.Current.TakePhotoAsync(mediaOptions);
                ReadPhoto(photo);
            }
        }

        // Click on Pick Photo button
        async void MediaPickButton_OnClicked(object sender, EventArgs e)
        {
            // Picks a photo 
            if (await CheckStorageReadPermission() && await CheckPickPhotoPlugin())
            {
                var photo = await CrossMedia.Current.PickPhotoAsync();
                ReadPhoto(photo);
            }
        }
    }
}