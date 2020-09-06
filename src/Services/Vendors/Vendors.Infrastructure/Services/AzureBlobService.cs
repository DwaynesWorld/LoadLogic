using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LoadLogic.Services.Vendors.Application.Interfaces;

namespace LoadLogic.Services.Vendors.Infrastructure.Services
{
    public class AzureBlobService : IBlobService
    {
        private readonly BlobServiceClient _client;
        public AzureBlobService(string connectionString)
        {
            _client = new BlobServiceClient(connectionString);
        }

        public Task<Stream> GetFileAsync(string containerName, string fileName, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Uri> UploadFileAsync(
            Stream content, string containerName,
            string fileName, CancellationToken cancellationToken = default)
        {
            var containerClient = await GetContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(content, cancellationToken);
            return blobClient.Uri;
        }

        private async Task<BlobContainerClient> GetContainerClient(string containerName)
        {
            var containerClient = _client.GetBlobContainerClient(containerName);
            var exists = await containerClient.ExistsAsync();
            if (!exists)
            {
                _ = await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
            }
            return containerClient;
        }
    }
}
