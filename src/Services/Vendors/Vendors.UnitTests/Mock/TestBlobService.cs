using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Vendors.Application.Interfaces;
using MediatR;

namespace LoadLogic.Services.Vendors.UnitTests
{
    public class TestBlobService : IBlobService
    {
        public Task<Stream> GetFileAsync(string containerName, string fileName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Uri> UploadFileAsync(Stream content, string containerName, string fileName, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new Uri("http://example.com"));
        }
    }
}
