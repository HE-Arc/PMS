using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using pms.Models;
using pms.Services;

namespace pms.ViewModels
{
    public class ProcessedImageViewModel : BaseViewModel
    {
        public MockDataStore DataStore { get; set;}
        public ObservableCollection<ProcessedImage> ProcessedImages { get; set; }
        public Command LoadProcessedImageCommand { get; set; }

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
    }
}
