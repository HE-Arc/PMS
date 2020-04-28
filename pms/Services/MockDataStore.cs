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
                new ProcessedImage { Id = Guid.NewGuid().ToString(), Datetime = "26.04.2020 17:18", Image = "img", Count = 100 },
                new ProcessedImage { Id = Guid.NewGuid().ToString(), Datetime = "27.04.2020 17:18", Image = "img", Count = 100 },
                new ProcessedImage { Id = Guid.NewGuid().ToString(), Datetime = "28.04.2020 17:18", Image = "img", Count = 100 },
                new ProcessedImage { Id = Guid.NewGuid().ToString(), Datetime = "28.04.2020 17:18", Image = "img", Count = 100 },
                new ProcessedImage { Id = Guid.NewGuid().ToString(), Datetime = "28.04.2020 17:18", Image = "img", Count = 100 },
            };
        }

        public async Task<bool> AddItemAsync(ProcessedImage processedImage)
        {
            processedImages.Add(processedImage);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(ProcessedImage processedImage)
        {
            var oldProcessedImage = processedImages.Where((ProcessedImage arg) => arg.Id == processedImage.Id).FirstOrDefault();
            processedImages.Remove(oldProcessedImage);
            processedImages.Add(processedImage);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldProcessedImage = processedImages.Where((ProcessedImage arg) => arg.Id == id).FirstOrDefault();
            processedImages.Remove(oldProcessedImage);

            return await Task.FromResult(true);
        }

        public async Task<ProcessedImage> GetItemAsync(string id)
        {
            return await Task.FromResult(processedImages.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<ProcessedImage>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(processedImages);
        }
    }
}
