using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;

using pms.Models;
using pms.Services;

namespace pms.ViewModels
{
    public class ProcessedImageViewModel : BaseViewModel
    {
        public static string URL_UPLOAD_IMAGE = "https://pms.srvz-webapp.he-arc.ch/api/upload";
        public static string URL_PROCESS_IMAGE = "https://pms.srvz-webapp.he-arc.ch/api/process/";
        public static string URL_LOAD_IMAGES = "https://pms.srvz-webapp.he-arc.ch/api/images";
        public static string URL_LOAD_IMAGE_BY_ID = "https://pms.srvz-webapp.he-arc.ch/api/image/";

        public MockDataStore DataStore { get; set;}
        public ObservableCollection<ProcessedImage> ProcessedImages { get; set; }
        public Command LoadProcessedImageCommand { get; set; }

        public int LastID { get; set; }
        public int FromID { get; set; }

        private bool refreshIsVisible;
        public bool RefreshIsVisible
        {
            get { return refreshIsVisible; }
            set
            {
                refreshIsVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _activityIndicatorContainerVisible;
        public bool ActivityIndicatorContainerVisible
        {
            get
            {
                return _activityIndicatorContainerVisible;
            }
            set
            {
                _activityIndicatorContainerVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _activityIndicatorRunning;
        public bool ActivityIndicatorIsRunning {
            get
            {
                return _activityIndicatorRunning;
            }
            set
            {
                _activityIndicatorRunning = value;
                OnPropertyChanged();
            }
        }

        public ProcessedImageViewModel()
        {
            Title = "PMS";
            DataStore = new MockDataStore();
            ProcessedImages = new ObservableCollection<ProcessedImage>();
            LoadProcessedImageCommand = new Command(async () => await ExecuteLoadProcessedImagesCommand());
        }

        public async Task ExecuteLoadProcessedImagesCommand()
        {
            IsBusy = true;

            try
            {
                ProcessedImages.Clear();
                FromID = 0;

                await LoadProcessedImages();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Loads processed images from the backend API
        public async Task LoadProcessedImages()
        {
            ActivityIndicatorContainerVisible = true;
            ActivityIndicatorIsRunning = true;

            // First load
            string url = URL_LOAD_IMAGES;

            // Loads from the given id
            if (FromID > 0)
            {
                url += "?from_id=" + FromID;
            }

            // Loads the images
            var httpClient = new HttpClient();
            var imagesData = await httpClient.GetStringAsync(url);

            // Adds the images to the list
            List<ProcessedImage> processedImages = JsonConvert.DeserializeObject<List<ProcessedImage>>(imagesData);
            foreach (ProcessedImage processedImage in processedImages)
            {
                ProcessedImages.Add(processedImage);
            }

            // Updates the FromID property
            if (processedImages.Count > 0)
            {
                FromID = ProcessedImages[ProcessedImages.Count - 1].id;

                if (FromID > 1)
                {
                    RefreshIsVisible = true;
                }
                else
                {
                    RefreshIsVisible = false;
                }
            }
            else
            {
                FromID = 0;
                RefreshIsVisible = false;
            }

            ActivityIndicatorIsRunning = false;
            ActivityIndicatorContainerVisible = false;
        }
    }
}
