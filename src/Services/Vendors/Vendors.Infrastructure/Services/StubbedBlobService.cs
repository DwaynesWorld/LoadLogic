using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Vendors.Application.Interfaces;

namespace LoadLogic.Services.Vendors.Infrastructure.Services
{
    public class StubbedBlobService : IBlobService
    {
        public StubbedBlobService()
        {
        }

        public Task<Stream> GetFileAsync(
            string containerName, string fileName,
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<Uri> UploadFileAsync(
            Stream content, string containerName,
            string fileName, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
