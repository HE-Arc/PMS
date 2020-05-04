using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pms.Models;

namespace pms.Services
{
    public class MockDataStore : IDataStore<ProcessedImage>
    {
        readonly List<ProcessedImage> processedImages;

        public MockDataStore()
        {
            processedImages = new List<ProcessedImage>
            {
                new ProcessedImage { id = 1, base_image = "", processed_image = "", datetime = "26.04.2020 17:18", count = 100 },
                new ProcessedImage { id = 2, base_image = "", processed_image = "", datetime = "27.04.2020 17:18", count = 100 },
                new ProcessedImage { id = 3, base_image = "", processed_image = "", datetime = "28.04.2020 17:18", count = 100 },
                new ProcessedImage { id = 4, base_image = "", processed_image = "", datetime = "28.04.2020 17:18", count = 100 },
                new ProcessedImage { id = 5, base_image = "", processed_image = "", datetime = "28.04.2020 17:18", count = 100 },
            };
        }

        public async Task<bool> AddItemAsync(ProcessedImage processedImage)
        {
            processedImages.Add(processedImage);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(ProcessedImage processedImage)
        {
            var oldProcessedImage = processedImages.Where((ProcessedImage arg) => arg.id == processedImage.id).FirstOrDefault();
            processedImages.Remove(oldProcessedImage);
            processedImages.Add(processedImage);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var oldProcessedImage = processedImages.Where((ProcessedImage arg) => arg.id == id).FirstOrDefault();
            processedImages.Remove(oldProcessedImage);

            return await Task.FromResult(true);
        }

        public async Task<ProcessedImage> GetItemAsync(int id)
        {
            return await Task.FromResult(processedImages.FirstOrDefault(s => s.id == id));
        }

        public async Task<IEnumerable<ProcessedImage>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(processedImages);
        }
    }
}
