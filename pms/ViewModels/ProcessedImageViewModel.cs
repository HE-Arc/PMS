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
        public ObservableCollection<ProcessedImage> ProcessedImages { get; set; }
        public Command LoadProcessedImageCommand { get; set; }

        public int LastID { get; set; }

        public ProcessedImageViewModel()
        {
            Title = "PMS";
            DataStore = new MockDataStore();
            ProcessedImages = new ObservableCollection<ProcessedImage>();
            LoadProcessedImageCommand = new Command(async () => await ExecuteLoadProcessedImagesCommand());
            /*
            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
            */
        }

        async Task ExecuteLoadProcessedImagesCommand()
        {
            IsBusy = true;

            try
            {
                ProcessedImages.Clear();

                //LoadProcessedImages();

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
        public async void LoadProcessedImages(int from_id = 0)
        {
            // First load
            string url = URL_LOAD_IMAGES;

            // Loads from the given id
            if (from_id > 0)
            {
                url += "?from_id=" + from_id;
            }

            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(url);

            var images = JsonConvert.DeserializeObject<List<ProcessedImage>>(response);
            //var image = JsonConvert.DeserializeObject(response);

            //TODO: LastID
            LastID = 0;

            foreach (var img in images)
            {
                //TODO: Add images to ProcessedImages list
                
            }
        }
    }
}
