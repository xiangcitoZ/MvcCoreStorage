using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;

namespace MvcCoreStorage.Services
{
    public class ServiceStorageFiles
    {
        private ShareDirectoryClient root;

        public ServiceStorageFiles(IConfiguration configuration)
        {
            string azureKeys =
                configuration.GetValue<string>("AzureKeys:StorageAccount");
            ShareClient shareClient =
                new ShareClient(azureKeys, "ejemplosfiles");
            this.root = shareClient.GetRootDirectoryClient();
        }

        public async Task<List<string>> GetFilesAsync()
        {
            List<string> files = new List<string>();

            await foreach(ShareFileItem item in 
                this.root.GetFilesAndDirectoriesAsync()) 
            {
                files.Add(item.Name);
            }
            return files;
        }
            
        public async Task<string> ReadFileAsync(string fileName)
        {
            ShareFileClient file = this.root.GetFileClient(fileName);
            ShareFileDownloadInfo data = await file.DownloadAsync();
            Stream stream = data.Content;
            string contenido = "";
            using(StreamReader reader = new StreamReader(stream)) 
            {
                contenido = await reader.ReadToEndAsync();
            }
            return contenido;
        }

        public async Task UploadFileAsync(string fileName,
            Stream stream)
        {
            ShareFileClient file = this.root.GetFileClient(fileName);

            await file.CreateAsync(stream.Length);
            await file.UploadAsync(stream);

        }

        public async Task DeleteFilesAsync(string fileName)
        {
            ShareFileClient file = this.root.GetFileClient(fileName);
            await file.DeleteAsync();
        }
    }
}
