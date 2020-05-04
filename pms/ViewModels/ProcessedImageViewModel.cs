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
        public static string URL_LOAD_IMAGES = "https://pms.srvz-webapp.he-arc.ch/api/images";
        public static string URL_LOAD_IMAGE_BY_ID = "https://pms.srvz-webapp.he-arc.ch/api/image/";

        public MockDataStore DataStore { get; set;}
        public List<ProcessedImage> ProcessedImages { get; set; }
        public Command LoadProcessedImageCommand { get; set; }

        public int LastID { get; set; }
        public int FromID { get; set; }

        public ProcessedImageViewModel()
        {
            Title = "PMS";
            DataStore = new MockDataStore();
            ProcessedImages = new List<ProcessedImage>();
            LoadProcessedImageCommand = new Command(async () => await ExecuteLoadProcessedImagesCommand());
        }

        async Task ExecuteLoadProcessedImagesCommand()
        {
            IsBusy = true;

            try
            {
                ProcessedImages.Clear();

                // TODO
                // LoadProcessedImages();
                // Remove code below then
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    ProcessedImages.Add(item);
                }
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
        public async Task<bool> LoadProcessedImages()
        {
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
            FromID = ProcessedImages[ProcessedImages.Count - 1].id - 1;

            // Can load more images
            return FromID > 0;
        }
    }
}
