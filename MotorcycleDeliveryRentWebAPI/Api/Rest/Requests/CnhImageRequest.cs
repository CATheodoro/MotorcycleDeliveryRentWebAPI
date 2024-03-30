using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Requests
{
    public class CnhImageRequest
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }


        internal static CnhImageModel Convert(string fileName, string filePath, long fileSize)
        {
            CnhImageModel model = new CnhImageModel();
            model.FileName = fileName;
            model.FilePath = filePath;
            model.FileSize = fileSize;
            return model;
        }
    }
}
