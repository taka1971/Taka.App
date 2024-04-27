namespace Taka.App.Deliverer.Domain.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(byte[] fileData, string fileName);
    }
}
