
namespace pms.Models
{
    public class ProcessedImage
    {
        // .NET properties naming convention not respected here because the backend
        // API is developped in Django ; maybe it is possible to format the data
        // before sending it here in order to respect the naming convention.

        public string id { get; set; }

        public string base_image { get; set; }

        public string processed_image { get; set; }

        public string datetime { get; set; }

        public int count { get; set; }
    }
}
