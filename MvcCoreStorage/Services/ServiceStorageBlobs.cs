using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MvcCoreStorage.Models;

namespace MvcCoreStorage.Services
{
    public class ServiceStorageBlobs
    {
        private BlobServiceClient client;

        public ServiceStorageBlobs(BlobServiceClient client)
        {
            this.client = client;
        }

        public async Task<List<string>> GetContainersAsync()
        {
            List<string> containers = new List<string>();
            await foreach(BlobContainerItem item in 
                this.client.GetBlobContainersAsync()) 
            {
                containers.Add(item.Name);
            }
            return containers;
        }


        public async Task CreateContainerAsync(string containerName)
        {
           await this.client.CreateBlobContainerAsync
                (containerName, PublicAccessType.Blob);
        }

        public async Task DeleteContainerAsync(string containerName)
        {
            await this.client.DeleteBlobContainerAsync
                 (containerName);
        }

        public async Task<List<BlobModel>> GetBlobsAsync
            (string containerName)
        {
            BlobContainerClient containerClient =
                this.client.GetBlobContainerClient(containerName);
            List<BlobModel> blobModels = new List<BlobModel>();
            await foreach(BlobItem item in containerClient.GetBlobsAsync()) 
            {
               BlobClient blobClient = 
                    containerClient.GetBlobClient(item.Name);
                BlobModel model = new BlobModel();
                model.Nombre = item.Name;
                model.Contenedor = containerName;
                model.Url = blobClient.Uri.AbsoluteUri;
                blobModels.Add(model);
            
            }
            return blobModels;
        }


        public async Task DeleteBlobAsync
            (string containerName, string blobName)
        {
            BlobContainerClient containerClient =
                this.client.GetBlobContainerClient (containerName); 
            await containerClient.DeleteBlobAsync (blobName);
        }

        public async Task UploadBlobAsync
            (string containerName, string blobName, Stream stream)
        {
            BlobContainerClient containerClient =
                this.client.GetBlobContainerClient(containerName);
            await containerClient.UploadBlobAsync(blobName, stream);
        }


    }
}
