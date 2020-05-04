using System;
using System.IO;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.Media;
using Plugin.Media.Abstractions;

using System.Net;
using Newtonsoft.Json;
using System.Net.Http;

using pms.Models;
using pms.ViewModels;

namespace pms.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        ProcessedImageViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ProcessedImageViewModel();
        }

        async void OnProcessedImageSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var processedImage = (ProcessedImage)layout.BindingContext;
            await Navigation.PushAsync(new ProcessedImageDetailPage(new ProcessedImageDetailViewModel(processedImage)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.ProcessedImages.Count == 0)
                viewModel.IsBusy = true;
        }

        // Reads the given photo and transfers it for analyze
        async Task ReadPhotoAsync(MediaFile photo)
        {
            if (photo != null)
            {
                byte[] imageData;

                using (var memoryStream = new MemoryStream())
                {
                    photo.GetStream().CopyTo(memoryStream);
                    photo.Dispose();

                    imageData = memoryStream.ToArray();
                }

                // POST form
                MultipartFormDataContent formData = new MultipartFormDataContent();
                formData.Add(new ByteArrayContent(imageData), "base_image");

                // Sends the image
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PostAsync(ProcessedImageViewModel.URL_UPLOAD_IMAGE, formData);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Updates the LastID property when a new image is added
                    viewModel.LastID++;

                    // Loads the last image
                    var imageJson = await httpClient.GetStringAsync(ProcessedImageViewModel.URL_LOAD_IMAGE_BY_ID + viewModel.LastID);

                    // Adds the image to the beginning of the list
                    ProcessedImage image = JsonConvert.DeserializeObject<ProcessedImage>(imageJson);
                    viewModel.ProcessedImages.Insert(0, image);
                }
                else
                {
                    Console.WriteLine("Error while loading the last processed image...");
                }
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
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
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
            if (!CrossMedia.Current.IsPickPhotoSupported)
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
                await ReadPhotoAsync(photo);
            }
        }

        // Click on Pick Photo button
        async void MediaPickButton_OnClicked(object sender, EventArgs e)
        {
            // Picks a photo 
            if (await CheckStorageReadPermission() && await CheckPickPhotoPlugin())
            {
                var photo = await CrossMedia.Current.PickPhotoAsync();
                await ReadPhotoAsync(photo);
            }
        }

        // Click on Load More Images button
        async void LoadMoreImagesButton_OnClicked(object sender, EventArgs e)
        {
            bool canLoadMore = await viewModel.LoadProcessedImages();

            if (! canLoadMore)
            {
                Button btn = (Button)sender;
                btn.IsVisible = false;
            }
        }
    }
}
